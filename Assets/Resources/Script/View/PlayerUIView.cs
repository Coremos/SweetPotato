using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerUIView : UIView
{
    private const float SECOND_DIVIDER = 1.0f / 60.0f;
    public Sprite[] SunAndMoon;         // 0 : sun, 1 : moon
    public Image SunMoonImg;            // 달, 해 이미지 표시 
    public Scrollbar SunMoonScrollBar;  // 
    public TMP_Text DayText;            // 날짜
    public TMP_Text SeedCnt;            // 씨앗 소지 개수

    private float accCheck;
    private float accGauge;

    private void Update()
    {
        accCheck += Time.deltaTime;
        accGauge += Time.deltaTime;

        SunMoonScrollBar.size = accGauge * SECOND_DIVIDER;
        SunMoonImg.sprite = SunMoonScrollBar.size < 0.5f ? SunAndMoon[0] : SunAndMoon[1];

        int day = (int)(accCheck * SECOND_DIVIDER) + 1;
        DayText.text = string.Format("Day {0}", day);
        SeedCnt.text = GameManager.Instance.GetPlayerSeed() + "/" + GameManager.Instance.MaxSeedCnt;

        if (day > GameManager.Instance.DayCnt)
        {
            GameManager.Instance.GoToNextStage();
            SunMoonScrollBar.size = 0f;
            accGauge = 0f;
        }
    }

    public void OnClickPauseBtn()
    {
        GameManager.Instance.PauseGame(true);
        PausePopup.Show(null, StopPause);
    }
    private void StopPause()
    {
        GameManager.Instance.PauseGame(false);
    }
}
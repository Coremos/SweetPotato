using UnityEngine;
using UnityEngine.UI;

public class StatusBar : MonoBehaviour
{
    private readonly Vector3 offset = new Vector3(0f, 75f, 0f);

    private RectTransform transformRect;
    private Camera Cam;
    private Canvas canvas;
    private Transform Target;
    private Slider StatBar;

    private float maxValue;
    private float curValue;

    private void OnEnable()
    {
        transformRect = GetComponent<RectTransform>();
        Cam = Camera.main;
        canvas = Cam.transform.GetComponentInChildren<Canvas>();
        StatBar = GetComponent<Slider>();
    }

    public void Initialize(Transform target)
    {
        Target = target;
    }

    public void SetValue(float curValue, float maxValue)
    {
        this.maxValue = maxValue;
        this.curValue = curValue;

        StatBar.value = curValue / maxValue;
        if (StatBar.value <= 0f)
        {
            gameObject.SetActive(false);
        }
    }

    private void Update()
    {
        var screenPos = Cam.WorldToScreenPoint(Target.position + offset);

        if (screenPos.z < 0.0f)
            screenPos *= -1.0f;

        var localPos = Vector2.zero;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            canvas.GetComponent<RectTransform>(),
            screenPos,
            Cam,
            out localPos); // 스크린 좌표를 다시 체력바 UI 캔버스 좌표로 변환

        transformRect.localPosition = localPos;
    }
}
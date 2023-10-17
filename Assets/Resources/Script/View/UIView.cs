using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(CanvasGroup))]
public class UIView : MonoBehaviour
{
    [HideInInspector]
    public UnityAction<string> InitAction = null;   // 외부 호출 => UIViewManager 에서 호출 예정

    [Header("UI")]
    private CanvasGroup FadeCanvasGrp;
    public GameObject[] EffectList;         // 해당 뷰에 할당된 모든 이펙트 오브젝트

    [Header("Variable")]
    private readonly float TotalPlayTime = 0.15f;
    private float DelayTimeShowViewEffect = 0f; // 뷰에 할당된 이펙트가 출력되기까지 지연시간
    private float DelayTimeHideViewEffect = 0f; // 뷰에 할당된 이펙트가 비활성화되기까지 지연시간
    private float DelayTimeShowView = 0f;       // 뷰가 출력되기까지 지연시간
    private float DelayTimeHideView = 0f;       // 뷰가 비활성화 되기 전까지 지연시간
    private bool isEffectShowing;



    private void Awake()
    {
        FadeCanvasGrp = this.GetComponent<CanvasGroup>();
        if (FadeCanvasGrp == null)
            FadeCanvasGrp = this.gameObject.AddComponent<CanvasGroup>();
    }



    /// <summary>
    /// 순서
    ///  Show -> CoreShow -> ShowHideEffect -> DoPlayFadeIn
    ///  Hide -> CoreHide -> ShowHideEffect -> DoPlayFadeOut
    /// 주의
    ///  ShowHideEffect 호출 전 isEffectShowing 변수 수정에 유의할것.
    /// </summary>
    public float Show(bool isDirectlyShow = false)
    {
        if (DelayTimeShowView > 0f && isDirectlyShow)
        {
            Invoke(nameof(CoreShow), DelayTimeShowView);
            return DelayTimeShowView;
        }
        CoreShow();
        return 0f;
    }
    public float Hide(bool isDirectlyHide = false)
    {
        if (DelayTimeHideView > 0f && isDirectlyHide)
        {
            Invoke(nameof(CoreHide), DelayTimeHideView);
            return DelayTimeHideView;
        }
        CoreHide();
        return 0f;
    }
    private void CoreShow()
    {
        if (FadeCanvasGrp)
            FadeCanvasGrp.alpha = 0f;

        // 코루틴 오류가 예상되어, 실제동작 부분을 따로 분리하였음.
        this.gameObject.SetActive(true);

        // 외부에서 호출
        //InitAction?.Invoke(viewData.DATA);

        isEffectShowing = true;             // ShowHideEffect 함수는 해당 변수의 통제를 따름
        if (DelayTimeShowViewEffect <= 0f)
            ShowHideEffect();
        else
            Invoke(nameof(ShowHideEffect), DelayTimeShowViewEffect);

        if (FadeCanvasGrp)
            StartCoroutine(DoPlayFadeIn());
    }
    private void CoreHide()
    {
        isEffectShowing = false;            // ShowHideEffect 함수는 해당 변수의 통제를 따름
        if (DelayTimeHideViewEffect <= 0f)
            ShowHideEffect();
        else
            Invoke(nameof(ShowHideEffect), DelayTimeHideViewEffect);

        if (FadeCanvasGrp)
            StartCoroutine(DoPlayFadeOut());
    }
    private void ShowHideEffect()
    {
        foreach (var effect in EffectList)
            effect.gameObject.SetActive(isEffectShowing);
    }
    IEnumerator DoPlayFadeIn()
    {
        float t = 0.0f;
        while (t < TotalPlayTime)
        {
            t += Time.unscaledDeltaTime;
            FadeCanvasGrp.alpha = t / TotalPlayTime;
            yield return null;
        }

        FadeCanvasGrp.alpha = 1.0f;
    }
    IEnumerator DoPlayFadeOut()
    {
        float t = 0.0f;
        while (t < TotalPlayTime)
        {
            t += Time.unscaledDeltaTime;
            FadeCanvasGrp.alpha = 1 - t / TotalPlayTime;
            yield return null;
        }

        FadeCanvasGrp.alpha = 0.0f;
        this.gameObject.SetActive(false);
    }

}

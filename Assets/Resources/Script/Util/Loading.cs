using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;
using UnityEngine.Events;

public class Loading : MonoBehaviour
{
    /// <summary>
    /// 기존 싱글톤 클래스를 상속하여 만들기엔, 가독성이 떨어질 염려로 임의로 싱글톤 코드를 구성하였음
    /// </summary>
    [Header("SingleTone")]
    private static Loading instance;
    public static Loading Instance
    {
        get 
        {
            if(instance == null)
            {
                var findObj = FindObjectOfType<Loading>();
                if(findObj == null)
                {
                    instance = Create();
                }
                else
                {
                    instance = findObj;
                }
            }
            return instance; 
        }
    }
    private static Loading Create()
    {
        return Instantiate(Resources.Load<Loading>("Prefab/UI/LoadingUI"));
    }

    [Header("Event")]
    private UnityAction loadEndFunc = null;

    [Header("UI")]
    public CanvasGroup LoadingCanvasGroup;
    public Scrollbar LoadingGauge;
    public Text LoadingText;

    [Header("Variable")]
    private string loadSceneName = "";

    private void Awake()
    {
        if(Instance != this)
        {
            Destroy(this.gameObject);
            return;
        }
        DontDestroyOnLoad(this.gameObject);
    }

    public void LoadScene(string sceneName, UnityAction loadEndFunc = null)
    {
        ObjectPoolManager.Instance.Initialize();
        UnitSystem.PickupSystem.PickupableManager.ClearUnit();
        UnitSystem.Unit.Units.Clear();

        this.gameObject.SetActive(true);
        SceneManager.sceneLoaded += onSceneLoadEvent;
        
        loadSceneName = sceneName;
        this.loadEndFunc = loadEndFunc;

        StartCoroutine("LoadSceneProcess");
    }
    IEnumerator LoadSceneProcess()
    {
        const float progressLimit = 0.8f;
        const float progressSpeed = (1 - progressLimit) * 10f;

        LoadingGauge.size = 0f;
        
        // 1. 페이드 인
        yield return StartCoroutine(Fade(true));

        // 2, 씬 로딩
        AsyncOperation op = SceneManager.LoadSceneAsync(loadSceneName);
        // 2-1. 내가 원하는 시점에 활성화 시켜야하기에, 씬 준비 되었을때 작동 기능을 잠시 비활성화
        op.allowSceneActivation = false;

        // 3. 로딩 진행중
        float timeAcc = 0f;
        while(!op.isDone)
        {
            yield return null;

            // 3-1. 90% 까지는 실제 씬의 로딩
            if (op.progress < progressLimit)
                LoadingGauge.size = op.progress;
            // 3-2. 나머지 10% 는 개발자가 임의로 조정할 퍼센테이지
            else
            {
                timeAcc += Time.unscaledDeltaTime / progressSpeed;
                LoadingGauge.size = Mathf.Lerp(progressLimit, 1f, timeAcc);
                if (LoadingGauge.size >= 1f)
                {
                    // 2-1. 기능 다시 재활성화
                    op.allowSceneActivation = true;
                    yield break;
                }
            }
        }

    }


    /// <summary>
    /// 씬 로딩 완료 이벤트 함수
    /// </summary>
    /// <param name="arg0"></param>
    /// <param name="arg1"></param>
    private void onSceneLoadEvent(Scene arg0, LoadSceneMode arg1)
    {
        if(arg0.name == loadSceneName)
        {
            StartCoroutine(Fade(false));
            this.loadEndFunc?.Invoke();
            SceneManager.sceneLoaded -= onSceneLoadEvent;
        }
    }



    /// <summary>
    /// 페이드 인아웃 코루틴
    /// </summary>
    /// <param name="isFadeIn"></param>
    /// <returns></returns>
    private IEnumerator Fade(bool isFadeIn)
    {
        const float maxTime = 0.7f;
        float timeAcc = 0f;

        float start = isFadeIn ? 0f : 1f;
        float end = isFadeIn ? 1f : 0f;

        while (timeAcc < maxTime)
        {
            timeAcc += Time.unscaledDeltaTime;
            LoadingCanvasGroup.alpha = Mathf.Lerp(start, end, (timeAcc / maxTime));
            yield return null;
        }

        if (isFadeIn == false)
            this.gameObject.SetActive(false);
    }
}

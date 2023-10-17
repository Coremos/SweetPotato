using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 이전 프로젝트의 viewName 은 없어지고, 프리팹 이름에 맞춰 아래 enum 변수에 추가하시면 됩니다.
/// </summary>
public enum View : int
{
    invalid = -1,
    MAIN = 0,
    StartView = 1,
    Count
};

public class UIViewManager : MonoBehaviour
{
    private class ViewData
    {
        private View view;
        private string data;
        private bool isDirectShow;

        public View VIEW
        {
            get { return view; }
            private set { view = value; }
        }
        public string DATA
        {
            get { return data; }
            private set { data = value; }
        }
        public bool DIRECT_SHOW
        {
            get { return isDirectShow; }
            set { isDirectShow = value; }
        }

        public ViewData(View view, string data)
        {
            this.VIEW = view;
            this.DATA = data;
            isDirectShow = false;
        }
    }

    /// <summary>
    /// 해당 클래스는 정적 영역으로부터 타 영역과 공유됩니다.
    /// 단, 싱글톤 클래스는 아니니, 반드시 생성(및 활성화) => Awake 이후 사용해주세요.
    /// </summary>
    public static UIViewManager Instance = null;

    [Header("Prefabs")]
    // 기존 씬에 프리팹 오브젝트를 추가필요.
    // 순서는 View 열거체 상수의 순서에 따를 것
    public List<UIView> ViewList = new List<UIView>();

    [Header("Variable")]
    // 뷰 출력 순서를 통제합니다. => 그래서 스택으로 사용됨
    // 아래 viewDataList 는 사용자(유저)가 원하는 순서대로 삽입할 수 있습니다.
    private Stack<ViewData> viewDataList = new Stack<ViewData>();
    private bool isViewChanging = false;




    private void Awake()
    {
        Instance = this;
        ObjectPoolManager.Instance.Initialize();
        UpLoadScript();
    }


    private void UpLoadScript()
    {
        CropScript.Load();
        MonsterScript.Load();
    }




    public void PushView(View view, string data = null)
    {
        if (Enum.IsDefined(typeof(View), view) == false || view == View.invalid || view == View.Count)
            return;

        // 뷰가 변환되는 중이라면 다른 뷰를 열수 없음
        if (isViewChanging)
            return;

        // 같은 뷰를 두번 열수는 없음
        if (viewDataList.Count > 0 && viewDataList.Peek().VIEW == view)
            return;

        StartCoroutine(CoPushView(view, data));
    }
    private IEnumerator CoPushView(View view, string data)
    {
        // 새 뷰가 변경 중(혹은 탑재 중)
        isViewChanging = true;
        float delayTime = 0f;

        // 맨 위의 있는 뷰 숨김
        UIView ViewObj = null;
        if (viewDataList.Count > 0)
        {
            ViewObj = ViewList[(int)viewDataList.Peek().VIEW];
            if (ViewObj != null)
            {
                delayTime = ViewObj.Hide();
                yield return new WaitForSecondsRT(delayTime);
            }
        }

        // 새 뷰를 맨 위로 탑재
        ViewData viewData = new ViewData(view, data);
        viewDataList.Push(viewData);

        // 맨 위에 있는 새 뷰를 출력
        ViewObj = ViewList[(int)viewDataList.Peek().VIEW];
        ViewObj.Show(viewData.DIRECT_SHOW);
        ViewObj.InitAction?.Invoke(viewData.DATA);
        yield return new WaitForSecondsRT(delayTime);

        // 새 뷰가 변경 완료(혹은 탑재 완료)
        isViewChanging = false;
    }
    public void GoView(View view, string data = null)
    {
        if (ViewList == null || ViewList.Count <= 0)
            return;

        StartCoroutine(CoGoView(view, data));
    }
    IEnumerator CoGoView(View view, string data)
    {
        isViewChanging = true;
        float delayTime = 0.0f;

        // 가장 최신까지 올라와있던 뷰 닫기
        UIView ViewObj = null;
        if (viewDataList.Count > 0)
        {
            ViewObj = ViewList[(int)viewDataList.Peek().VIEW];
            if (ViewObj != null)
            {
                delayTime = ViewObj.Hide();
                yield return new WaitForSecondsRT(delayTime);
            }
        }
        // 뷰 데이터 스택 청소
        viewDataList.Clear();
        // 새 뷰 데이터 삽입
        ViewData viewData = new ViewData(view, data);
        viewDataList.Push(viewData);

        ViewObj = ViewList[(int)viewData.VIEW];
        delayTime = ViewObj.Show(viewData.DIRECT_SHOW);
        ViewObj.InitAction?.Invoke(viewData.DATA);
        yield return new WaitForSecondsRT(delayTime);

        isViewChanging = false;
    }
    public void PopView()
    {
        if (isViewChanging)
            return;

        if (viewDataList.Count <= 0)
            return;

        StartCoroutine(CoPopView());
    }
    private IEnumerator CoPopView()
    {
        isViewChanging = true;
        float delayTime = 0.0f;

        // 맨위의 뷰 숨김 처리
        UIView ViewObj = null;
        if (viewDataList.Count > 0)
        {
            ViewObj = ViewList[(int)viewDataList.Peek().VIEW];
            if (ViewObj != null)
            {
                delayTime = ViewObj.Hide();
                yield return new WaitForSecondsRT(delayTime);
            }
        }

        // 현재 맨위의 뷰데이터 삭제
        viewDataList.Pop();

        // 그 다음 남아있는 뷰 중의 최상위 뷰를 출력
        ViewData viewData = null;
        if (viewDataList.Count > 0)
            viewData = viewDataList.Peek();

        ViewObj = null;
        if (viewData == null)
        {
            ViewObj = ViewList[(int)View.MAIN];
            ViewObj.Show();

            yield return new WaitForSecondsRT(delayTime);
            isViewChanging = false;
            yield break;
        }
        else
        {
            ViewObj = ViewList[(int)viewData.VIEW];
            ViewObj.Show(viewData.DIRECT_SHOW);
            ViewObj.InitAction?.Invoke(viewData.DATA);

            yield return new WaitForSecondsRT(delayTime);
            isViewChanging = false;
            yield break;
        }
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// ���� ������Ʈ�� viewName �� ��������, ������ �̸��� ���� �Ʒ� enum ������ �߰��Ͻø� �˴ϴ�.
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
    /// �ش� Ŭ������ ���� �������κ��� Ÿ ������ �����˴ϴ�.
    /// ��, �̱��� Ŭ������ �ƴϴ�, �ݵ�� ����(�� Ȱ��ȭ) => Awake ���� ������ּ���.
    /// </summary>
    public static UIViewManager Instance = null;

    [Header("Prefabs")]
    // ���� ���� ������ ������Ʈ�� �߰��ʿ�.
    // ������ View ����ü ����� ������ ���� ��
    public List<UIView> ViewList = new List<UIView>();

    [Header("Variable")]
    // �� ��� ������ �����մϴ�. => �׷��� �������� ����
    // �Ʒ� viewDataList �� �����(����)�� ���ϴ� ������� ������ �� �ֽ��ϴ�.
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

        // �䰡 ��ȯ�Ǵ� ���̶�� �ٸ� �並 ���� ����
        if (isViewChanging)
            return;

        // ���� �並 �ι� ������ ����
        if (viewDataList.Count > 0 && viewDataList.Peek().VIEW == view)
            return;

        StartCoroutine(CoPushView(view, data));
    }
    private IEnumerator CoPushView(View view, string data)
    {
        // �� �䰡 ���� ��(Ȥ�� ž�� ��)
        isViewChanging = true;
        float delayTime = 0f;

        // �� ���� �ִ� �� ����
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

        // �� �並 �� ���� ž��
        ViewData viewData = new ViewData(view, data);
        viewDataList.Push(viewData);

        // �� ���� �ִ� �� �並 ���
        ViewObj = ViewList[(int)viewDataList.Peek().VIEW];
        ViewObj.Show(viewData.DIRECT_SHOW);
        ViewObj.InitAction?.Invoke(viewData.DATA);
        yield return new WaitForSecondsRT(delayTime);

        // �� �䰡 ���� �Ϸ�(Ȥ�� ž�� �Ϸ�)
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

        // ���� �ֽű��� �ö���ִ� �� �ݱ�
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
        // �� ������ ���� û��
        viewDataList.Clear();
        // �� �� ������ ����
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

        // ������ �� ���� ó��
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

        // ���� ������ �䵥���� ����
        viewDataList.Pop();

        // �� ���� �����ִ� �� ���� �ֻ��� �並 ���
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

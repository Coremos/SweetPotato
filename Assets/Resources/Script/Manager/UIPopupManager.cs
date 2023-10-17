using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Popup : int
{
    invalid = -1,
    StageDay = 0,
    PausePopup = 1,
    PopupGameOver = 2,
    Count
};

/// <summary>
/// ������� :
///   ����
///     1. UIPopupManager.Show
///     2. Stack push
///     3. Instantiate
///   �Ҹ�
///     1. UIPopup Close
///     2. UIPopup Disable
///     3. Statck Pop
/// </summary>
public class UIPopupManager : MonoBehaviour
{
    public class popupData
    {
        private Popup popup;
        private string data;

        public Popup POPUP
        {
            get { return popup; }
            private set { popup = value; }
        }
        public string DATA
        {
            get { return data; }
            private set { data = value; }
        }

        public popupData(Popup kind, string data)
        {
            this.POPUP = kind;
            this.DATA = data;
        }
    }

    public static UIPopupManager Instance;
    // ��Ʈ�� ������ �˾� �̸� �߰���
    // ������(�ν��Ͻ�) �߰� ���, �˾����� �˾��� ��µǾ���� ��� ��� ������ ���������� ����.
    // �˾��� ��Ÿ�ӿ��� �����Ͽ� ���� �ϴ� ������ ����
    [Header("Prefabs")]
    private string[] PopupPrefabs = new string[(int)Popup.Count]
    {
        "StageDay",
        "PausePopup",
        "PopupGameOver"
    };
    [Header("Variable")]
    private Stack<popupData> popupDataList = new Stack<popupData>();



    private void Awake()
    {
        Instance = this;
    }



    public static Transform GetTransform()
    {
        if (Instance == null)
            return null;

        return Instance.transform;
    }
    public static UIPopup Show(Popup kind, string data)
    {
        if (Instance == null)
            return null;

        // PopupPrefabs ���� == Popup.Count ��ġ
        // �ٸ� �� �����̴�, �ش� enum �߰��ÿ� �ݵ�� �ν��������� �����Ұ�
        if (Enum.IsDefined(typeof(Popup), kind) == false || kind == Popup.Count)
            return null;

        // ���ÿ� �˾������� ����
        popupData popupData = new popupData(kind, data);
        Instance.popupDataList.Push(popupData);

        // ���� ���� �ִ� �˾� ���
        GameObject popup = Instance.MakePopup(Instance.popupDataList.Peek().POPUP);
        if (popup == null)
            return null;

        if (Instance.gameObject != null)
        {
            popup.SetParent(Instance.gameObject);
            popup.ResetTransform();
        }

        UIPopup popupComp = popup.GetComponent<UIPopup>();
        if (popupComp == null)
            return null;

        popupComp.InitPopupData(kind, data);
        popup.SetActive(true);

        return popupComp;
    }

    private GameObject MakePopup(Popup kind)
    {
        return Instantiate<GameObject>(Resources.Load<GameObject>(string.Format("Prefab/Popup/{0}", PopupPrefabs[(int)kind])), this.transform, false);
    }
    public void Close()
    {
        if (popupDataList.Count > 0)
            popupDataList.Pop();
    }

    public void AllClose()
    {
        if (popupDataList.Count > 0)
        {
            for (int i = 0; i < this.transform.childCount; i++)
            {
                var child = this.transform.GetChild(i);
                if (child.GetComponent<UIPopup>() != null)
                    child.GetComponent<UIPopup>().Close();
            }

        }
    }

    public int GetPopupCount()
    {
        return popupDataList.Count;
    }
}

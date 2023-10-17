using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class UIPopup : MonoBehaviour
{
    [Header("Event")]
    public UnityAction CloseAction = null;

    [Header("Variable")]
    protected UIPopupManager.popupData popupData;


    protected virtual void OnEnable()
    {

    }
    protected virtual void OnDisable()
    {
        UIPopupManager.Instance.Close();
    }


    public void InitPopupData(Popup kind, string data)
    {
        popupData = new UIPopupManager.popupData(kind, data);
    }

    public void Close()
    {
        CloseAction?.Invoke();
        gameObject.SetActive(false);
        Destroy(this.gameObject);
    }
}

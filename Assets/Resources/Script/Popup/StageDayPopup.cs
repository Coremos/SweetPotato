using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using TMPro;

public class StageDayPopup : UIPopup
{
    public TMP_Text desc;

    [Header("Event")]
    private UnityAction okAction;


    public static StageDayPopup Show(Transform parent, string desc, UnityAction okaction = null)
    {
        StageDayPopup popupUI = (StageDayPopup)UIPopupManager.Show(Popup.StageDay, null);
        if (popupUI == null)
            return popupUI;

        popupUI.SetData(parent, desc);
        popupUI.SetEvent(okaction);
        return popupUI;
    }
    private void SetData(Transform parent, string desc)
    {
        if (parent == null)
            parent = UIPopupManager.GetTransform();

        this.desc.text = desc;
    }
    private void SetEvent(UnityAction okaction)
    {
        this.okAction = okaction;
    }

    public void ClosePopup()
    {
        this.okAction?.Invoke();
        Close();
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PausePopup : UIPopup
{
    [Header("Event")]
    private UnityAction okAction;


    public static PausePopup Show(Transform parent, UnityAction okaction = null)
    {
        PausePopup popupUI = (PausePopup)UIPopupManager.Show(Popup.PausePopup, null);
        if (popupUI == null)
            return popupUI;

        popupUI.SetData(parent);
        popupUI.SetEvent(okaction);
        return popupUI;
    }
    private void SetData(Transform parent)
    {
        if (parent == null)
            parent = UIPopupManager.GetTransform();
    }
    private void SetEvent(UnityAction okaction)
    {
        this.okAction = okaction;
    }

    
    public void OnClickToMainMenu()
    {
        Loading.Instance.LoadScene("Main");
    }
    public void ClosePopup()
    {
        this.okAction?.Invoke();
        Close();
    }
}

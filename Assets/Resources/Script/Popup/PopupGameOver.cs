using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PopupGameOver : UIPopup
{
    [Header("Event")]
    private UnityAction okAction;

    public static PopupGameOver SHOW(Transform parent, UnityAction okaction = null)
    {
        PopupGameOver popupUI = (PopupGameOver)UIPopupManager.Show(Popup.PopupGameOver, null);
        if (popupUI == null)
            return popupUI;

        popupUI.SetData(parent);
        popupUI.SetEvent(okaction);
        GameManager.Instance.PauseGame(true);
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


    public void OnClickRetryBtn()
    {
        Loading.Instance.LoadScene("Main");
    }
    public void OnClickMainMenuBtn()
    {
        Loading.Instance.LoadScene("Main");
    }
}

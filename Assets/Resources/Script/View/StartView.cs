using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartView : UIView
{
    private void Start()
    {
        GameManager.Instance.PauseGame(true);
    }


    public void OnClickGamePlayBtn()
    {
        UIViewManager.Instance.GoView(View.MAIN);
        GameManager.Instance.PauseGame(false);
    }
    public void OnClickExitBtn()
    {
        Application.Quit();
    }
  
}

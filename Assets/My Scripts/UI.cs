using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UI : MonoBehaviour
{
    // ESC메뉴 UI를 보관할 변수.
    public GameObject UI_Esc;

    private void Start()
    {
        
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            
            OpenMenu();
        }
    }

    public void OnClickExit()
    {
        Application.Quit();
    }

    public void OpenMenu()
    {
        Time.timeScale = 0f;
    }

    public void CloseMenu()
    {
        Time.timeScale = 1f;
    }

    public void OnClickStartCampaignMode()
    {
        SceneManager.LoadScene("Stage_01");
    }

    public void OnClickStartInfiniteMode()
    {
        SceneManager.LoadScene("InfiniteMode");
    }

    public void OnClickRetryInfiniteMode()
    {
        SceneManager.LoadScene("InfiniteMode");
    }

    public void OnClickRetryCampaignMode()
    {
        SceneManager.LoadScene("Stage_01");
    }
}

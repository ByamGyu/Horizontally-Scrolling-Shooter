using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UI_Button_All : MonoBehaviour
{
    //public Text _scoretext;


    private void Start()
    {
        
    }

    private void Update()
    {
        
    }

    public void OnClickExit()
    {
        SoundManager.instance.PlaySoundEffectOneShot("UI_Select");
        Application.Quit();
    }

    public void Resume()
    {
        SoundManager.instance.PlaySoundEffectOneShot("UI_Select");

        UIManager.instance.Init();
        UIManager.instance.SetActiveSceneUIGroup(true);

        Time.timeScale = 1f;
    }

    public void OpenEscMenu()
    {
        GameManager.instance._gamemode = Define.GameMode.UI;

        UIManager.instance.Init();
        UIManager.instance.SetActiveEscMenuGroup(true);

        Time.timeScale = 0f;
    }

    public void CloseEscMenu()
    {
        UIManager.instance.Init();
        UIManager.instance.SetActiveSceneUIGroup(true);

        SoundManager.instance.PlaySoundEffectOneShot("UI_Select");

        Time.timeScale = 1f;
    }

    public void OnClickStartCampaignMode()
    {
        if (GameManager.instance == null)
        {
            // 현재 구조에서 메인 메뉴에는 게임 매니저가 없음
        }
        else
        {
            GameManager.instance._gamemode = Define.GameMode.Infinite;
            GameManager.instance.Init();
        }

        SoundManager.instance.PlaySoundEffectOneShot("UI_Select");

        GameInstance.instance.Read1stScoreFile();

        ObjectManager.instance.AllObjectSetActiveFalse();

        UIManager.instance.Init();

        SceneManager.LoadScene("Stage_01");

        UIManager.instance.SetActiveSceneUIGroup(true);

        Time.timeScale = 1f;
    }    

    public void OnClickStartInfiniteMode()
    {
        if(GameManager.instance == null)
        {
            // 현재 구조에서 메인 메뉴에는 게임 매니저가 없음
        }
        else
        {
            GameManager.instance._gamemode = Define.GameMode.Infinite;
            GameManager.instance.Init();
        }        

        SoundManager.instance.PlaySoundEffectOneShot("UI_Select");

        GameInstance.instance.Read1stScoreFile();

        ObjectManager.instance.AllObjectSetActiveFalse();

        UIManager.instance.Init();

        SceneManager.LoadScene("InfiniteMode");

        UIManager.instance.SetActiveSceneUIGroup(true);

        Time.timeScale = 1f;
    }

    public void OnClickRetry()
    {
        GameManager.instance.Init();

        GameInstance.instance.Read1stScoreFile();

        SoundManager.instance.PlaySoundEffectOneShot("UI_Select");

        ObjectManager.instance.AllObjectSetActiveFalse();

        UIManager.instance.Init();        

        Time.timeScale = 1f;

        if(SceneManager.GetActiveScene().name == "Stage_01")
        {
            SceneManager.LoadScene("Stage_01");
        }
        else if(SceneManager.GetActiveScene().name == "InfiniteMode")
        {
            SceneManager.LoadScene("InfiniteMode");
        }

        UIManager.instance.SetActiveSceneUIGroup(true);
    }

    public void OnClickOpenMainMenu()
    {
        GameManager.instance._gamemode = Define.GameMode.None;

        GameInstance.instance.Read1stScoreFile();

        SoundManager.instance.PlaySoundEffectOneShot("UI_Select");

        ObjectManager.instance.AllObjectSetActiveFalse();

        UIManager.instance.Init();        

        Time.timeScale = 1f;
        SceneManager.LoadScene("MainMenu");

        UIManager.instance.SetActiveMainMenuGroup(true);
    }

    public void OnMouseEnter()
    {
        SoundManager.instance.PlaySoundEffectOneShot("UI_Change", 0.5f);
    }
}

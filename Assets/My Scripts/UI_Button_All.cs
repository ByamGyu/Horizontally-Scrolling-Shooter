using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UI_Button_All : MonoBehaviour
{
    // public GameObject Instance;

    public Text _scoretext;

    // public Define.GameMode _previousgamemode = Define.GameMode.None;


    private void Start()
    {
        
    }

    private void Update()
    {
        
    }

    public void SetScoreText()
    {
        if(_scoretext != null)
        {
            GameManager.instance._scoreText.text = string.Format("Score: " + "{0:n0}", GameManager.instance._PlayerScore);
        }
        
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
        //_previousgamemode = GameManager.instance._gamemode;
        GameManager.instance._gamemode = Define.GameMode.UI;

        UIManager.instance.Init();
        UIManager.instance.SetActiveEscMenuGroup(true);

        Time.timeScale = 0f;
    }

    public void CloseEscMenu()
    {
        //GameManager.instance._gamemode = _previousgamemode;

        UIManager.instance.Init();
        UIManager.instance.SetActiveSceneUIGroup(true);

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

        ObjectManager.instance.AllObjectSetActiveFalse();

        UIManager.instance.Init();
        
        Time.timeScale = 1f;

        SceneManager.LoadScene("Stage_01");

        UIManager.instance.SetActiveSceneUIGroup(true);
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

        ObjectManager.instance.AllObjectSetActiveFalse();

        UIManager.instance.Init();
        
        Time.timeScale = 1f;

        SceneManager.LoadScene("InfiniteMode");

        UIManager.instance.SetActiveSceneUIGroup(true);
    }

    public void OnClickRetry()
    {
        //_previousgamemode = GameManager.instance._gamemode;
        GameManager.instance.Init();

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

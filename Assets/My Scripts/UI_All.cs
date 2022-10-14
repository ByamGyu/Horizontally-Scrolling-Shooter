using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UI_All : MonoBehaviour
{
    // public GameObject UI_Esc;
    public SoundManager _soundmanager;
    public GameManager _gamemanager;

    private void Start()
    {
        GameObject go1 = GameObject.Find("SoundManager");
        if(go1 != null)
        {
            _soundmanager = go1.GetComponent<SoundManager>();
        }
        else Debug.Log("UI_All's SoundManager is null");

        GameObject go2 = GameObject.Find("GameManager");
        if(go2 != null)
        {
            _gamemanager = go2.GetComponent<GameManager>();
        }
        else Debug.Log("UI_All's GameManager is null");
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {            
            OpenEscMenu();
        }
    }

    public void OnClickExit()
    {
        _soundmanager.PlaySoundEffectOneShot("UI_Select");
        Application.Quit();
    }

    public void OpenEscMenu()
    {
        Time.timeScale = 0f;
    }

    public void CloseEscMenu()
    {
        Time.timeScale = 1f;
    }

    public void OnClickStartCampaignMode()
    {
        _soundmanager.PlaySoundEffectOneShot("UI_Select");
        SceneManager.LoadScene("Stage_01");
    }

    public void OnClickStartInfiniteMode()
    {
        _soundmanager.PlaySoundEffectOneShot("UI_Select");
        SceneManager.LoadScene("InfiniteMode");
    }

    public void OnClickRetryInfiniteMode()
    {
        _soundmanager.PlaySoundEffectOneShot("UI_Select");
        SceneManager.LoadScene("InfiniteMode");
    }

    public void OnClickRetryCampaignMode()
    {
        _soundmanager.PlaySoundEffectOneShot("UI_Select");
        SceneManager.LoadScene("Stage_01");
    }

    public void OnClickOpenMainMenu()
    {
        _soundmanager.PlaySoundEffectOneShot("UI_Select");
        SceneManager.LoadScene("MainMenu");
    }

    public void OnMouseEnter()
    {
        _soundmanager.PlaySoundEffectOneShot("UI_Change", 0.5f);
    }
}

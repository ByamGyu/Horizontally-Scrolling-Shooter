using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UI_All : MonoBehaviour
{
    public GameObject Instance;
    public SoundManager _soundmanager;
    public GameManager _gamemanager;
    public ObjectManager _objectmanager;
    public Text _scoretext;

    public Define.GameMode _previousgamemode = Define.GameMode.None;


    private void Start()
    {
        GameObject go1 = GameObject.Find("SoundManager");
        if(go1 != null)
        {
            _soundmanager = go1.GetComponent<SoundManager>();
        }
        else Debug.Log("UI_All's SoundManager is null");

        GameObject go2 = GameObject.Find("ObjectManager");
        if (go2 != null)
        {
            _objectmanager = go2.GetComponent<ObjectManager>();
        }
        else Debug.Log("UI_All's ObjectManager is null");

        FindGameManager();
    }

    private void Update()
    {
        //if(Input.GetKeyDown(KeyCode.Escape))
        //{            
        //    if(_gamemanager != null && (_gamemanager._gamemode == Define.GameMode.Campaign || _gamemanager._gamemode == Define.GameMode.Infinite))
        //    {
        //        if(Instance.activeSelf == false) OpenEscMenu();
        //    }
        //}
    }

    void FindGameManager()
    {
        GameObject go2 = GameObject.Find("GameManager");
        if (go2 != null)
        {
            _gamemanager = go2.GetComponent<GameManager>();
        }
        else Debug.Log("UI_All's GameManager is null");
    }

    public void SetScoreText()
    {
        if(_scoretext != null && _gamemanager != null)
        {
            _scoretext.text = string.Format("Score: " + "{0:n0}", _gamemanager._PlayerScore);
        }
    }

    public void OnClickExit()
    {
        if(_soundmanager != null) _soundmanager.PlaySoundEffectOneShot("UI_Select");
        Application.Quit();
    }

    public void Resume()
    {
        if (_soundmanager != null) _soundmanager.PlaySoundEffectOneShot("UI_Select");

        if(_gamemanager != null)
        {
            Time.timeScale = 1f;
        }

        if(Instance != null) Instance.SetActive(false);
    }

    public void OpenEscMenu()
    {
        if (_gamemanager != null)
        {
            _previousgamemode = _gamemanager._gamemode;
            _gamemanager._gamemode = Define.GameMode.UI;
        }

        if (Instance != null)
        {
            Instance.SetActive(true);
            Time.timeScale = 0f;
        }        
    }

    public void CloseEscMenu()
    {
        if (_gamemanager != null)
        {
            _gamemanager._gamemode = _previousgamemode;
        }

        if (Instance != null)
        {
            Instance.SetActive(false);
            Time.timeScale = 1f;
        }
    }

    public void OnClickStartCampaignMode()
    {
        if(_gamemanager != null)
        {
            _gamemanager._gamemode = Define.GameMode.Campaign;
            _gamemanager.Init();
        }

        if (_soundmanager != null) _soundmanager.PlaySoundEffectOneShot("UI_Select");

        if(_objectmanager != null) _objectmanager.AllObjectSetActiveFalse();

        Time.timeScale = 1f;
        SceneManager.LoadScene("Stage_01");
    }    

    public void OnClickStartInfiniteMode()
    {
        if (_gamemanager != null)
        {
            _gamemanager._gamemode = Define.GameMode.Infinite;
            _gamemanager.Init();
        }

        if (_soundmanager != null) _soundmanager.PlaySoundEffectOneShot("UI_Select");

        if (_objectmanager != null) _objectmanager.AllObjectSetActiveFalse();

        Time.timeScale = 1f;
        SceneManager.LoadScene("InfiniteMode");
    }

    public void OnClickRetry()
    {
        if (_gamemanager != null)
        {
            _previousgamemode = _gamemanager._gamemode;
            _gamemanager.Init();
        }

        if (_soundmanager != null) _soundmanager.PlaySoundEffectOneShot("UI_Select");

        if (_objectmanager != null) _objectmanager.AllObjectSetActiveFalse();

        Time.timeScale = 1f;

        if(_previousgamemode == Define.GameMode.Campaign)
        {
            SceneManager.LoadScene("Stage_01");
        }
        else if(_previousgamemode == Define.GameMode.Infinite)
        {
            SceneManager.LoadScene("InfiniteMode");
        }
        
    }

    public void OnClickOpenMainMenu()
    {
        if (_gamemanager != null)
        {
            _gamemanager._gamemode = Define.GameMode.None;
        }

        if (_soundmanager != null) _soundmanager.PlaySoundEffectOneShot("UI_Select");

        if (_objectmanager != null) _objectmanager.AllObjectSetActiveFalse();

        Time.timeScale = 1f;
        SceneManager.LoadScene("MainMenu");
    }

    public void OnMouseEnter()
    {
        if (_soundmanager != null) _soundmanager.PlaySoundEffectOneShot("UI_Change", 0.5f);
    }
}

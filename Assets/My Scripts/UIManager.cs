using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager instance;

    public Text _scoretext;
    public Image[] _lifeImage;
    public Image[] _UltImage;

    // Canvas들
    public GameObject _MainMenuGroup;
    public GameObject _SceneUIGroup;
    public GameObject _GameOverGroup;
    public GameObject _EscMenuGroup;
    public GameObject _StageClearGroup;


    void Awake()
    {
        if(instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this);
        }
        else
        {
            Debug.Log("UIManager가 2개 이상 존재함!");
            Destroy(gameObject);
        }
    }

    void Start()
    {
        // 메인메뉴에서 시작함
        SetActiveMainMenuGroup(true);
        SetActiveSceneUIGroup(false);
        SetActiveGameOverGroup(false);
        SetActiveEscMenuGroup(false);
        SetActiveStageClearGroup(false);
    }


    void Update()
    {
        
    }

    public void UpdateScoreText(int tmp)
    {

    }

    public void UpdateLifeIcon(int tmp)
    {

    }

    public void UpdateUltIcon(int tmp)
    {

    }

    public void SetActiveMainMenuGroup(bool active)
    {
        _MainMenuGroup.SetActive(active);
    }

    public void SetActiveSceneUIGroup(bool active)
    {
        _SceneUIGroup.SetActive(active);
    }

    public void SetActiveGameOverGroup(bool active)
    {
        _GameOverGroup.SetActive(active);
    }

    public void SetActiveEscMenuGroup(bool active)
    {
        _EscMenuGroup.SetActive(active);
    }

    public void SetActiveStageClearGroup(bool active)
    {
        _StageClearGroup.SetActive(active);
    }
}
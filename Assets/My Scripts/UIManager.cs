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

    // Canvas��
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
            Debug.Log("UIManager�� 2�� �̻� ������!");
            Destroy(gameObject);
        }
    }

    void Start()
    {
        // ���θ޴����� ������
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
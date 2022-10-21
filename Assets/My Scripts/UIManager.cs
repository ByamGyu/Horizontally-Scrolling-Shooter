using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    public static UIManager instance;

    public Text _scoretext;
    public Text _stageclearscoretext;
    public Image[] _lifeImage;
    public Image[] _UltImage;
    public Slider _ChargeAttackBar;

    // Canvas��
    public GameObject _MainMenuGroup;
    public GameObject _SceneUIGroup;
    public GameObject _GameOverGroup;
    public GameObject _EscMenuGroup;
    public GameObject _StageClearGroup;

    private Scene _CurScene;


    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this);
        }
        else
        {
            Debug.Log("UIManager�� 2�� �̻� ������!");
            Destroy(gameObject);
        }

        Init();
    }

    void Start()
    {
        OpenSceneStartUI();
    }


    void Update()
    {

    }

    public void Init()
    {
        SetActiveMainMenuGroup(false);
        SetActiveSceneUIGroup(false);
        SetActiveGameOverGroup(false);
        SetActiveEscMenuGroup(false);
        SetActiveStageClearGroup(false);
    }

    public void OpenSceneStartUI()
    {
        // ���� �� �̸� ��������
        _CurScene = SceneManager.GetActiveScene();

        Init();

        if (_CurScene.name == "MainMenu")
        {
            SetActiveMainMenuGroup(true);
        }
        else if (_CurScene.name == "Stage_01")
        {
            SetActiveSceneUIGroup(true);
        }
        else if (_CurScene.name == "InfiniteMode")
        {
            SetActiveSceneUIGroup(true);
        }
    }

    public void UpdateScoreText(int tmp)
    {
        _scoretext.text = string.Format("Score: " + "{0:n0}", tmp);

        UpdateStageClearScoreText(tmp);
    }

    public void UpdateStageClearScoreText(int tmp)
    {
        _stageclearscoretext.text = string.Format("Final Score: " + "{0:n0}", tmp);
    }

    public void UpdateLifeIcon(int life)
    {
        for (int i = 0; i < 3; i++)
        {
            _lifeImage[i].color = new Color(1, 1, 1, 0); // ���İ��� 0���� �ؼ� �Ⱥ��̰� �Ѵ�
        }

        // ������ ������ �̹��� ���� �ݿ�
        for (int i = 0; i < life; i++)
        {
            _lifeImage[i].color = new Color(1, 1, 1, 1); // ���İ��� 1�� �ؼ� ���̰� �Ѵ�
        }
    }

    public void UpdateUltIcon(int tmp)
    {
        for (int i = 0; i < 3; i++)
        {
            _UltImage[i].color = new Color(1, 1, 1, 0);
        }

        for (int i = 0; i < tmp; i++)
        {
            _UltImage[i].color = new Color(1, 1, 1, 1);
        }
    }

    public void UpdateChargeGuage(float curvalue, float maxvalue = 3.0f)
    {
        if (curvalue <= 0) _ChargeAttackBar.value = 0;

        _ChargeAttackBar.value = curvalue / maxvalue;
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

    public bool GetEscMenuActiveSelf()
    {
        return _EscMenuGroup.activeSelf;
    }
}
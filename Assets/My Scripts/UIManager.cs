using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    public static UIManager instance;

    public Text _scoretext;
    public Text _hightscoretext;
    public Text _stageclearscoretext;
    public Image[] _lifeImage;
    public Image[] _UltImage;
    public Slider _ChargeAttackBar;

    // Canvas들
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
            Debug.Log("UIManager가 2개 이상 존재함!");
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
        // 현재 씬 이름 가져오기
        _CurScene = SceneManager.GetActiveScene();

        Init();

        if (_CurScene.name == "MainMenu")
        {
            SetActiveMainMenuGroup(true);
        }
        else if (_CurScene.name == "Stage_01" || _CurScene.name == "InfiniteMode")
        {
            SetActiveSceneUIGroup(true);
        }
    }

    public void UpdateScoreText(int tmp)
    {
        _scoretext.text = string.Format("Score: " + "{0:n0}", tmp);

        if(tmp >= GameInstance.instance.Get1stScore())
        {
            _hightscoretext.text = string.Format("High Score: " + "{0:n0}", tmp);
        }

        UpdateStageClearScoreText(tmp);
    }

    public void InitScoreTextAndHighScoreTextAtGameStart() // 씬에 따라서 처음 시작할 때 점수 text 갱신 용도
    {
        _CurScene = SceneManager.GetActiveScene();

        if (_CurScene.name == "Stage_01")
        {
            _scoretext.text = string.Format("Score: " + "{0:n0}", 0);

            int hightscoretmp = GameInstance.instance.Get1stScore();
            _hightscoretext.text = string.Format("High Score: " + "{0:n0}", hightscoretmp);
        }
        else if(_CurScene.name == "InfiniteMode")
        {
            _scoretext.text = string.Format("Score: " + "{0:n0}", 0);

            int hightscoretmp = GameInstance.instance.Get1stScore();
            _hightscoretext.text = string.Format("High Score: " + "{0:n0}", hightscoretmp);
        }
        else if(_CurScene == null)
        {
            Debug.Log("점수와 상관없는 씬이거나 코드 업데이트 바람!");
            return;
        }
    }

    public void UpdateStageClearScoreText(int tmp)
    {
        _stageclearscoretext.text = string.Format("Final Score: " + "{0:n0}", tmp);
    }

    public void UpdateLifeIcon(int life)
    {
        for (int i = 0; i < 3; i++)
        {
            _lifeImage[i].color = new Color(1, 1, 1, 0); // 알파값을 0으로 해서 안보이게 한다
        }

        // 라이프 아이콘 이미지 상태 반영
        for (int i = 0; i < life; i++)
        {
            _lifeImage[i].color = new Color(1, 1, 1, 1); // 알파값을 1로 해서 보이게 한다
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
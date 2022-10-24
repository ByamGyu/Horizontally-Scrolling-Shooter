using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class GameInstance : MonoBehaviour
{
    public static GameInstance instance;

    [SerializeField]
    private int _playerscore_InfiniteMode; // 현재 무한모드 점수
    [SerializeField]
    private int _playerscore_CampaignMode; // 현재 캠페인모드 점수
    [SerializeField]
    private int _1stScore_InfiniteMode; // 무한모드 최고 점수
    [SerializeField]
    private int _1stScore_CampaignMode; // 캠페인모드 최고 점수


    void Awake()
    {
        Init();

        if(instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this);
        }
        else
        {
            Debug.Log("게임인스턴스가 2개 이상 존재함!");
            Destroy(gameObject);
        }
    }

    void Start()
    {
        
    }

    void Update()
    {
        
    }

    void Init()
    {
        _playerscore_InfiniteMode = 0;
        _playerscore_CampaignMode = 0;
        _1stScore_InfiniteMode = 0;
        _1stScore_CampaignMode = 0;

        Read1stScoreFile();
    }

    public void InitPlayerScore()
    {
        _playerscore_InfiniteMode = 0;
        _playerscore_CampaignMode = 0;
    }

    public void Read1stScoreFile() // text 파일에 저장된 기존 최고 점수 가져오기
    {
        TextAsset textFile = Resources.Load("CampaignModeHighScore") as TextAsset;
        if (textFile == null) // 파일이 없으면 생성
        {
            string fullpath = "Assets/Resources/CampaignModeHighScore";
            System.IO.File.WriteAllText(fullpath + ".txt", "");
        }
        else // 파일이 있으면 점수 읽어오기
        {
            StringReader stringReader = new StringReader(textFile.text);

            while (stringReader != null)
            {
                string line = stringReader.ReadLine();
                // 파일에 내용이 없거나 파일이 없으면 null
                if (line == null) break;

                int tmp = int.Parse(line); // 점수만 저장되기 때문에 바로 parse
                _1stScore_CampaignMode = tmp; // 파일의 점수를 변수에 저장
            }

            stringReader.Close(); // 파일을 다 읽었으면 꼭 닫아줘야한다.
        }        

        TextAsset textFile2 = Resources.Load("InfiniteModeHighScore") as TextAsset;
        if (textFile2 == null) // 파일이 없으면 생성
        {
            string fullpath = "Assets/Resources/InfiniteModeHighScore";
            System.IO.File.WriteAllText(fullpath + ".txt", "");
        }
        else
        {
            StringReader stringReader2 = new StringReader(textFile2.text);

            while (stringReader2 != null)
            {
                string line = stringReader2.ReadLine();
                // 파일에 내용이 없거나 파일이 없으면 null
                if (line == null) break;

                int tmp = int.Parse(line); // 점수만 저장되기 때문에 바로 parse
                _1stScore_InfiniteMode = tmp; // 파일의 점수를 변수에 저장
            }

            stringReader2.Close(); // 파일을 다 읽었으면 꼭 닫아줘야한다.
        }
    }

    void CalculHighScore()
    {
        if (GameManager.instance._gamemode == Define.GameMode.Campaign)
        {
            if(_playerscore_CampaignMode >= _1stScore_CampaignMode)
            {
                // 게임 인스턴스에 저장
                _1stScore_CampaignMode = _playerscore_CampaignMode;

                // txt파일에 저장
                string fullpath = "Assets/Resources/CampaignModeHighScore";
                System.IO.File.WriteAllText(fullpath + ".txt", _1stScore_CampaignMode.ToString());
            }
        }
        else if (GameManager.instance._gamemode == Define.GameMode.Infinite)
        {
            if(_playerscore_InfiniteMode >= _1stScore_InfiniteMode)
            {
                // 게임 인스턴스에 저장
                _1stScore_InfiniteMode = _playerscore_InfiniteMode;

                // txt파일에 저장
                string fullpath = "Assets/Resources/InfiniteModeHighScore";
                System.IO.File.WriteAllText(fullpath + ".txt", _1stScore_InfiniteMode.ToString());
            }
        }
    }

    public void SetPlayerScore(int tmp) // 점수 기록과 최고 점수 갱신을 동시에
    {
        if(GameManager.instance._gamemode == Define.GameMode.Campaign)
        {
            _playerscore_CampaignMode = tmp;
        }
        else if(GameManager.instance._gamemode == Define.GameMode.Infinite)
        {
            _playerscore_InfiniteMode = tmp;
        }

        CalculHighScore();
    }

    public int GetPlayerScore() // 모드에 맞게 점수를 반환
    {
        if (GameManager.instance._gamemode == Define.GameMode.Campaign)
        {
            return _playerscore_CampaignMode;
        }
        else if (GameManager.instance._gamemode == Define.GameMode.Infinite)
        {
            return _playerscore_InfiniteMode;
        }
        else
        {
            Debug.Log("점수 기록을 할 수 없는 모드임!");
            return 0;
        }
    }

    public int Get1stScore() // 모드에 맞게 점수를 반환
    {
        if (GameManager.instance._gamemode == Define.GameMode.Campaign)
        {
            return _1stScore_CampaignMode;
        }
        else if (GameManager.instance._gamemode == Define.GameMode.Infinite)
        {
            return _1stScore_InfiniteMode;
        }
        else
        {
            Debug.Log("점수와 관련 없는 모드임!");
            return 0;
        }
    }
}
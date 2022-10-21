using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
public class GameInstance : MonoBehaviour
{
    public static GameInstance instance;

    [SerializeField]
    private int _playerscore_InfiniteMode;
    [SerializeField]
    private int _playerscore_CampaignMode;
    [SerializeField]
    private int _1stScore_InfiniteMode;
    [SerializeField]
    private int _1stScore_CampaignMode;


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
            Debug.Log("�����ν��Ͻ��� 2�� �̻� ������!");
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

    void Read1stScoreFile() // ���� �ְ� ���� ��������
    {
        TextAsset textFile = Resources.Load("CampaignModeHighScore") as TextAsset;

        if (textFile == null) return;

        StringReader stringReader = new StringReader(textFile.text);

        while (stringReader != null)
        {
            string line = stringReader.ReadLine();
            // ���Ͽ� ������ ���ų� ������ ������ null
            if (line == null) break;

            int tmp = int.Parse(line);
            _1stScore_CampaignMode = tmp;
        }

        stringReader.Close();

        TextAsset textFile2 = Resources.Load("InfiniteModeHighScore") as TextAsset;

        StringReader stringReader2 = new StringReader(textFile.text);

        if (textFile2 == null) return;

        while (stringReader != null)
        {
            string line = stringReader2.ReadLine();
            // ���Ͽ� ������ ���ų� ������ ������ null
            if (line == null) break;

            int tmp = int.Parse(line);
            _1stScore_InfiniteMode = tmp;
        }

        stringReader2.Close(); // ������ �� �о����� �� �ݾ�����Ѵ�.
    }

    void CalculHighScore()
    {
        if (GameManager.instance._gamemode == Define.GameMode.Campaign)
        {
            if(_playerscore_CampaignMode >= _1stScore_CampaignMode)
            {
                _1stScore_CampaignMode = _playerscore_CampaignMode;
            }
        }
        else if (GameManager.instance._gamemode == Define.GameMode.Infinite)
        {
            if(_playerscore_InfiniteMode >= _1stScore_InfiniteMode)
            {
                _1stScore_InfiniteMode = _playerscore_InfiniteMode;
            }
        }
    }

    public void SetPlayerScore(int tmp)
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

    public int GetPlayerScore()
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
            return 0;
        }
    }

    public int Get1stScore()
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
            return 0;
        }
    }
}

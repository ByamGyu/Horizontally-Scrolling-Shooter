using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class GameInstance : MonoBehaviour
{
    public static GameInstance instance;

    [SerializeField]
    private int _playerscore_InfiniteMode; // ���� ���Ѹ�� ����
    [SerializeField]
    private int _playerscore_CampaignMode; // ���� ķ���θ�� ����
    [SerializeField]
    private int _1stScore_InfiniteMode; // ���Ѹ�� �ְ� ����
    [SerializeField]
    private int _1stScore_CampaignMode; // ķ���θ�� �ְ� ����


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

    public void InitPlayerScore()
    {
        _playerscore_InfiniteMode = 0;
        _playerscore_CampaignMode = 0;
    }

    public void Read1stScoreFile() // text ���Ͽ� ����� ���� �ְ� ���� ��������
    {
        TextAsset textFile = Resources.Load("CampaignModeHighScore") as TextAsset;
        if (textFile == null) // ������ ������ ����
        {
            string fullpath = "Assets/Resources/CampaignModeHighScore";
            System.IO.File.WriteAllText(fullpath + ".txt", "");
        }
        else // ������ ������ ���� �о����
        {
            StringReader stringReader = new StringReader(textFile.text);

            while (stringReader != null)
            {
                string line = stringReader.ReadLine();
                // ���Ͽ� ������ ���ų� ������ ������ null
                if (line == null) break;

                int tmp = int.Parse(line); // ������ ����Ǳ� ������ �ٷ� parse
                _1stScore_CampaignMode = tmp; // ������ ������ ������ ����
            }

            stringReader.Close(); // ������ �� �о����� �� �ݾ�����Ѵ�.
        }        

        TextAsset textFile2 = Resources.Load("InfiniteModeHighScore") as TextAsset;
        if (textFile2 == null) // ������ ������ ����
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
                // ���Ͽ� ������ ���ų� ������ ������ null
                if (line == null) break;

                int tmp = int.Parse(line); // ������ ����Ǳ� ������ �ٷ� parse
                _1stScore_InfiniteMode = tmp; // ������ ������ ������ ����
            }

            stringReader2.Close(); // ������ �� �о����� �� �ݾ�����Ѵ�.
        }
    }

    void CalculHighScore()
    {
        if (GameManager.instance._gamemode == Define.GameMode.Campaign)
        {
            if(_playerscore_CampaignMode >= _1stScore_CampaignMode)
            {
                // ���� �ν��Ͻ��� ����
                _1stScore_CampaignMode = _playerscore_CampaignMode;

                // txt���Ͽ� ����
                string fullpath = "Assets/Resources/CampaignModeHighScore";
                System.IO.File.WriteAllText(fullpath + ".txt", _1stScore_CampaignMode.ToString());
            }
        }
        else if (GameManager.instance._gamemode == Define.GameMode.Infinite)
        {
            if(_playerscore_InfiniteMode >= _1stScore_InfiniteMode)
            {
                // ���� �ν��Ͻ��� ����
                _1stScore_InfiniteMode = _playerscore_InfiniteMode;

                // txt���Ͽ� ����
                string fullpath = "Assets/Resources/InfiniteModeHighScore";
                System.IO.File.WriteAllText(fullpath + ".txt", _1stScore_InfiniteMode.ToString());
            }
        }
    }

    public void SetPlayerScore(int tmp) // ���� ��ϰ� �ְ� ���� ������ ���ÿ�
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

    public int GetPlayerScore() // ��忡 �°� ������ ��ȯ
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
            Debug.Log("���� ����� �� �� ���� �����!");
            return 0;
        }
    }

    public int Get1stScore() // ��忡 �°� ������ ��ȯ
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
            Debug.Log("������ ���� ���� �����!");
            return 0;
        }
    }
}
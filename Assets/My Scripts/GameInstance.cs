using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameInstance : MonoBehaviour
{
    public static GameInstance instance;

    [SerializeField]
    private int _playerscore;
    [SerializeField]
    private int _1stScore;


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
        _playerscore = 0;
        _1stScore = 0;
    }

    void CalculHighScore()
    {
        if(_playerscore >= _1stScore)
        {
            _1stScore = _playerscore;
        }
    }

    public void SetPlayerScore(int tmp)
    {
        _playerscore = tmp;

        CalculHighScore();
    }

    public int GetPlayerScore()
    {
        return _playerscore;
    }
}

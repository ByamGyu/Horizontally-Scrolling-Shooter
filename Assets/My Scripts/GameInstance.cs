using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameInstance : MonoBehaviour
{
    public static GameInstance instance;

    [SerializeField]
    private int _playerscore;


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
    }
}

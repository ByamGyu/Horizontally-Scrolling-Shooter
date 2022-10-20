using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Background_Scrolling_Deco : MonoBehaviour
{
    [SerializeField]
    private float _speed;
    //private int curIndex = 0; // 배열의 현재 인덱스
    [SerializeField]
    private int endIndex; // 배열의 마지막 인덱스

    [SerializeField]
    private float rightPosX; // 배경의 시작 x 좌표
    [SerializeField]
    private float leftPosX; // 배경의 끝 x 좌표

    [SerializeField]
    public Transform[] sprites;


    void Awake()
    {

    }

    void Start()
    {
        Init();
    }

    void Update()
    {
        Move();
        Scrolling();
    }

    void Init()
    {
        _speed = 0.25f;
        rightPosX = 30; // 시작하는 x좌표 위치
        leftPosX = -15; // 끝나는 x좌표 위치
        endIndex = sprites.Length - 1;
    }

    void Move()
    {
        for (int i = 0; i < sprites.Length; i++)
        {
            Vector3 curpos = sprites[i].transform.position;
            Vector3 nextpos = Vector3.left * _speed * Time.deltaTime;
            sprites[i].transform.position = curpos + nextpos;
        }
    }

    void Scrolling()
    {
        for (int i = 0; i < sprites.Length; i++)
        {
            if (sprites[i].transform.position.x <= leftPosX)
            {
                sprites[i].transform.position += new Vector3(rightPosX, 0, 0);
            }
        }
    }
}

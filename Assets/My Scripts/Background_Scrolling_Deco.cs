using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Background_Scrolling_Deco : MonoBehaviour
{
    public float _speed;
    public int curIndex = 0; // 배열의 현재 인덱스
    public int endIndex; // 배열의 마지막 인덱스

    public float rightPosX; // 배경의 시작 x 좌표
    public float leftPosX; // 배경의 끝 x 좌표

    public Transform[] sprites; // 스프라이트 여러개를 갖고있는 하나의 그룹이 원소로 들어간다.


    private void Awake()
    {

    }

    void Start()
    {
        endIndex = sprites.Length - 1;        
    }

    void Update()
    {
        Move();
        Scrolling();
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

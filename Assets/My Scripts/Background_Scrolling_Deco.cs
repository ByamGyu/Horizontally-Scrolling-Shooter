using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Background_Scrolling_Deco : MonoBehaviour
{
    public float _speed;
    public int curIndex = 0; // �迭�� ���� �ε���
    public int endIndex; // �迭�� ������ �ε���

    public float rightPosX; // ����� ���� x ��ǥ
    public float leftPosX; // ����� �� x ��ǥ

    public Transform[] sprites; // ��������Ʈ �������� �����ִ� �ϳ��� �׷��� ���ҷ� ����.


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

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Background_Scrolling_Deco : MonoBehaviour
{
    [SerializeField]
    private float _speed;
    //private int curIndex = 0; // �迭�� ���� �ε���
    [SerializeField]
    private int endIndex; // �迭�� ������ �ε���

    [SerializeField]
    private float rightPosX; // ����� ���� x ��ǥ
    [SerializeField]
    private float leftPosX; // ����� �� x ��ǥ

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
        rightPosX = 30; // �����ϴ� x��ǥ ��ġ
        leftPosX = -15; // ������ x��ǥ ��ġ
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

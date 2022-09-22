using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Satelite : Enemy_Base
{
    // 탄환 발사 딜레이 변수
    [SerializeField]
    float _Bullet_Shot_Delay_Cur = 0.0f;
    [SerializeField]
    float _Bullet_Shot_Delay_Max = 5.0f;

    // 오브젝트 저장 변수
    [SerializeField]
    public GameObject _Bullet1; // 필요한 오브젝트를 붙일 수 있다. (카메라, 물체 등등)

    void Awake()
    {
        _rigid = GetComponent<Rigidbody2D>();

        SimpleMove();
    }

    void Start()
    {
        _rigid = GetComponent<Rigidbody2D>();

        _Player = GameObject.FindGameObjectWithTag("Player");
    }

    protected override void Update()
    {
        Fire_Bullet();
        Bullet_Delay();
    }

    protected override void SimpleMove()
    {
        _rigid.velocity = Vector2.left * _speed;
    }

    void Fire_Bullet()
    {
        // 총알 생성 딜레이 시간 판별
        if (_Bullet_Shot_Delay_Cur < _Bullet_Shot_Delay_Max) return;

        // 플레이어와의 거리를 계산
        Vector3 _distance = (_Player.transform.position - transform.position).normalized;

        GameObject bullet1 = Instantiate(_Bullet1, transform.position + Vector3.left * 0.5f, transform.rotation);
        Rigidbody2D rigid1 = bullet1.GetComponent<Rigidbody2D>();
        rigid1.AddForce(_distance * 2, ForceMode2D.Impulse);

        _Bullet_Shot_Delay_Cur = 0.0f;
    }

    void Bullet_Delay()
    {
        if (_Bullet_Shot_Delay_Cur > 10) return;

        _Bullet_Shot_Delay_Cur += Time.deltaTime;
    }
}
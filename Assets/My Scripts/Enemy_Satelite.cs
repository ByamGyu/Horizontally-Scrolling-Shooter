using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Satelite : Enemy_Base
{
    // 탄환 발사 딜레이 변수
    [SerializeField]
    float _Bullet_Shot_Delay_Cur = 0.0f;
    [SerializeField]
    float _Bullet_Shot_Delay_Max = 5f;


    void Start()
    {
        _rigid = GetComponent<Rigidbody2D>();

        _Player = GameObject.Find("Player");
    }

    protected override void Update()
    {
        Fire_Bullet();
        Bullet_Delay();
        SimpleMoveLeft();
    }

    public override void SimpleMoveLeft()
    {
        _rigid.velocity = Vector2.left * _speed;
    }

    void Fire_Bullet()
    {
        if (_Player == null) return; // 플레이어 기체가 없으면 return

        // 총알 생성 딜레이 시간 판별
        if (_Bullet_Shot_Delay_Cur < _Bullet_Shot_Delay_Max) return;

        // 플레이어와의 거리를 계산
        Vector3 _distance = (_Player.transform.position - transform.position).normalized;
                
        
        GameObject bullet1 = ObjectManager.instance.MakeObj("Bullet_Enemy_Orange");
        bullet1.transform.position = transform.position + Vector3.left * 0.5f;
        bullet1.transform.rotation = transform.rotation;

        Rigidbody2D rigid1 = bullet1.GetComponent<Rigidbody2D>();
        rigid1.AddForce(_distance * 2, ForceMode2D.Impulse);

        _Bullet_Shot_Delay_Cur = 0.0f;
    }

    void Bullet_Delay()
    {
        if (_Bullet_Shot_Delay_Cur > 10) return;

        _Bullet_Shot_Delay_Cur += Time.deltaTime;
    }

    public override void Init()
    {
        _life = _MaxLife;
        _Bullet_Shot_Delay_Cur = 0;
    }
}
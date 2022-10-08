using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Cone : Enemy_Base
{
    // ÅºÈ¯ ¹ß»ç µô·¹ÀÌ º¯¼ö
    [SerializeField]
    float _Bullet_Shot_Delay_Cur = 0.0f;
    [SerializeField]
    float _Bullet_Shot_Delay_Max = 2.5f;
    

    void Start()
    {
        _rigid = GetComponent<Rigidbody2D>();
    }    

    protected override void Update()
    {
        Fire_Bullet();
        Bullet_Delay();
    }

    public override void SimpleMoveLeft()
    {
        _rigid.velocity = Vector2.left * _speed;
    }

    public override void SimpleMoveUp()
    {
        _rigid.velocity = Vector2.up * _speed * (0.5f);
    }

    public override void SimpleMoveDown()
    {
        _rigid.velocity = Vector2.up * _speed * (-0.5f);
    }

    void Fire_Bullet()
    {
        // ÃÑ¾Ë »ý¼º µô·¹ÀÌ ½Ã°£ ÆÇº°
        if (_Bullet_Shot_Delay_Cur < _Bullet_Shot_Delay_Max) return;

        GameObject bullet1 = objectmanager.MakeObj("Bullet_Enemy_Blue");
        bullet1.transform.position = transform.position + Vector3.left * 0.5f;
        bullet1.transform.rotation = transform.rotation;

        Rigidbody2D rigid1 = bullet1.GetComponent<Rigidbody2D>();
        rigid1.AddForce(Vector2.left * 5, ForceMode2D.Impulse);

        _Bullet_Shot_Delay_Cur = 0.0f;
    }

    void Bullet_Delay()
    {
        if (_Bullet_Shot_Delay_Cur > 10) return;

        _Bullet_Shot_Delay_Cur += Time.deltaTime;
    }
}
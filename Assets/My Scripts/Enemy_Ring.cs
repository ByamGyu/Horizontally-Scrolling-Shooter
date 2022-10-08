using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Ring : Enemy_Base
{
    // ÅºÈ¯ ¹ß»ç µô·¹ÀÌ º¯¼ö
    [SerializeField]
    float _Bullet_Shot_Delay_Cur = 0.0f;
    [SerializeField]
    float _Bullet_Shot_Delay_Max = 3.5f;


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

        GameObject bulletmid = objectmanager.MakeObj("Bullet_Enemy_Green");
        bulletmid.transform.position = transform.position + Vector3.left * 0.5f;
        bulletmid.transform.rotation = transform.rotation;
        GameObject bullettop = objectmanager.MakeObj("Bullet_Enemy_Green");
        bullettop.transform.position = transform.position + Vector3.up * 0.5f;
        bullettop.transform.rotation = transform.rotation;
        GameObject bulletbottom = objectmanager.MakeObj("Bullet_Enemy_Green");
        bulletbottom.transform.position = transform.position + Vector3.down * 0.5f;
        bulletbottom.transform.rotation = transform.rotation;
        Rigidbody2D rigid1 = bulletmid.GetComponent<Rigidbody2D>();
        Rigidbody2D rigid2 = bullettop.GetComponent<Rigidbody2D>();
        Rigidbody2D rigid3 = bulletbottom.GetComponent<Rigidbody2D>();
        rigid1.AddForce(Vector2.left * 5, ForceMode2D.Impulse);
        rigid2.AddForce(Vector2.up * 5 + (Vector2.left * _speed), ForceMode2D.Impulse);
        rigid3.AddForce(Vector2.down * 5 + (Vector2.left * _speed), ForceMode2D.Impulse);

        _Bullet_Shot_Delay_Cur = 0.0f;
    }

    void Bullet_Delay()
    {
        if (_Bullet_Shot_Delay_Cur > 10) return;

        _Bullet_Shot_Delay_Cur += Time.deltaTime;
    }
}
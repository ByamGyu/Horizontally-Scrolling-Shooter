using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Ring : Enemy_Base
{
    // źȯ �߻� ������ ����
    [SerializeField]
    float _Bullet_Shot_Delay_Cur = 0.0f;
    [SerializeField]
    float _Bullet_Shot_Delay_Max = 3.5f;

    // ������Ʈ ���� ����
    [SerializeField]
    public GameObject _Bullet1; // �ʿ��� ������Ʈ�� ���� �� �ִ�. (ī�޶�, ��ü ���)

    void Awake()
    {
        _rigid = GetComponent<Rigidbody2D>();

        SimpleMove();
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
        // �Ѿ� ���� ������ �ð� �Ǻ�
        if (_Bullet_Shot_Delay_Cur < _Bullet_Shot_Delay_Max) return;

        GameObject bulletmid = Instantiate(_Bullet1, transform.position + Vector3.left * 0.5f, transform.rotation);
        GameObject bullettop = Instantiate(_Bullet1, transform.position + Vector3.up * 0.5f, transform.rotation);
        GameObject bulletbottom = Instantiate(_Bullet1, transform.position + Vector3.down * 0.5f, transform.rotation);
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
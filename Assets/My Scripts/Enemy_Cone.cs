using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Cone : Enemy_Base
{
    // źȯ �߻� ������ ����
    [SerializeField]
    float _Bullet_Shot_Delay_Cur = 0.0f;
    [SerializeField]
    float _Bullet_Shot_Delay_Max = 2.5f;

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

        GameObject bullet1 = Instantiate(_Bullet1, transform.position + Vector3.left * 0.5f, transform.rotation);
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
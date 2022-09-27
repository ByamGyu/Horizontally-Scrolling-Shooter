using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Satelite : Enemy_Base
{
    // źȯ �߻� ������ ����
    [SerializeField]
    float _Bullet_Shot_Delay_Cur = 0.0f;
    [SerializeField]
    float _Bullet_Shot_Delay_Max = 5.0f;


    // ������Ʈ ���� ����
    [SerializeField]
    public GameObject _Bullet1; // �ʿ��� ������Ʈ�� ���� �� �ִ�. (ī�޶�, ��ü ���)


    void Start()
    {
        _rigid = GetComponent<Rigidbody2D>();
        SimpleMoveLeft();
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

    void Fire_Bullet()
    {
        if (_Player == null) return; // �÷��̾� ��ü�� ������ return

        // �Ѿ� ���� ������ �ð� �Ǻ�
        if (_Bullet_Shot_Delay_Cur < _Bullet_Shot_Delay_Max) return;

        // �÷��̾���� �Ÿ��� ���
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
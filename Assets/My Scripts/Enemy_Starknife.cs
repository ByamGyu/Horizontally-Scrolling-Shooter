using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Starknife : Enemy_Base
{
    void Start()
    {
        _rigid = GetComponent<Rigidbody2D>();

        _Player = GameObject.FindGameObjectWithTag("Player");
    }

    protected override void Update()
    {
        if (_Player == null) return;
        else
        {
            Stalk_Player(_Player);
        }
    }

    public override void SimpleMoveLeft()
    {
        _rigid.velocity = Vector2.left * _speed;
    }

    public void Stalk_Player(GameObject player)
    {
        if (player == null) return;
        else
        {
            // �÷��̾���� �Ÿ��� ���
            Vector3 _distance = (_Player.transform.position - transform.position).normalized;
            // �÷��̾ �ٶ󺸴� ����
            float _angle = Mathf.Atan2(-_distance.y, -_distance.x) * Mathf.Rad2Deg;

            // �÷��̾� ��ü�� x��ǥ�� �������� ������ �����Ѵ�.
            if(transform.position.x - 1.0f < _Player.transform.position.x)
            {
                return;
            }

            // �Ÿ� �� ���� ����
            _rigid.velocity = _distance * _speed;
            _rigid.rotation = _angle;
        }
    }
}
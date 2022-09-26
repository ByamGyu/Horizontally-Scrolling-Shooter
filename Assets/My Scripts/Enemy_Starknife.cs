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
            // 플레이어와의 거리를 계산
            Vector3 _distance = (_Player.transform.position - transform.position).normalized;
            // 플레이어를 바라보는 방향
            float _angle = Mathf.Atan2(-_distance.y, -_distance.x) * Mathf.Rad2Deg;

            // 플레이어 기체의 x좌표를 기준으로 추적을 포기한다.
            if(transform.position.x - 1.0f < _Player.transform.position.x)
            {
                return;
            }

            // 거리 및 방향 적용
            _rigid.velocity = _distance * _speed;
            _rigid.rotation = _angle;
        }
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet_Chase : Bullet_Base
{
    [SerializeField]
    GameObject _target = null;
    [SerializeField]
    float _speed_cur = 5.0f;
    [SerializeField]
    float _speed_max = 10.0f;

    Rigidbody2D _rigid;
    

    private void Start()
    {
        _rigid = GetComponent<Rigidbody2D>();
        // 처음 발사할 때 특정 방향으로 이동시킴 (PlayerController에서 하는 걸로)
    }

    private void FixedUpdate()
    {
        if (_target == null) // 타겟이 없으면 탐색
        {
            SearchTarget();
        }

        if (_target != null && _target.activeSelf == false) // 타겟이 있는데 비활성화 상태면
        {
            _target = null; // 타겟 변수 null로 변경

            SearchTarget(); // 탐색

            return;
        }

        // 타겟이 있고 활성화 상태면 추적
        if (_target != null && _target.activeSelf == true)
        {
            ChaseTarget();
        }
    }

    void SearchTarget()
    {
        if (_target == null)
        {
            // 만약 일반 적과 보스가 섞여있으면 보스 우선순위로
            _target = GameObject.FindGameObjectWithTag("Enemy");

            if(_target == null) // 일반 적이 없으면 보스를 탐색한다.
            {
                _target = GameObject.FindGameObjectWithTag("Enemy_Boss");

                if(_target == null)
                {
                    return;
                }
            }
        }
    }

    void ChaseTarget()
    {
        if (_target == null) return;
        else
        {
            // 가속
            if (_speed_cur <= _speed_max)
            {
                _speed_cur += _speed_cur * Time.deltaTime;
            }
            
            // 방향값
            Vector3 dir = (_target.transform.position - transform.position).normalized;

            // 목표물을 향해서 일정한 속도 * _speed_cur 를 설정
            _rigid.velocity = dir * _speed_cur;
            
            // 목표물 향해서 회전 완료(건들지마)
            float _angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
            _rigid.rotation = _angle;
        }
    }

    void SimpleMoveRight() { _rigid.velocity = Vector2.right * _speed_cur; }
}

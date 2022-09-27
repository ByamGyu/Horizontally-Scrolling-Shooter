using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet_Chase : Bullet_Base
{
    [SerializeField]
    GameObject _target;
    [SerializeField]
    bool _nomoresearch = false; // 타겟을 찾은 적이 있으면 더이상 탐색하지 못하게 하는 변수
    [SerializeField]
    float _speed_cur = 5.0f;
    [SerializeField]
    float _speed_max = 10.0f;

    Rigidbody2D _rigid;
    

    private void Start()
    {
        _rigid = GetComponent<Rigidbody2D>();

        // 처음 발사할 때 특정 방향으로 이동시킴 (PlayerController에서 하는 걸로)
        //_rigid.velocity = Vector2.right * _speed_cur;
    }

    private void FixedUpdate()
    {
        SearchTarget();

        if (_target != null) ChaseTarget();
    }

    void SearchTarget()
    {
        if (_target != null) _nomoresearch = true;

        if (_nomoresearch == false) _target = GameObject.FindGameObjectWithTag("Enemy");
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

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Enemy")
        {
            // 이 객체 파괴를 제외한 나머지 연산들
        }
    }
}

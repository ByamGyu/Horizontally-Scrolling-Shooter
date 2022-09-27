using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet_Guide : Bullet_Base
{
    [SerializeField]
    GameObject _target;
    [SerializeField]
    bool _nomoresearch = false; // 타겟을 찾은 적이 있으면 더이상 탐색하지 못하게 하는 변수
    [SerializeField]
    float _speed = 10.0f;
    [SerializeField]
    float _rotspeed = 10.0f;

    Rigidbody2D _rigid;
    

    private void Start()
    {
        _rigid = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        SearchTarget();

        if (_target == null)
        {
            SimpleMoveRight();
        }
        else ChaseTarget();        
    }

    void SearchTarget()
    {
        if (_target != null) _nomoresearch = true;

        if (_nomoresearch == false) _target = GameObject.FindGameObjectWithTag("Enemy");
    }

    void ChaseTarget()
    {
        //if (player == null) return;
        //else
        //{
        //    // 플레이어와의 거리를 계산
        //    Vector3 _distance = (_Player.transform.position - transform.position).normalized;
        //    // 플레이어를 바라보는 방향
        //    float _angle = Mathf.Atan2(-_distance.y, -_distance.x) * Mathf.Rad2Deg;

        //    // 플레이어 기체의 x좌표를 기준으로 추적을 포기한다.
        //    if (transform.position.x - 1.0f < _Player.transform.position.x) return;

        //    // 거리 및 방향 적용
        //    _rigid.velocity = _distance * _speed;
        //    _rigid.rotation = _angle;
        //}
    }

    void SimpleMoveRight() { _rigid.velocity = Vector2.right * _speed * 2.0f; }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Enemy")
        {
            // 이 객체 파괴를 제외한 나머지 연산들
        }
    }
}

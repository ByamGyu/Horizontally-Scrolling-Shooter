using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet_Guide : Bullet_Base
{
    [SerializeField]
    GameObject _target;
    [SerializeField]
    bool _nomoresearch = false; // Ÿ���� ã�� ���� ������ ���̻� Ž������ ���ϰ� �ϴ� ����
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
        //    // �÷��̾���� �Ÿ��� ���
        //    Vector3 _distance = (_Player.transform.position - transform.position).normalized;
        //    // �÷��̾ �ٶ󺸴� ����
        //    float _angle = Mathf.Atan2(-_distance.y, -_distance.x) * Mathf.Rad2Deg;

        //    // �÷��̾� ��ü�� x��ǥ�� �������� ������ �����Ѵ�.
        //    if (transform.position.x - 1.0f < _Player.transform.position.x) return;

        //    // �Ÿ� �� ���� ����
        //    _rigid.velocity = _distance * _speed;
        //    _rigid.rotation = _angle;
        //}
    }

    void SimpleMoveRight() { _rigid.velocity = Vector2.right * _speed * 2.0f; }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Enemy")
        {
            // �� ��ü �ı��� ������ ������ �����
        }
    }
}

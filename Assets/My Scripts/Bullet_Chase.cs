using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet_Chase : Bullet_Base
{
    [SerializeField]
    GameObject _target;
    [SerializeField]
    bool _nomoresearch = false; // Ÿ���� ã�� ���� ������ ���̻� Ž������ ���ϰ� �ϴ� ����
    [SerializeField]
    float _speed_cur = 5.0f;
    [SerializeField]
    float _speed_max = 10.0f;

    Rigidbody2D _rigid;
    

    private void Start()
    {
        _rigid = GetComponent<Rigidbody2D>();

        // ó�� �߻��� �� Ư�� �������� �̵���Ŵ (PlayerController���� �ϴ� �ɷ�)
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
            // ����
            if (_speed_cur <= _speed_max)
            {
                _speed_cur += _speed_cur * Time.deltaTime;
            }
            
            // ���Ⱚ
            Vector3 dir = (_target.transform.position - transform.position).normalized;

            // ��ǥ���� ���ؼ� ������ �ӵ� * _speed_cur �� ����
            _rigid.velocity = dir * _speed_cur;
            
            // ��ǥ�� ���ؼ� ȸ�� �Ϸ�(�ǵ�����)
            float _angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
            _rigid.rotation = _angle;
        }
    }

    void SimpleMoveRight() { _rigid.velocity = Vector2.right * _speed_cur; }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Enemy")
        {
            // �� ��ü �ı��� ������ ������ �����
        }
    }
}
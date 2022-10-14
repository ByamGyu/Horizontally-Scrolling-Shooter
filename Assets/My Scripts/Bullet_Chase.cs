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
        // ó�� �߻��� �� Ư�� �������� �̵���Ŵ (PlayerController���� �ϴ� �ɷ�)
    }

    private void FixedUpdate()
    {
        if (_target == null) // Ÿ���� ������ Ž��
        {
            SearchTarget();
        }

        if (_target != null && _target.activeSelf == false) // Ÿ���� �ִµ� ��Ȱ��ȭ ���¸�
        {
            _target = null; // Ÿ�� ���� null�� ����

            SearchTarget(); // Ž��

            return;
        }

        // Ÿ���� �ְ� Ȱ��ȭ ���¸� ����
        if (_target != null && _target.activeSelf == true)
        {
            ChaseTarget();
        }
    }

    void SearchTarget()
    {
        if (_target == null)
        {
            // ���� �Ϲ� ���� ������ ���������� ���� �켱������
            _target = GameObject.FindGameObjectWithTag("Enemy");

            if(_target == null) // �Ϲ� ���� ������ ������ Ž���Ѵ�.
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
}

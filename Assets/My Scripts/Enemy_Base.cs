using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Base : MonoBehaviour
{
    [SerializeReference]
    public float _speed;
    [SerializeReference]
    public float _life;
    [SerializeReference]
    Rigidbody2D _rigid;


    private void Awake()
    {
        _rigid = GetComponent<Rigidbody2D>();
        _rigid.velocity = Vector2.left * _speed;
    }

    void Update()
    {
        
    }

    void Move()
    {

    }

    void OnHit(int damage)
    {
        _life -= damage;

        if(_life <= 0)
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "BorderBullet")
        {
            Destroy(gameObject);
        }
        else if(collision.gameObject.tag == "PlayerBullet")
        {
            // ���� �÷��̾��� źȯ ������ �����´�
            Bullet_Base bullet = collision.gameObject.GetComponent<Bullet_Base>();
            OnHit(bullet._damage);

            // ���� �÷��̾��� źȯ ����
            Destroy(collision.gameObject);
        }
    }
}

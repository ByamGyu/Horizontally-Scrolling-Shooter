using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Base : MonoBehaviour
{
    [SerializeReference]
    protected float _speed;
    [SerializeReference]
    protected float _life;
    [SerializeReference]
    protected Rigidbody2D _rigid;
    [SerializeReference]
    protected GameObject _Player = null;


    private void Awake()
    {
        _rigid = GetComponent<Rigidbody2D>();
    }

    protected virtual void Update()
    {
        
    }

    protected virtual void SimpleMove() { } // ������ ��ũ��Ʈ���� �����ϵ��� ��

    void OnHit(int damage) // �ǰ� ����
    {
        _life -= damage;

        if(_life <= 0)
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision) // Ʈ����
    {
        if(collision.gameObject.tag == "BorderBullet") // ��輱���� �����
        {
            Destroy(gameObject);
        }
        else if(collision.gameObject.tag == "PlayerBullet") // �÷��̾� ��ü ���ݿ� �ǰ�
        {
            // ���� �÷��̾��� źȯ ����(���ݷ�)�� �����´�
            Bullet_Base bullet = collision.gameObject.GetComponent<Bullet_Base>();
            OnHit(bullet._damage);

            // ���� �÷��̾��� źȯ ����
            Destroy(collision.gameObject);
        }
    }
}
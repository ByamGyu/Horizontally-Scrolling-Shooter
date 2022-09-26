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
    public int _score = 100;
    [SerializeReference]
    public Rigidbody2D _rigid;
    [SerializeReference]
    public GameObject _Player = null;


    private void Awake()
    {
        _Player = GameObject.FindGameObjectWithTag("Player");

        _rigid = GetComponent<Rigidbody2D>();
    }

    protected virtual void Update()
    {
        
    }

    public virtual void SimpleMoveLeft() { } // ������ ��ũ��Ʈ���� �����ϵ��� ��
    public virtual void SimpleMoveUp() { } // ������ ��ũ��Ʈ���� �����ϵ��� ��
    public virtual void SimpleMoveDown() { } // ������ ��ũ��Ʈ���� �����ϵ��� ��

    void OnHit(int damage) // �ǰ� ����
    {
        _life -= damage;

        if(_life <= 0)
        {
            _Player = GameObject.FindGameObjectWithTag("Player"); // ó�� �� ��ü�� �÷��̾ null�� ������ �̻��� ���� �ذ��
            PlayerController playerinfo = _Player.GetComponent<PlayerController>();
            playerinfo.AddScore(_score);

            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision) // Ʈ����
    {
        if (collision.gameObject.tag == "BorderBullet") // ��輱���� �����
        {
            Destroy(gameObject);
        }
        else if (collision.gameObject.tag == "PlayerBullet") // �÷��̾� ��ü ���ݿ� �ǰ�
        {
            // ���� �÷��̾��� źȯ ����(���ݷ�)�� �����´�
            Bullet_Base bullet = collision.gameObject.GetComponent<Bullet_Base>();
            OnHit(bullet._damage);

            // ���� �÷��̾��� źȯ ����
            Destroy(collision.gameObject);
        }
        else if (collision.gameObject.tag == "Player")
        {
            // !!!!!�÷��̾ ���� ������鼭 null��
            //PlayerController playerinfo = _Player.GetComponent<PlayerController>();
            //playerinfo.AddScore(_score);

            Destroy(gameObject);
        }
    }

    public int GetScore() { return _score; }
}
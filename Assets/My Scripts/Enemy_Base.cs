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

    protected virtual void SimpleMove() { } // 각자의 스크립트에서 구현하도록 함

    void OnHit(int damage) // 피격 판정
    {
        _life -= damage;

        if(_life <= 0)
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision) // 트리거
    {
        if(collision.gameObject.tag == "BorderBullet") // 경계선에서 사라짐
        {
            Destroy(gameObject);
        }
        else if(collision.gameObject.tag == "PlayerBullet") // 플레이어 기체 공격에 피격
        {
            // 닿은 플레이어의 탄환 정보(공격력)를 가져온다
            Bullet_Base bullet = collision.gameObject.GetComponent<Bullet_Base>();
            OnHit(bullet._damage);

            // 닿은 플레이어의 탄환 제거
            Destroy(collision.gameObject);
        }
    }
}
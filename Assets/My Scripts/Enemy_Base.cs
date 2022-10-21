using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Base : MonoBehaviour
{
    [SerializeField]
    protected float _speed;
    [SerializeField]
    protected float _life;
    [SerializeField]
    protected float _MaxLife;
    public int _score = 100;
    [SerializeField]
    protected Rigidbody2D _rigid;
    [SerializeField]
    protected GameObject _Player = null;


    void Awake()
    {
        _rigid = GetComponent<Rigidbody2D>();
    }

    void Start()
    {
        _Player = GameObject.Find("Player");
    }

    protected virtual void Update()
    {
        
    }

    public virtual void SimpleMoveLeft() { } // 각자의 스크립트에서 구현하도록 함
    public virtual void SimpleMoveUp() { } // 각자의 스크립트에서 구현하도록 함
    public virtual void SimpleMoveDown() { } // 각자의 스크립트에서 구현하도록 함


    void OnHit(int damage) // 피격 판정
    {
        _life -= damage;

        if(_life <= 0)
        {
            _Player = GameObject.FindGameObjectWithTag("Player"); // 처음 몇 개체만 플레이어가 null로 잡히는 이상한 버그 해결용
            PlayerController playerinfo = _Player.GetComponent<PlayerController>();
            playerinfo.AddScore(_score);

            int RandomInt = Random.Range(0, 2);
            if(RandomInt == 0) SoundManager.instance.PlaySoundEffectOneShot("Enemy_Destroy(Small)", 0.5f);
            else if(RandomInt == 1) SoundManager.instance.PlaySoundEffectOneShot("Enemy_Destroy(Loud)", 0.5f);

            int RandomInt2 = Random.Range(0, 4);
            if(RandomInt2 == 0) EffectManager.instance.SpawnEffect("Effect_Explosion_Redspark", transform.position, Vector3.one);
            else if(RandomInt2 == 1) EffectManager.instance.SpawnEffect("Effect_Explosion_Orangespark", transform.position, Vector3.one);
            else if(RandomInt2 == 2) EffectManager.instance.SpawnEffect("Effect_Explosion_Greenspark", transform.position, Vector3.one);
            else if(RandomInt2 == 3) EffectManager.instance.SpawnEffect("Effect_Explosion_Purplespark", transform.position, Vector3.one);


            Init();
            gameObject.SetActive(false);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "BorderBullet") // 경계선에서 사라짐
        {
            gameObject.SetActive(false);
        }
        else if (collision.gameObject.tag == "PlayerBullet") // 플레이어 기체 공격에 피격
        {
            // 닿은 플레이어의 탄환 정보(공격력)를 가져온다
            Bullet_Base bullet = collision.gameObject.GetComponent<Bullet_Base>();
            OnHit(bullet._damage);

            if(collision.gameObject.name == "Player Bullet Default(Clone)")
            {
                EffectManager.instance.SpawnEffect("Effect_PlayerBullet_Hit03_2", transform.position, Vector3.one);
            }
            else if(collision.gameObject.name == "Player Bullet Waveform(Clone)")
            {
                EffectManager.instance.SpawnEffect("Effect_PlayerBullet_Hit03_1", transform.position, Vector3.one);
            }

            // 닿은 플레이어의 탄환 제거
            collision.gameObject.SetActive(false);
        }
        else if(collision.gameObject.tag == "PlayerBullet_Charged")
        {
            // 닿은 플레이어의 탄환 정보(공격력)를 가져온다
            Bullet_Base bullet = collision.gameObject.GetComponent<Bullet_Base>();
            OnHit(bullet._damage);
        }
        else if (collision.gameObject.tag == "Player")
        {
            Init();
            gameObject.SetActive(false);
        }
    }

    public int GetScore() { return _score; }

    public virtual void Init()
    {
        
    }
}
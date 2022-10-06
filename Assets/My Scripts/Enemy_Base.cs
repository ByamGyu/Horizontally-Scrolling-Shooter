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

    // 효과음 파일
    [SerializeField]
    public AudioClip[] _Sounds;


    private void Awake()
    {
        _Player = GameObject.FindGameObjectWithTag("Player");

        _rigid = GetComponent<Rigidbody2D>();
    }

    protected virtual void Update()
    {
        
    }

    public virtual void SimpleMoveLeft() { } // 각자의 스크립트에서 구현하도록 함
    public virtual void SimpleMoveUp() { } // 각자의 스크립트에서 구현하도록 함
    public virtual void SimpleMoveDown() { } // 각자의 스크립트에서 구현하도록 함

    void PlaySound(string Name)
    {
        string tmp;

        for(int i = 0; i < _Sounds.Length; i++)
        {
            if (_Sounds[i].name == Name) tmp = _Sounds[i].name;
        }

        // SoundManager.instance.PlaySound(tmp);
    }

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

            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision) // 트리거
    {
        if (collision.gameObject.tag == "BorderBullet") // 경계선에서 사라짐
        {
            Destroy(gameObject);
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
            Destroy(collision.gameObject);
        }
        else if(collision.gameObject.tag == "PlayerBullet_Charged")
        {
            // 닿은 플레이어의 탄환 정보(공격력)를 가져온다
            Bullet_Base bullet = collision.gameObject.GetComponent<Bullet_Base>();
            OnHit(bullet._damage);
        }
        else if (collision.gameObject.tag == "Player")
        {
            // !!!!!플레이어가 먼저 사라지면서 null뜸
            //PlayerController playerinfo = _Player.GetComponent<PlayerController>();
            //playerinfo.AddScore(_score);

            Destroy(gameObject);
        }
    }

    public int GetScore() { return _score; }
}
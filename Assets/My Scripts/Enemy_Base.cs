using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Base : MonoBehaviour
{
    [SerializeField]
    public float _speed;
    [SerializeField]
    public float _life;
    [SerializeField]
    public float _MaxLife;
    [SerializeField]
    public int _score = 100;
    [SerializeField]
    public Rigidbody2D _rigid;
    [SerializeField]
    public GameObject _Player = null;

    [SerializeField]
    public GameManager _gamemanager = null;


    // ������Ʈ �Ŵ���(������Ʈ Ǯ��)
    // ������ ������ ��� ���� �� �� ����, �ڵ带 ���ؼ�(���� �Ŵ�������) �����
    public ObjectManager objectmanager;


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

    private void OnTriggerEnter2D(Collider2D collision) // Ʈ����
    {
        if (collision.gameObject.tag == "BorderBullet") // ��輱���� �����
        {
            gameObject.SetActive(false);
        }
        else if (collision.gameObject.tag == "PlayerBullet") // �÷��̾� ��ü ���ݿ� �ǰ�
        {
            // ���� �÷��̾��� źȯ ����(���ݷ�)�� �����´�
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

            // ���� �÷��̾��� źȯ ����
            collision.gameObject.SetActive(false);
        }
        else if(collision.gameObject.tag == "PlayerBullet_Charged")
        {
            // ���� �÷��̾��� źȯ ����(���ݷ�)�� �����´�
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
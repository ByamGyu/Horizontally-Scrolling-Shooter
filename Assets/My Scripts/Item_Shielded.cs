using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item_Shielded : MonoBehaviour
{
    // ������ �������� ����ִٰ� �ı��Ǹ� �ش� �������� �����ϰ� �������.

    public int _life = 3;
    public int _lifeMax = 3;
    Rigidbody2D _rigid;
    public float _speed = 1.0f;
    public int _score = 50;
        

    private void Start()
    {
        _rigid = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        SimpleMoveLeft();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "BorderBullet") // ��輱���� �����
        {
            gameObject.SetActive(false);
        }
        if (collision.gameObject.tag == "PlayerBullet") // źȯ�� ������
        {
            // �÷��̾��� źȯ ���ݷ��� ������ ����Ѵ�.
            Bullet_Base bulletinfo = collision.gameObject.GetComponent<Bullet_Base>();
            _life -= bulletinfo._damage;

            collision.gameObject.SetActive(false); ; // źȯ ����

            if(_life <= 0)
            {
                _life = 0;

                ItemspawnAndDestroy();
            }
        }
        if (collision.gameObject.tag == "PlayerBullet_Charged") // źȯ�� ������
        {
            // �÷��̾��� źȯ ���ݷ��� ������ ����Ѵ�.
            Bullet_Base bulletinfo = collision.gameObject.GetComponent<Bullet_Base>();
            _life -= bulletinfo._damage;

            if (_life <= 0)
            {
                _life = 0;

                ItemspawnAndDestroy();
            }
        }
        else if(collision.gameObject.tag == "Player") // �÷��̾� ��ü�� �浹�ϸ�
        {
            _life = 0;

            ItemspawnAndDestroy();
        }
    }

    void SimpleMoveLeft()
    {
        _rigid.velocity = Vector2.left * _speed;
    }

    void ItemspawnAndDestroy()
    {
        // �÷��̾ ���� �߰�
        GameObject _Player = GameObject.FindGameObjectWithTag("Player");
        PlayerController playerinfo = _Player.GetComponent<PlayerController>();
        playerinfo.AddScore(_score);

        if (ObjectManager.instance == null)
        {
            return;
        }

        if(this.name == "Item_Shielded_Life(Clone)")
        {
            GameObject SpawnItem = ObjectManager.instance.MakeObj("Item_Life");
            SpawnItem.transform.position = transform.position;
        }
        else if(this.name == "Item_Shielded_Power(Clone)")
        {
            GameObject SpawnItem = ObjectManager.instance.MakeObj("Item_Power");            
            SpawnItem.transform.position = transform.position;            
        }
        else if(this.name == "Item_Shielded_Speed(Clone)")
        {
            GameObject SpawnItem = ObjectManager.instance.MakeObj("Item_Speed");
            SpawnItem.transform.position = transform.position;
        }
        else if(this.name == "Item_Shielded_Ult(Clone)")
        {
            GameObject SpawnItem = ObjectManager.instance.MakeObj("Item_Ult");
            SpawnItem.transform.position = transform.position;
        }
        else if(this.name == "Item_Shielded_GuidedAttack(Clone)")
        {
            GameObject SpawnItem = ObjectManager.instance.MakeObj("Item_GuideAttack");
            SpawnItem.transform.position = transform.position;
        }

        if (GameManager.instance == null) GameManager.instance._CanSpawnItemShielded = true;
        Init();
        
        
        gameObject.SetActive(false);
    }

    void Init()
    {
        _life = _lifeMax;
    }
}

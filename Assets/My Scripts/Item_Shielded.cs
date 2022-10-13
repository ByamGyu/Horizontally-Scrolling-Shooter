using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item_Shielded : MonoBehaviour
{
    // ������ �������� ����ִٰ� �ı��Ǹ� �ش� �������� �����ϰ� �������.

    [SerializeField]
    public int _life = 3;
    [SerializeField]
    public int _lifeMax = 3;
    [SerializeField]
    Rigidbody2D _rigid;
    [SerializeField]
    public float _speed = 1.0f;
    [SerializeField]
    public int _score = 50;

    public GameManager gamemanager = null;
    public ObjectManager objectmanager = null;
    

    private void Start()
    {
        _rigid = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        SimpleMoveLeft();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "PlayerBullet") // źȯ�� ������
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

            collision.gameObject.SetActive(false); ; // źȯ ����

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

        Debug.Log(this.name);

        if (objectmanager == null)
        {
            Debug.Log("Item_Shielded's objectmanager is null!");
            return;
        }

        if(this.name == "Item_Shielded_Life(Clone)")
        {
            GameObject SpawnItem = objectmanager.MakeObj("Item_Life");
            SpawnItem.transform.position = transform.position;
        }
        else if(this.name == "Item_Shielded_Power(Clone)")
        {
            GameObject SpawnItem = objectmanager.MakeObj("Item_Power");            
            SpawnItem.transform.position = transform.position;
            
        }
        else if(this.name == "Item_Shielded_Speed(Clone)")
        {
            GameObject SpawnItem = objectmanager.MakeObj("Item_Speed");
            SpawnItem.transform.position = transform.position;
        }
        else if(this.name == "Item_Shielded_Ult(Clone)")
        {
            GameObject SpawnItem = objectmanager.MakeObj("Item_Ult");
            SpawnItem.transform.position = transform.position;
        }
        else if(this.name == "Item_Shielded_GuidedAttack(Clone)")
        {
            GameObject SpawnItem = objectmanager.MakeObj("Item_GuideAttack");
            SpawnItem.transform.position = transform.position;
        }

        if(gamemanager != null) gamemanager._CanSpawnItemShielded = true;
        Init();
        
        
        gameObject.SetActive(false);
    }

    void Init()
    {
        _life = _lifeMax;
    }
}

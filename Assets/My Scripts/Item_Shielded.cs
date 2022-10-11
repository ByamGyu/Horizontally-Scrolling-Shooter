using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item_Shielded : MonoBehaviour
{
    // 스폰할 아이템을 들고있다가 파괴되면 해당 아이템을 스폰하고 사라진다.

    [SerializeField]
    public int _life = 3;
    [SerializeField]
    Rigidbody2D _rigid;
    [SerializeField]
    public float _speed = 1.0f;
    [SerializeField]
    public int _score = 50;

    public ObjectManager objectmanager;
    

    private void Start()
    {
        _rigid = GetComponent<Rigidbody2D>();

        SimpleMoveLeft();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "PlayerBullet") // 탄환에 맞으면
        {
            // 플레이어의 탄환 공격력을 가져와 계산한다.
            Bullet_Base bulletinfo = collision.gameObject.GetComponent<Bullet_Base>();
            _life -= bulletinfo._damage;

            collision.gameObject.SetActive(false); ; // 탄환 제거

            if(_life <= 0)
            {
                _life = 0;

                ItemspawnAndDestroy();
            }
        }
        if (collision.gameObject.tag == "PlayerBullet_Charged") // 탄환에 맞으면
        {
            // 플레이어의 탄환 공격력을 가져와 계산한다.
            Bullet_Base bulletinfo = collision.gameObject.GetComponent<Bullet_Base>();
            _life -= bulletinfo._damage;

            collision.gameObject.SetActive(false); ; // 탄환 제거

            if (_life <= 0)
            {
                _life = 0;

                ItemspawnAndDestroy();
            }
        }
        else if(collision.gameObject.tag == "Player") // 플레이어 기체와 충돌하면
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
        // 플레이어에 점수 추가
        GameObject _Player = GameObject.FindGameObjectWithTag("Player");
        PlayerController playerinfo = _Player.GetComponent<PlayerController>();
        playerinfo.AddScore(_score);

        Debug.Log(this.name);

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

        gameObject.SetActive(false);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item_Shielded : MonoBehaviour
{
    // ������ �������� ����ִٰ� �ı��Ǹ� �ش� �������� �����ϰ� �������.

    [SerializeField]
    public int _life = 3;
    [SerializeField]
    GameObject _spawnItem;
    [SerializeField]
    Rigidbody2D _rigid;
    [SerializeField]
    public float _speed = 1.0f;
    [SerializeField]
    public int _score = 50;
    

    private void Start()
    {
        _rigid = GetComponent<Rigidbody2D>();

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
                ItemspawnAndDestroy();
            }
        }
        else if(collision.gameObject.tag == "Player") // �÷��̾� ��ü�� �浹�ϸ�
        {
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

        // ������ ����
        GameObject SpawnItem = Instantiate(_spawnItem, transform.position, transform.rotation);

        // ��ü �ı�
        gameObject.SetActive(false);
    }
}

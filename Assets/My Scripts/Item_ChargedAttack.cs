using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item_ChargedAttack : Item_Base
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            PlayerController playerinfo = collision.gameObject.GetComponent<PlayerController>();
            playerinfo.SetChargeAttack(true);
            playerinfo.AddScore(_score);

            // ���� ���°� �ƴ� �������� �ϴ� �ı��ϴ� �ɷ�
            Destroy(gameObject);
            //gameObject.SetActive(false);
        }
    }
}

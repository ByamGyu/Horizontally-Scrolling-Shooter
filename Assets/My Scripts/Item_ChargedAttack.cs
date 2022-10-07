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

            // 쉴드 상태가 아닌 아이템은 일단 파괴하는 걸로
            Destroy(gameObject);
            //gameObject.SetActive(false);
        }
    }
}

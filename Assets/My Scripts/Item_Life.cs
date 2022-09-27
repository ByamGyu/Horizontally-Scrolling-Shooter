using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item_Life : Item_Base
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            PlayerController playerinfo = collision.gameObject.GetComponent<PlayerController>();
            playerinfo.SetLife(1); // 라이프 추가
            playerinfo.AddScore(_score); // 점수 추가

            Destroy(gameObject);
        }
    }
}

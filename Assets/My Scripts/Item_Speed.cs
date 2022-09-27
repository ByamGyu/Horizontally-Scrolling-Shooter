using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item_Speed : Item_Base
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            PlayerController playerinfo = collision.gameObject.GetComponent<PlayerController>();
            playerinfo.SetSpeed(1.25f);
            playerinfo.AddScore(_score);

            Destroy(gameObject);
        }
    }
}
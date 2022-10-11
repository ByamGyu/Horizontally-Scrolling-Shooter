using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item_GuideAttack : Item_Base
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            PlayerController playerinfo = collision.gameObject.GetComponent<PlayerController>();
            playerinfo.SetGuideAttack(true);
            playerinfo.AddScore(_score);

            SoundManager.instance.PlaySoundEffectOneShot("Item_UltGet", 0.33f);

            gameObject.SetActive(false);
        }
    }
}

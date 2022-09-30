using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_SerpentBodyPartsCollisionSetting : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {        
        if(collision.gameObject.tag == "PlayerBullet")
        {
            // 탄환 튕기는 소리 재생 추가 필요


            Destroy(collision.gameObject);
        }
    }
}

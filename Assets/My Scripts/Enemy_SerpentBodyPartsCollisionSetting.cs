using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_SerpentBodyPartsCollisionSetting : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {        
        if(collision.gameObject.tag == "PlayerBullet")
        {
            // źȯ ƨ��� �Ҹ� ��� �߰� �ʿ�


            Destroy(collision.gameObject);
        }
    }
}

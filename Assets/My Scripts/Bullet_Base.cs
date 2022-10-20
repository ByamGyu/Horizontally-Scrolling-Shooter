using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet_Base : MonoBehaviour
{
    // public으로 열어두기
    public int _damage = 1;


    void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "BorderBullet")
        {
            gameObject.SetActive(false);
        }
    }
}

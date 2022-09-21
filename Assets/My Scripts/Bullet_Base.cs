using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet_Base : MonoBehaviour
{
    [SerializeField]
    public int _damage = 1;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "BorderBullet")
        {
            Destroy(gameObject);
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundBorder : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Background")
        {
            collision.gameObject.SetActive(false);
        }
        if(collision.gameObject.tag == "EnemyBullet")
        {
            collision.gameObject.SetActive(false);
        }
    }
}

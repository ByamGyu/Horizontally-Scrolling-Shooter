using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundBorder : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Background")
        {
            collision.gameObject.SetActive(false);
        }
    }
}

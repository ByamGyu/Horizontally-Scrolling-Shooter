using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss_Serpent_BulletSpawner : MonoBehaviour
{
    private float _Bullet_Shot_Delay_Cur;
    private float _Bullet_Shot_Delay_Max;


    void Awake()
    {
        _Bullet_Shot_Delay_Max = 4f;
    }

    void Start()
    {
        if (ObjectManager.instance == null)
        {
            Debug.Log("ObjectManager is Null");
        }
    }

    void Update()
    {
        if (ObjectManager.instance == null) return;

        Bullet_Delay();
        Fire();
    }

    void Bullet_Delay()
    {
        if (_Bullet_Shot_Delay_Cur > 5) return;

        _Bullet_Shot_Delay_Cur += Time.deltaTime;
    }

    void Fire()
    {
        // ?Ѿ? ???? ?????? ?ð? ?Ǻ?
        if (_Bullet_Shot_Delay_Cur < _Bullet_Shot_Delay_Max) return;

        GameObject bullettop = ObjectManager.instance.MakeObj("Bullet_Enemy_Green");
        bullettop.transform.position = transform.position + transform.up * 1f;
        bullettop.transform.rotation = transform.rotation;
        bullettop.transform.localScale = new Vector3(2.0f, 2.0f, 2.0f);
        GameObject bulletbottom = ObjectManager.instance.MakeObj("Bullet_Enemy_Green");
        bulletbottom.transform.position = transform.position + (transform.up * 1f * (-1));
        bulletbottom.transform.rotation = transform.rotation;
        bulletbottom.transform.localScale = new Vector3(2.0f, 2.0f, 2.0f);
        Rigidbody2D rigid1 = bullettop.GetComponent<Rigidbody2D>();
        Rigidbody2D rigid2 = bulletbottom.GetComponent<Rigidbody2D>();
        rigid1.AddForce(transform.up * 3, ForceMode2D.Impulse);
        rigid2.AddForce(transform.up * 3 * (-1), ForceMode2D.Impulse);

        _Bullet_Shot_Delay_Cur = 0.0f;
    }
}
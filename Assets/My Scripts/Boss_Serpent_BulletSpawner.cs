using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss_Serpent_BulletSpawner : MonoBehaviour
{
    // enemyInfo.objectmanager = objectManager;
    public ObjectManager _objectmanager;

    public float _Bullet_Shot_Delay_Cur;
    public float _Bullet_Shot_Delay_Max;


    private void Awake()
    {
        _Bullet_Shot_Delay_Max = 4f;
    }

    private void Start()
    {
        _objectmanager = GameManager.instance.objectManager;
        if (_objectmanager == null)
        {
            Debug.Log("ObjectManager is Null");
        }
    }

    private void Update()
    {
        if (_objectmanager == null) return;

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
        // 총알 생성 딜레이 시간 판별
        if (_Bullet_Shot_Delay_Cur < _Bullet_Shot_Delay_Max) return;

        GameObject bullettop = _objectmanager.MakeObj("Bullet_Enemy_Green");
        bullettop.transform.position = transform.position + transform.up * 1f;
        bullettop.transform.rotation = transform.rotation;
        bullettop.transform.localScale = new Vector3(2.0f, 2.0f, 2.0f);
        GameObject bulletbottom = _objectmanager.MakeObj("Bullet_Enemy_Green");
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
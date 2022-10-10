using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss_BulletSpawner : MonoBehaviour
{
    public string _mode;

    public GameObject _Player = null;

    // enemyInfo.objectmanager = objectManager;
    public ObjectManager _objectmanager;

    public float _Bullet_Shot_Delay_Cur;
    public float _Bullet_Shot_Delay_Max;
    float _tmp = 1; // 탄막에 변화를 주는 변수


    private void Awake()
    {
        _Bullet_Shot_Delay_Max = 1.5f;
    }

    private void Start()
    {
        _Player = GameObject.FindGameObjectWithTag("Player");

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
        if(_mode == "FireBigBullet")
        {
            FireBigBullet();
        }
        if(_mode == "FireMultiRandomshotToPlayer")
        {
            FireMultiRandomshotToPlayer(10);
        }
        if(_mode == "FireArc")
        {
            FireArc(0.1f);
        }
        if(_mode == "FireArc2Way")
        {
            FireArc2Way(0.1f);
        }
        if(_mode == "")
        {

        }
    }

    void FireBigBullet()
    {
        _mode = "FireBigBullet";
        _Bullet_Shot_Delay_Max = 2; // 탄환 스폰 시간

        // 총알 생성 딜레이 시간 판별
        if (_Bullet_Shot_Delay_Cur < _Bullet_Shot_Delay_Max) return;

        GameObject bullet = _objectmanager.MakeObj("Bullet_Enemy_Red_Big");
        bullet.transform.position = transform.position + Vector3.left * 0.5f;
        bullet.transform.rotation = transform.rotation;

        Vector3 _dir = (_Player.transform.position - transform.position).normalized;

        Rigidbody2D rigid = bullet.GetComponent<Rigidbody2D>();
        rigid.AddForce(_dir * 2f, ForceMode2D.Impulse);

        _Bullet_Shot_Delay_Cur = 0.0f;
    }

    void FireMultiRandomshotToPlayer(int _cnt)
    {
        _mode = "FireMultiRandomshotToPlayer";
        _Bullet_Shot_Delay_Max = 1.25f; // 탄환 스폰 시간

        // 총알 생성 딜레이 시간 판별
        if (_Bullet_Shot_Delay_Cur < _Bullet_Shot_Delay_Max) return;

        for (int i = 0; i < _cnt; i++)
        {
            GameObject bullet = _objectmanager.MakeObj("Bullet_Enemy_Red");
            bullet.transform.position = transform.position + Vector3.left * 0.5f;
            bullet.transform.rotation = transform.rotation;
            bullet.transform.localScale = new Vector3(2f, 2f, 2f);

            Vector2 _dir = (_Player.transform.position - transform.position).normalized;
            Vector2 _changedir = new Vector2(0, Random.Range(-0.5f, 0.5f));
            _dir += _changedir;

            Rigidbody2D rigid = bullet.GetComponent<Rigidbody2D>();
            rigid.AddForce(_dir * 3f, ForceMode2D.Impulse);
        }

        _Bullet_Shot_Delay_Cur = 0.0f;
    }

    void FireArc(float _fireDelayTime)
    {
        _mode = "FireArc";
        _Bullet_Shot_Delay_Max = _fireDelayTime; // 탄환 스폰 시간

        // 총알 생성 딜레이 시간 판별
        if (_Bullet_Shot_Delay_Cur < _Bullet_Shot_Delay_Max) return;

        GameObject bullet = _objectmanager.MakeObj("Bullet_Enemy_Red");
        bullet.transform.position = transform.position + Vector3.left * 0.5f;
        bullet.transform.rotation = transform.rotation;
        bullet.transform.localScale = new Vector3(2f, 2f, 2f);

        Rigidbody2D rigid = bullet.GetComponent<Rigidbody2D>();
        Vector2 _dir = new Vector2(-1, Mathf.Sin(Mathf.PI * 7.5f * _tmp / 100));
        rigid.AddForce(_dir.normalized * 5, ForceMode2D.Impulse);

        _tmp++;

        _Bullet_Shot_Delay_Cur = 0.0f;
    }

    void FireArc2Way(float _fireDelayTime)
    {
        _mode = "FireArc2Way";
        _Bullet_Shot_Delay_Max = _fireDelayTime; // 탄환 스폰 시간

        // 총알 생성 딜레이 시간 판별
        if (_Bullet_Shot_Delay_Cur < _Bullet_Shot_Delay_Max) return;

        GameObject bullet1 = _objectmanager.MakeObj("Bullet_Enemy_Red");
        bullet1.transform.position = transform.position + Vector3.left * 0.5f;
        bullet1.transform.rotation = transform.rotation;
        bullet1.transform.localScale = new Vector3(2f, 2f, 2f);

        Rigidbody2D rigid1 = bullet1.GetComponent<Rigidbody2D>();
        Vector2 _dir = new Vector2(-1, Mathf.Sin(Mathf.PI * 7.5f * _tmp / 100));
        rigid1.AddForce(_dir.normalized * 5, ForceMode2D.Impulse);

        GameObject bullet2 = _objectmanager.MakeObj("Bullet_Enemy_Red");
        bullet2.transform.position = transform.position + Vector3.left * 0.5f;
        bullet2.transform.rotation = transform.rotation;
        bullet2.transform.localScale = new Vector3(2f, 2f, 2f);

        Rigidbody2D rigid2 = bullet2.GetComponent<Rigidbody2D>();
        Vector2 _dir2 = new Vector2(-1, Mathf.Sin(-Mathf.PI * 7.5f * _tmp / 100));
        rigid2.AddForce(_dir2.normalized * 5, ForceMode2D.Impulse);

        _tmp++;

        _Bullet_Shot_Delay_Cur = 0.0f;
    }

    void FireCircle()
    {

    }
}

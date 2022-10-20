using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss_Claw_BulletSpawner : MonoBehaviour
{
    public GameObject _Player = null;
    public GameObject _mainbodyinfo = null;

    public bool _CanFireBigBullet = false;
    public bool _CanFireMultiRandomshotToPlayer = false;
    public bool _CanFireArc = false;
    public bool _CanFireArc2Way = false;
    public bool _CanFireCircle = false ;

    public float _Bullet_Shot_Delay_FireBigBullet_Cur;
    public float _Bullet_Shot_Delay_FireBigBullet_Max;
    public float _Bullet_Shot_Delay_FireMultiRandomshotToPlayer_Cur;
    public float _Bullet_Shot_Delay_FireMultiRandomshotToPlayer_Max;
    public float _Bullet_Shot_Delay_FireArc_Cur;
    public float _Bullet_Shot_Delay_FireArc_Max;
    public float _Bullet_Shot_Delay_FireArc2Way_Cur;
    public float _Bullet_Shot_Delay_FireArc2Way_Max;
    public float _Bullet_Shot_Delay_FireCircle_Cur;
    public float _Bullet_Shot_Delay_FireCircle_Max;

    float _tmp = 1; // 탄막에 변화를 주는 변수


    void Awake()
    {

    }

    void Start()
    {
        _Player = GameObject.FindGameObjectWithTag("Player");

        
        if (ObjectManager.instance == null)
        {
            Debug.Log("ObjectManager is Null");
        }

        InitAll();
    }

    void Update()
    {
        if (ObjectManager.instance == null) return;

        Bullet_Delay();

        if (_CanFireBigBullet == true) FireBigBullet();
        if (_CanFireMultiRandomshotToPlayer == true) FireMultiRandomshotToPlayer(10);
        if (_CanFireArc == true) FireArc(0.1f);
        if (_CanFireArc2Way) FireArc2Way(0.1f);
        if (_CanFireCircle) FireCircle(50);
    }

    void InitAll()
    {
        _Bullet_Shot_Delay_FireBigBullet_Max = 2f;
        _Bullet_Shot_Delay_FireMultiRandomshotToPlayer_Max = 1.25f;
        _Bullet_Shot_Delay_FireArc_Max = 0.1f;
        _Bullet_Shot_Delay_FireArc2Way_Max = 0.1f;
        _Bullet_Shot_Delay_FireCircle_Max = 3f;

        _Bullet_Shot_Delay_FireBigBullet_Cur = 0;
        _Bullet_Shot_Delay_FireMultiRandomshotToPlayer_Cur = 0;
        _Bullet_Shot_Delay_FireArc_Cur = 0;
        _Bullet_Shot_Delay_FireArc2Way_Cur = 0;
        _Bullet_Shot_Delay_FireCircle_Cur = 0;
    }

    public void InitBulletDelayCurAll()
    {
        _Bullet_Shot_Delay_FireBigBullet_Cur = 0;
        _Bullet_Shot_Delay_FireMultiRandomshotToPlayer_Cur = 0;
        _Bullet_Shot_Delay_FireArc_Cur = 0;
        _Bullet_Shot_Delay_FireArc2Way_Cur = 0;
        _Bullet_Shot_Delay_FireCircle_Cur = 0;
    }

    public void TurnOffAllFire()
    {
        _CanFireBigBullet = false;
        _CanFireMultiRandomshotToPlayer = false;
        _CanFireArc = false;
        _CanFireArc2Way = false;
        _CanFireCircle = false;
    }

    void Bullet_Delay()
    {
        if (_Bullet_Shot_Delay_FireBigBullet_Cur > 5) { }
        else _Bullet_Shot_Delay_FireBigBullet_Cur += Time.deltaTime;

        if (_Bullet_Shot_Delay_FireMultiRandomshotToPlayer_Cur > 5) { }
        else _Bullet_Shot_Delay_FireMultiRandomshotToPlayer_Cur += Time.deltaTime;

        if (_Bullet_Shot_Delay_FireArc_Cur > 5) { }
        else _Bullet_Shot_Delay_FireArc_Cur += Time.deltaTime;

        if (_Bullet_Shot_Delay_FireArc2Way_Cur > 5) { }
        else _Bullet_Shot_Delay_FireArc2Way_Cur += Time.deltaTime;

        if (_Bullet_Shot_Delay_FireCircle_Cur > 5) { }
        else _Bullet_Shot_Delay_FireCircle_Cur += Time.deltaTime;
    }

    void FireBigBullet()
    {
        _Bullet_Shot_Delay_FireBigBullet_Max = 2; // 탄환 스폰 시간

        // 총알 생성 딜레이 시간 판별
        if (_Bullet_Shot_Delay_FireBigBullet_Cur < _Bullet_Shot_Delay_FireBigBullet_Max) return;

        GameObject bullet = ObjectManager.instance.MakeObj("Bullet_Enemy_Red_Big");
        bullet.transform.position = transform.position + Vector3.left * 0.5f;
        bullet.transform.rotation = transform.rotation;

        Vector3 _dir = (_Player.transform.position - transform.position).normalized;

        Rigidbody2D rigid = bullet.GetComponent<Rigidbody2D>();
        rigid.AddForce(_dir * 2f, ForceMode2D.Impulse);

        _Bullet_Shot_Delay_FireBigBullet_Cur = 0.0f;
    }

    void FireMultiRandomshotToPlayer(int _cnt)
    {
        _Bullet_Shot_Delay_FireMultiRandomshotToPlayer_Max = 1.25f; // 탄환 스폰 시간

        // 총알 생성 딜레이 시간 판별
        if (_Bullet_Shot_Delay_FireMultiRandomshotToPlayer_Cur < _Bullet_Shot_Delay_FireMultiRandomshotToPlayer_Max) return;

        for (int i = 0; i < _cnt; i++)
        {
            GameObject bullet = ObjectManager.instance.MakeObj("Bullet_Enemy_Red");
            bullet.transform.position = transform.position + Vector3.left * 0.5f;
            bullet.transform.rotation = transform.rotation;
            bullet.transform.localScale = new Vector3(2f, 2f, 2f);

            Vector2 _dir = (_Player.transform.position - transform.position).normalized;
            Vector2 _changedir = new Vector2(0, Random.Range(-0.5f, 0.5f));
            _dir += _changedir;

            Rigidbody2D rigid = bullet.GetComponent<Rigidbody2D>();
            rigid.AddForce(_dir * 3f, ForceMode2D.Impulse);
        }

        _Bullet_Shot_Delay_FireMultiRandomshotToPlayer_Cur = 0.0f;
    }

    void FireArc(float _fireDelayTime)
    {
        _Bullet_Shot_Delay_FireArc_Max = _fireDelayTime; // 탄환 스폰 시간

        // 총알 생성 딜레이 시간 판별
        if (_Bullet_Shot_Delay_FireArc_Cur < _Bullet_Shot_Delay_FireArc_Max) return;

        GameObject bullet = ObjectManager.instance.MakeObj("Bullet_Enemy_Red");
        bullet.transform.position = transform.position + Vector3.left * 0.5f;
        bullet.transform.rotation = transform.rotation;
        bullet.transform.localScale = new Vector3(2f, 2f, 2f);

        Rigidbody2D rigid = bullet.GetComponent<Rigidbody2D>();
        Vector2 _dir = new Vector2(-1, Mathf.Sin(Mathf.PI * 7.5f * _tmp / 100));
        rigid.AddForce(_dir.normalized * 5, ForceMode2D.Impulse);

        _tmp++;

        _Bullet_Shot_Delay_FireArc_Cur = 0.0f;
    }

    void FireArc2Way(float _fireDelayTime)
    {
        _Bullet_Shot_Delay_FireArc2Way_Max = _fireDelayTime; // 탄환 스폰 시간

        // 총알 생성 딜레이 시간 판별
        if (_Bullet_Shot_Delay_FireArc2Way_Cur < _Bullet_Shot_Delay_FireArc2Way_Max) return;

        GameObject bullet1 = ObjectManager.instance.MakeObj("Bullet_Enemy_Red");
        bullet1.transform.position = transform.position + Vector3.left * 0.5f;
        bullet1.transform.rotation = transform.rotation;
        bullet1.transform.localScale = new Vector3(2f, 2f, 2f);
        Rigidbody2D rigid1 = bullet1.GetComponent<Rigidbody2D>();
        Vector2 _dir = new Vector2(-1, Mathf.Sin(Mathf.PI * 7.5f * _tmp / 100));
        rigid1.AddForce(_dir.normalized * 5, ForceMode2D.Impulse);

        GameObject bullet2 = ObjectManager.instance.MakeObj("Bullet_Enemy_Red");
        bullet2.transform.position = transform.position + Vector3.left * 0.5f;
        bullet2.transform.rotation = transform.rotation;
        bullet2.transform.localScale = new Vector3(2f, 2f, 2f);
        Rigidbody2D rigid2 = bullet2.GetComponent<Rigidbody2D>();
        Vector2 _dir2 = new Vector2(-1, Mathf.Sin(-Mathf.PI * 7.5f * _tmp / 100));
        rigid2.AddForce(_dir2.normalized * 5, ForceMode2D.Impulse);

        _tmp++;

        _Bullet_Shot_Delay_FireArc2Way_Cur = 0.0f;
    }

    void FireCircle(int _cnt)
    {
        _Bullet_Shot_Delay_FireCircle_Max = 3.0f; // 탄환 스폰 시간

        // 총알 생성 딜레이 시간 판별
        if (_Bullet_Shot_Delay_FireCircle_Cur < _Bullet_Shot_Delay_FireCircle_Max) return;

        for (int i = 0; i < _cnt; i++)
        {
            GameObject bullet = ObjectManager.instance.MakeObj("Bullet_Enemy_Red");
            bullet.transform.position = transform.position + Vector3.left * 0.5f;
            bullet.transform.rotation = transform.rotation;
            bullet.transform.localScale = new Vector3(2f, 2f, 2f);

            Rigidbody2D rigid = bullet.GetComponent<Rigidbody2D>();
            Vector2 _dir = new Vector2(Mathf.Sin(Mathf.PI * 2 * i / _cnt), Mathf.Cos(Mathf.PI * 2 * i / _cnt));
            rigid.AddForce(_dir.normalized * 2, ForceMode2D.Impulse);
        }

        _Bullet_Shot_Delay_FireCircle_Cur = 0.0f;
    }
}

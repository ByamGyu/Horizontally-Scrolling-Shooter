using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Ring : Enemy_Base
{
    // 탄환 발사 딜레이 변수
    [SerializeField]
    float _Bullet_Shot_Delay_Cur = 0.0f;
    [SerializeField]
    float _Bullet_Shot_Delay_Max = 3.5f;
    [SerializeField]
    float _y = 1;

    [SerializeField]
    float _movementScale = 1f;

    float _startxpos;
    float _startypos;

    public bool _CanMoveSin = false;
    public bool _CanMoveCos = false;
    public bool _CanMoveLeft = false;
    public bool _CanMoveUp = false;
    public bool _CanMoveDown = false;


    void Start()
    {
        _startxpos = transform.position.x;
        _startypos = transform.position.y;
        _rigid = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        Fire_Bullet();
        Bullet_Delay();

        _y += Time.deltaTime;

        if (_CanMoveSin == true) MoveSin();
        if (_CanMoveCos == true) MoveCos();

        if (_CanMoveLeft == true) SimpleMoveLeft();
        if (_CanMoveUp == true) SimpleMoveUp();
        if (_CanMoveDown == true) SimpleMoveDown();
    }

    public override void SimpleMoveLeft()
    {
        // 60프레임(FixedUpdate)에 맞춤
        transform.position += new Vector3(-0.017f * _speed, 0, 0);
    }

    public override void SimpleMoveUp()
    {
        // 60프레임(FixedUpdate)에 맞춤
        transform.position += new Vector3(0, 0.017f * _speed * 0.5f, 0);
    }

    public override void SimpleMoveDown()
    {
        // 60프레임(FixedUpdate)에 맞춤
        transform.position += new Vector3(0, -0.017f * _speed * 0.5f, 0);
    }
    
    public void MoveSin()
    {
        // 60프레임(FixedUpdate)에 맞춤
        transform.localPosition += new Vector3(0, Mathf.Sin(_y) * 0.017f * _movementScale, 0);
    }

    public void MoveCos()
    {
        // 60프레임(FixedUpdate)에 맞춤
        transform.localPosition += new Vector3(Mathf.Cos(_y) * 0.017f * _movementScale, 0, 0);
    }


    void Fire_Bullet()
    {
        // 총알 생성 딜레이 시간 판별
        if (_Bullet_Shot_Delay_Cur < _Bullet_Shot_Delay_Max) return;

        GameObject bulletmid = objectmanager.MakeObj("Bullet_Enemy_Green");
        bulletmid.transform.position = transform.position + Vector3.left * 0.5f;
        bulletmid.transform.rotation = transform.rotation;
        GameObject bullettop = objectmanager.MakeObj("Bullet_Enemy_Green");
        bullettop.transform.position = transform.position + Vector3.up * 0.5f;
        bullettop.transform.rotation = transform.rotation;
        GameObject bulletbottom = objectmanager.MakeObj("Bullet_Enemy_Green");
        bulletbottom.transform.position = transform.position + Vector3.down * 0.5f;
        bulletbottom.transform.rotation = transform.rotation;
        Rigidbody2D rigid1 = bulletmid.GetComponent<Rigidbody2D>();
        Rigidbody2D rigid2 = bullettop.GetComponent<Rigidbody2D>();
        Rigidbody2D rigid3 = bulletbottom.GetComponent<Rigidbody2D>();
        rigid1.AddForce(Vector2.left * 5, ForceMode2D.Impulse);
        rigid2.AddForce(Vector2.up * 5 + (Vector2.left * _speed), ForceMode2D.Impulse);
        rigid3.AddForce(Vector2.down * 5 + (Vector2.left * _speed), ForceMode2D.Impulse);

        _Bullet_Shot_Delay_Cur = 0.0f;
    }

    void Bullet_Delay()
    {
        if (_Bullet_Shot_Delay_Cur > 10) return;

        _Bullet_Shot_Delay_Cur += Time.deltaTime;
    }

    public override void Init()
    {
        _life = _MaxLife;
        _Bullet_Shot_Delay_Cur = 0;
        _CanMoveSin = false;
        _CanMoveCos = false;
        _CanMoveLeft = false;
        _CanMoveUp = false;
        _CanMoveDown = false;
    }
}
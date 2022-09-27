using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    // 게임 매니저 저장
    public GameManager manager;

    // 플레이어 능력치
    [SerializeField]
    float _speed = 5.0f; // 이동 속도 변수
    [SerializeField]
    float _Bullet_Shot_Delay_Cur = 0.0f;
    [SerializeField]
    float _Bullet_Shot_Delay_Max = 0.2f;
    [SerializeField]
    float _GuideAttack_Delay_Cur = 0.0f;
    [SerializeField]
    float _GuideAttack_Delay_Max = 3.0f;
    [SerializeField]
    int _life = 3;
    [SerializeField]
    int _power = 1;
    [SerializeField]
    bool _guideattack = false;
    [SerializeField]
    bool _chargeattack = false;
    [SerializeField]
    int _ult = 3;
    [SerializeField]
    int _score = 0;
    [SerializeField]
    float _invincibleTime = 2.5f;
    [SerializeField]
    bool _isHit = false;

    // 플레이어 이동 판정
    [SerializeField]
    bool _isTouchTop = false;
    [SerializeField]
    bool _isTouchBottom = false;
    [SerializeField]
    bool _isTouchRight = false;
    [SerializeField]
    bool _isTouchLeft = false;

    // 오브젝트 저장 변수
    [SerializeField]
    public GameObject _Bullet1; // 필요한 오브젝트를 붙일 수 있다. (카메라, 물체 등등)
    [SerializeField]
    public GameObject _Bullet2;
    [SerializeField]
    public GameObject _Bullet3;
    [SerializeField]
    public GameObject _Bullet4;
    [SerializeField]
    public GameObject _Bullet5;
    [SerializeField]
    public GameObject _Bullet6;
    [SerializeField]
    public GameObject _Bullet7;


    void Start()
    {
        manager.UpdateLifeIcon(GetLife());
    }

    void Update()
    {
        CalculateInvincible();
        Move();
        Fire_Bullet();
        Bullet_Delay();
        GuideAttack_Delay();
    }

    void CalculateInvincible()
    {
        if (_invincibleTime <= 0) // 무적시간이 없으면
        {
            _invincibleTime = 0.0f;
        }
        else // 무적시간이 남아있으면
        {
            _invincibleTime -= Time.deltaTime;
        }
    }

    public void SetInvincibleTime(float time)
    {
        _invincibleTime = time;
    }

    void Move()
    {
        // Input.GetAxisRaw()는 방향 값을 추출하는 함수
        float _height = Input.GetAxisRaw("Horizontal"); // 키보드 수평 이동 입력 받기
        float _vertical = Input.GetAxisRaw("Vertical"); // 키보드 수직 이동 입력 받기

        // 경계선 이동 제한하기
        if ((_isTouchTop == true && _vertical == 1) || (_isTouchBottom == true && _vertical == -1)) _vertical = 0;
        if ((_isTouchRight == true && _height == 1) || (_isTouchLeft == true && _height == -1)) _height = 0;

        Vector3 CurPos = transform.position; // 현재 위치
        Vector3 NextPos = new Vector3(_height, _vertical, 0) * _speed * Time.deltaTime; // 이동하는 거리

        transform.position = CurPos + NextPos; // 다음 위치
    }

    void Fire_Bullet()
    {
        // '['를 누르면 발사
        if (!Input.GetKey(KeyCode.LeftBracket)) return;

        // 총알 생성 딜레이 시간 판별
        if (_Bullet_Shot_Delay_Cur < _Bullet_Shot_Delay_Max) return;

        switch(_power) // 일반 탄환
        {
            case 1: // 파워레벨 1
                // 프리팹(_Bullet1)을 오브젝트로 생성, 생성 위치, 생성 방향
                GameObject bulletMid1 = Instantiate(_Bullet1, transform.position + Vector3.right * 0.5f, transform.rotation);

                // 탄환에 힘을 가해 움직이게 한다.
                Rigidbody2D rigid1 = bulletMid1.GetComponent<Rigidbody2D>();

                rigid1.AddForce(Vector2.right * 10, ForceMode2D.Impulse);

                // 총알 생성 딜레이 시간 초기화
                _Bullet_Shot_Delay_Cur = 0.0f;
                break;
            case 2: // 파워레벨 2
                GameObject bulletTop2 = Instantiate(_Bullet1, transform.position + Vector3.up * 0.15f + Vector3.right * 0.5f, transform.rotation);
                GameObject bulletDown2 = Instantiate(_Bullet1, transform.position + Vector3.down * 0.15f + Vector3.right * 0.5f, transform.rotation);

                Rigidbody2D rigidTop2 = bulletTop2.GetComponent<Rigidbody2D>();
                Rigidbody2D rigidBottom2 = bulletDown2.GetComponent<Rigidbody2D>();

                rigidTop2.AddForce(Vector2.right * 10, ForceMode2D.Impulse);
                rigidBottom2.AddForce(Vector2.right * 10, ForceMode2D.Impulse);

                _Bullet_Shot_Delay_Cur = 0.0f;
                break;
            case 3: // 파워레벨 3
                GameObject bulletTop3 = Instantiate(_Bullet1, transform.position + Vector3.up * 0.25f + Vector3.right * 0.5f, transform.rotation);
                GameObject bulletMid3 = Instantiate(_Bullet1, transform.position + Vector3.right * 0.75f, transform.rotation);
                GameObject bulletBottom3 = Instantiate(_Bullet1, transform.position + Vector3.down * 0.25f + Vector3.right * 0.5f, transform.rotation);

                Rigidbody2D rigidTop3 = bulletTop3.GetComponent<Rigidbody2D>();
                Rigidbody2D rigidMid3 = bulletMid3.GetComponent<Rigidbody2D>();
                Rigidbody2D rigidBottom3 = bulletBottom3.GetComponent<Rigidbody2D>();

                rigidTop3.AddForce(Vector2.right * 10, ForceMode2D.Impulse);
                rigidMid3.AddForce(Vector2.right * 10, ForceMode2D.Impulse);
                rigidBottom3.AddForce(Vector2.right * 10, ForceMode2D.Impulse);

                _Bullet_Shot_Delay_Cur = 0.0f;
                break;
            case 4: // 파워레벨 4
                GameObject bulletMid4 = Instantiate(_Bullet5, transform.position + Vector3.right * 0.5f, transform.rotation);
                
                Rigidbody2D rigidMid1 = bulletMid4.GetComponent<Rigidbody2D>();

                rigidMid1.AddForce(Vector2.right * 10, ForceMode2D.Impulse);

                _Bullet_Shot_Delay_Cur = 0.0f;
                break;
            case 5: // 테스트 용도
                GameObject test = Instantiate(_Bullet6, transform.position + Vector3.right * 0.5f, transform.rotation);
                
                _Bullet_Shot_Delay_Cur = 0.0f;
                break;
        }
    }

    void Fire_ChargeAttack()
    {
        // 누르는 시간에 비례해서 스케일을 키우는 방식으로
        // 누르는 시간에 비례해서 데미지도 증가
    }

    void Fire_GuideAttack()
    {
        // 유도탄 딜레이 시간 판별
        if (_Bullet_Shot_Delay_Cur < _Bullet_Shot_Delay_Max) return;

        // 유도탄 프리팹 스폰하는 방식으로

        _GuideAttack_Delay_Cur = 0.0f;
    }

    void Bullet_Delay()
    {
        if (_Bullet_Shot_Delay_Cur > 10) return;

        _Bullet_Shot_Delay_Cur += Time.deltaTime;
    }

    void GuideAttack_Delay()
    {
        if (_GuideAttack_Delay_Cur > 10) return;

        _GuideAttack_Delay_Cur += Time.deltaTime;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Border") // 경계선 이동 제한
        {
            switch (collision.gameObject.name)
            {
                case "Border_Top":
                    _isTouchTop = true;
                    break;
                case "Border_Bottom":
                    _isTouchBottom = true;
                    break;
                case "Border_Right":
                    _isTouchRight = true;
                    break;
                case "Border_Left":
                    _isTouchLeft = true;
                    break;
            }
        }
        else if (collision.gameObject.tag == "EnemyBullet" || collision.gameObject.tag == "Enemy" || collision.gameObject.tag == "Item_Shielded")
        {
            // 무적상태면 return;
            if (_invincibleTime > 0) return;

            // 한번에 여러번 맞는 현상 방지
            if (_isHit == true) return;
            _isHit = true;

            if (collision.gameObject.tag == "Enemy")
            {
                Enemy_Base enemyinfo = collision.gameObject.GetComponent<Enemy_Base>();
                AddScore(enemyinfo.GetScore());
            }
            else if (collision.gameObject.tag == "EnemyBullet")
            {
                Destroy(collision.gameObject);
            }
            else if(collision.gameObject.tag == "Item_Shielded")
            {
                Destroy(collision.gameObject);
            }

            SetLife(-1);            

            manager.RespawnPlayerInvoke(2.0f);
            gameObject.SetActive(false);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Border")
        {
            switch (collision.gameObject.name)
            {
                case "Border_Top":
                    _isTouchTop = false;
                    break;
                case "Border_Bottom":
                    _isTouchBottom = false;
                    break;
                case "Border_Right":
                    _isTouchRight = false;
                    break;
                case "Border_Left":
                    _isTouchLeft = false;
                    break;
            }
        }
    }

    public void SetLife(int tmp)
    {
        _life = _life + tmp;

        if(_life <= -1)
        {
            _life = -1;

            manager.GameOver();
        }

        // 최대 체력은 3
        if (_life >= 3) _life = 3;

        manager.UpdateLifeIcon(GetLife());
    }

    public int GetLife() { return _life; }
    public void AddScore(int score) { _score += score; }
    public int GetScore() { return _score; }
    public void SetIsHit(bool tmp) { _isHit = tmp; }
    public bool GetIsHit() { return _isHit; }

    public void SetPower(int tmp)
    {
        _power += tmp;

        if (_power >= 4) _power = 4;
        else if (_power <= 1) _power = 1;
    }

    public void SetSpeed(float tmp)
    {
        _speed += tmp;

        if (_speed >= 7.5f) _speed = 7.5f;
        else if (_speed <= 2.5f) _speed = 2.5f;
    }

    public void SetUlt(int tmp)
    {
        _ult += tmp;

        if (_ult >= 3) _ult = 3;
        else if (_ult <= 0) _ult = 0;
    }

    public void SetGuideAttack(bool tmp)
    {
        _guideattack = tmp;
    }

    public void SetChargeAttack(bool tmp)
    {
        _chargeattack = tmp;
    }

    public bool GetChargeAttack()
    {
        return _chargeattack;
    }

}
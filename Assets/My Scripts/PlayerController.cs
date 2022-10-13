using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    // 게임 매니저 저장
    public GameManager gamemanager;

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
    float _GuideAttack_Delay_Max = 2.0f;
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

    // 플레이어 차지 공격 관련
    [SerializeField]
    bool _isCharging = false;
    [SerializeField]
    float _maxChargeTime = 3.0f;
    [SerializeField]
    float _chargeTime = 0f;
    
    // 플레이어 차지 공격 효과음 재생, 끊기를 위한 변수
    bool _soundeffectchargedisplay = false;

    // 플레이어 차지 공격 이펙트 재생, 끊기를 위한 변수
    bool _effectChargeisplay = false;
    GameObject _effecttmp;

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
    [SerializeField]
    public GameObject _Ult;

    // 효과음 변수
    [SerializeField]
    public Dictionary<string, AudioClip> _SoundEffects;

    // 오브젝트 매니저(오브젝트 풀링)
    public ObjectManager objectmanager;


    void Start()
    {
        gamemanager.UpdateLifeIcon(GetLife());
        gamemanager.UpdateUltIcon(GetUlt());
        gamemanager.UpdateChargeGuage(0);
    }

    void Update() // FixedUpdate()를 사용하면 입력이 씹히는 현상 발생
    {
        // 무적 시간
        CalculateInvincible();

        // 이동 입력
        Move();

        // 충전(charge) 공격 관련
        Calculate_ChargeAttackTime();

        // 기본 탄환 발사 관련
        Fire_DefaultBullet();
        Bullet_Delay();

        // 유도탄 관련
        GuideAttack_Delay();
        Fire_GuideAttack();

        // 궁극기 관련
        Fire_Ult();
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


    
    void Calculate_ChargeAttackTime()
    {
        // ']'키에서 손을 떼면
        if(Input.GetKeyUp(KeyCode.RightBracket) == true)
        {
            Fire_ChargeAttack(_chargeTime / _maxChargeTime);
            _isCharging = false;
            _effectChargeisplay = false;

            if (_effecttmp != null) Destroy(_effecttmp);

            _chargeTime = 0;            
            gamemanager.UpdateChargeGuage(0);
            return;
        }

        // 키가 눌리지 않았으면 사용되는 관련 변수들 초기화
        if (!Input.GetKey(KeyCode.RightBracket))
        {
            _isCharging = false;
            _chargeTime = 0;
            return;
        }

        // ']'를 누르면 함수 실행 (충전)
        if (Input.GetKey(KeyCode.RightBracket) == true)
        {
            _isCharging = true;
            _chargeTime += Time.deltaTime;


            if(_chargeTime / _maxChargeTime >= 1.0f)
            {
                if(_soundeffectchargedisplay == false)
                {
                    SoundManager.instance.PlaySoundEffectOneShot("Player_ChargeFinish", 0.75f);
                    SoundManager.instance.PlaySoundEffectbyAudioSource("Player_Charged", true, 0.75f);
                    _soundeffectchargedisplay = true;
                }
            }
            else if(_chargeTime / _maxChargeTime < 1.0f && _chargeTime >= 0.1f)
            {
                //Player_Charging1_3sec
                SoundManager.instance.PlaySoundEffectbyAudioSource("Player_Charging1_3sec", true, 0.75f);
            }

            if (_chargeTime >= _maxChargeTime) _chargeTime = _maxChargeTime;
        }

        // 0.1초 보다 오래 눌렸고, _isCharging이 true면
        if(_chargeTime >= 0.1 && _isCharging == true)
        {
            // 차지 공격 게이지 갱신
            gamemanager.UpdateChargeGuage(_chargeTime, _maxChargeTime);

            if(_effectChargeisplay == false)
            {
                _effectChargeisplay = true;

                _effecttmp = EffectManager.instance.GetEffect
                    ("Effect_Player_Charge",
                    transform.position + Vector3.right * 0.5f,
                    new Vector3(transform.rotation.x, transform.rotation.y, transform.rotation.z),
                    this.transform);
            }            
        }
    }

    void Fire_ChargeAttack(float percent)
    {
        if (percent < 0.1) return;

        // 100% ~ 300%
        float finalpercent = 1 + percent * 2;

        // 차지 공격 객체 스폰
        GameObject chargedbullet = objectmanager.MakeObj("Bullet_Player_Charge");
        chargedbullet.transform.position = transform.position + Vector3.right * 1f;
        chargedbullet.transform.rotation = transform.rotation;
        Rigidbody2D rigid = chargedbullet.GetComponent<Rigidbody2D>();

        // 크기 조절 (2 ~ 6배 사이)
        chargedbullet.transform.localScale = new Vector3(finalpercent, finalpercent, finalpercent) * 2;

        // 데미지 조절 (10 ~ 30 사이)
        Bullet_Base bulletinfo = chargedbullet.GetComponent<Bullet_Base>();
        bulletinfo._damage = (int)(10 * (finalpercent));

        // 속도는 percent 매개변수에 따라서 1 ~ 3배 사이
        rigid.AddForce((Vector2.right * 10) * (finalpercent), ForceMode2D.Impulse);

        // 차지 공격 효과음은 오디오 소스로 재생중임, 멈춰줌
        SoundManager.instance.StopSoundEffectAudioSource();

        if (percent <= 0.33 && percent > 0.1)
        {
            SoundManager.instance.PlaySoundEffectOneShot("Player_ChargeAttackSmall", 0.75f);
        }
        else if(percent <= 1f && percent >0.33)
        {
            SoundManager.instance.PlaySoundEffectOneShot("Player_ChargeAttackBig", 0.75f);
        }

        _soundeffectchargedisplay = false;
    }

    void Fire_DefaultBullet()
    {
        // '['를 누르면 발사
        if (!Input.GetKey(KeyCode.LeftBracket)) return;

        // 총알 생성 딜레이 시간 판별
        if (_Bullet_Shot_Delay_Cur < _Bullet_Shot_Delay_Max) return;

        switch(_power) // 일반 탄환
        {
            case 1: // 파워레벨 1
                // 프리팹(_Bullet1)을 오브젝트로 생성, 생성 위치, 생성 방향
                // _Bullet1, transform.position + Vector3.right * 0.5f, transform.rotation
                GameObject bulletMid1 = objectmanager.MakeObj("Bullet_Player_Default");
                bulletMid1.transform.position = transform.position + Vector3.right * 0.5f;
                bulletMid1.transform.rotation = transform.rotation;

                // 탄환에 힘을 가해 움직이게 한다.
                Rigidbody2D rigid1 = bulletMid1.GetComponent<Rigidbody2D>();

                rigid1.AddForce(Vector2.right * 10, ForceMode2D.Impulse);

                // 총알 생성 딜레이 시간 초기화
                _Bullet_Shot_Delay_Cur = 0.0f;
                break;
            case 2: // 파워레벨 2
                GameObject bulletTop2 = objectmanager.MakeObj("Bullet_Player_Default");
                bulletTop2.transform.position = transform.position + Vector3.up * 0.15f + Vector3.right * 0.5f;
                bulletTop2.transform.rotation = transform.rotation;
                GameObject bulletDown2 = objectmanager.MakeObj("Bullet_Player_Default");
                bulletDown2.transform.position = transform.position + Vector3.down * 0.15f + Vector3.right * 0.5f;
                bulletDown2.transform.rotation = transform.rotation;

                Rigidbody2D rigidTop2 = bulletTop2.GetComponent<Rigidbody2D>();
                Rigidbody2D rigidBottom2 = bulletDown2.GetComponent<Rigidbody2D>();

                rigidTop2.AddForce(Vector2.right * 10, ForceMode2D.Impulse);
                rigidBottom2.AddForce(Vector2.right * 10, ForceMode2D.Impulse);

                _Bullet_Shot_Delay_Cur = 0.0f;
                break;
            case 3: // 파워레벨 3
                GameObject bulletTop3 = objectmanager.MakeObj("Bullet_Player_Default");
                bulletTop3.transform.position = transform.position + Vector3.up * 0.25f + Vector3.right * 0.5f;
                bulletTop3.transform.rotation = transform.rotation;
                GameObject bulletMid3 = objectmanager.MakeObj("Bullet_Player_Default");
                bulletMid3.transform.position = transform.position + Vector3.right * 0.75f;
                bulletMid3.transform.rotation = transform.rotation;
                GameObject bulletBottom3 = objectmanager.MakeObj("Bullet_Player_Default");
                bulletBottom3.transform.position = transform.position + Vector3.down * 0.25f + Vector3.right * 0.5f;
                bulletBottom3.transform.rotation = transform.rotation;

                Rigidbody2D rigidTop3 = bulletTop3.GetComponent<Rigidbody2D>();
                Rigidbody2D rigidMid3 = bulletMid3.GetComponent<Rigidbody2D>();
                Rigidbody2D rigidBottom3 = bulletBottom3.GetComponent<Rigidbody2D>();

                rigidTop3.AddForce(Vector2.right * 10, ForceMode2D.Impulse);
                rigidMid3.AddForce(Vector2.right * 10, ForceMode2D.Impulse);
                rigidBottom3.AddForce(Vector2.right * 10, ForceMode2D.Impulse);

                _Bullet_Shot_Delay_Cur = 0.0f;
                break;
            case 4: // 파워레벨 4
                // _Bullet5, transform.position + Vector3.right * 0.5f, transform.rotation
                GameObject bulletMid4 = objectmanager.MakeObj("Bullet_Player_MaxPower");
                bulletMid4.transform.position = transform.position + Vector3.right * 0.5f;
                bulletMid4.transform.rotation = transform.rotation;

                Rigidbody2D rigidMid1 = bulletMid4.GetComponent<Rigidbody2D>();

                rigidMid1.AddForce(Vector2.right * 10, ForceMode2D.Impulse);

                _Bullet_Shot_Delay_Cur = 0.0f;
                break;
            case 5: // 테스트 용도                
                break;
        }        
    }

    void Fire_Ult()
    {
        if (_ult <= 0) return;

        // '스페이스바'를 누르면 발사
        if (!Input.GetKeyDown(KeyCode.Space)) return;

        // 프리팹(_Bullet1)을 오브젝트로 생성, 생성 위치, 생성 방향
        GameObject Ult = Instantiate(_Ult, new Vector3(0, 0, 0), Quaternion.Euler(new Vector3(90f, 0, 0f)));

        SetUlt(-1);
        gamemanager.UpdateUltIcon(GetUlt());
    }

    void GuideAttack_Delay()
    {
        if (_GuideAttack_Delay_Cur > 10) return;

        _GuideAttack_Delay_Cur += Time.deltaTime;
    }

    void Fire_GuideAttack()
    {
        // 유도탄 아이템을 먹었는지 판별
        if (_guideattack == false) return;

        // 유도탄 딜레이 시간 판별
        if (_GuideAttack_Delay_Cur < _GuideAttack_Delay_Max) return;

        // 유도탄 프리팹 스폰하는 방식으로
        GameObject chasebulletTop = objectmanager.MakeObj("Bullet_Player_Guide");
        GameObject chasebulletBottom = objectmanager.MakeObj("Bullet_Player_Guide");
        chasebulletTop.transform.position = transform.position + Vector3.up * 0.5f;
        chasebulletTop.transform.rotation = Quaternion.Euler(new Vector3(0, 0, 90f));
        chasebulletBottom.transform.position = transform.position + Vector3.down * 0.5f;
        chasebulletBottom.transform.rotation = Quaternion.Euler(new Vector3(0, 0, -90f));

        //GameObject chasebulletTop = Instantiate(_Bullet6, transform.position + Vector3.up * 0.5f, Quaternion.Euler(new Vector3(0, 0, 90f)));
        //GameObject chasebulletDown = Instantiate(_Bullet6, transform.position + Vector3.down * 0.5f, Quaternion.Euler(new Vector3(0, 0, -90f)));

        Rigidbody2D rigidTop = chasebulletTop.GetComponent<Rigidbody2D>();
        Rigidbody2D rigidBottom = chasebulletBottom.GetComponent<Rigidbody2D>();

        rigidTop.AddForce(Vector2.up * 10, ForceMode2D.Impulse);
        rigidBottom.AddForce(Vector2.down * 10, ForceMode2D.Impulse);

        SoundManager.instance.PlaySoundEffectOneShot("Player_GuideAttack", 0.33f);

        _GuideAttack_Delay_Cur = 0.0f;
    }

    void Bullet_Delay()
    {
        if (_Bullet_Shot_Delay_Cur > 10) return;

        _Bullet_Shot_Delay_Cur += Time.deltaTime;
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
        else // 이동제한 경계선이 아닌 모든 처리
        {
            // 무적상태면 return;
            if (_invincibleTime > 0) return;

            if(collision.gameObject.tag == "PlayerBullet")
            {
                return;
            }

            // 한번에 여러번 맞는 현상 방지
            if (_isHit == true) return;
            _isHit = true;

            if (collision.gameObject.tag == "Enemy")
            {
                // 일반 적과 충돌이면 적 파괴 및 점수 획득
                Enemy_Base enemyinfo = collision.gameObject.GetComponent<Enemy_Base>();
                AddScore(enemyinfo.GetScore());
            }
            else if (collision.gameObject.tag == "Enemy_Boss")
            {
                // 보스와 충돌이면

            }
            else if (collision.gameObject.tag == "EnemyBullet")
            {
                // 적 탄환과 충돌이면 적 탄환 파괴
                gameObject.SetActive(false);
            }
            else if(collision.gameObject.tag == "Item_Shielded")
            {
                // Item_Shielded와 충돌하면 Item_Shielded 파괴
                gameObject.SetActive(false);
            }
            else if(collision.gameObject.tag == "Item")
            {
                collision.gameObject.SetActive(false);
                return;
            }

            SetLife(-1);
            Dead();

            gamemanager.RespawnPlayerInvoke(2.0f);
            gameObject.SetActive(false);
        }
    }

    void Dead()
    {
        // 기체가 파괴되면 능력치 하락 및 특수 공격 초기화
        SetSpeed(-1.25f);
        SetPower(-1);
        SetGuideAttack(false);
        SetChargeAttack(false);
        _chargeTime = 0f;

        SoundManager.instance.PlaySoundEffectOneShot("Enemy_Destroy(Small)", 0.75f);
        EffectManager.instance.SpawnEffect("Effect_Explosion_Orangespark", transform.position, new Vector3(0, 0, 0));
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

            gamemanager.GameOver();
        }

        // 최대 체력은 3
        if (_life >= 3) _life = 3;

        gamemanager.UpdateLifeIcon(GetLife());
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

        gamemanager.UpdateUltIcon(GetUlt());
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

    public int GetUlt()
    {
        return _ult;
    }
}
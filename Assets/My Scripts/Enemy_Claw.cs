using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Claw : MonoBehaviour
{
    [System.Serializable]
    public class Animation
    {
        public Sprite[] sprites;
        public float frameTime = .05f;
    }

    // 능력치
    [SerializeField]
    int _life = 750;
    [SerializeField]
    int _maxlife = 750;
    [SerializeField]
    float _lifepercent = 1f;
    [SerializeField]
    int _score = 5000;

    public int rotations = 5;
    public float movementRange = 1;
    public float movementSpeed = 1;
    public int punchingRange = 17;
    public float punchingPower = 15;

    // 애니메이션 스프라이트
    public Animation sleep;
    public Animation idle;
    public Animation clench;
    public Animation attack;
    public Animation open;
    public Animation charge;
    public Animation chargeattack;

    [SerializeField]
    GameObject _Player = null;
    [SerializeField]
    GameManager _gm;
    [SerializeField]

    // 돌진 공격 관련
    bool _SE_Punch = false;

    Rigidbody2D _rigid;

    float _frameTime;
    int _rotations;
    float _speed;
    [SerializeField]
    public int _state;
    float _y;

    // charge 공격 관련
    [SerializeField]
    float _chargetime = 0;
    bool _chargeeffecton = false;
    [SerializeField]
    float _chargeattacktime = 0;
    bool _chargeattackeffecton = false;

    // sleep 상태 관련
    int repeattime = 2;
    int currepeat = 0;

    


    public int frame;

    public SpriteRenderer spriteRenderer;

    public Boss_Claw_BulletSpawner _bulletspawner;

    void Awake()
    {
        if (spriteRenderer == null) spriteRenderer = GetComponent<SpriteRenderer>();

        _rigid = GetComponent<Rigidbody2D>();

        _Player = GameObject.FindGameObjectWithTag("Player");
    }

    void Start()
    {
        Debug.Log("claw Start");

        GameObject tmp = GameObject.FindGameObjectWithTag("GameManager");
        _gm = tmp.GetComponent<GameManager>();

        _life = _maxlife;

        _bulletspawner = this.GetComponent<Boss_Claw_BulletSpawner>();

        if(_gm != null)
        {
            _gm._isBossSpawn = true;
        }
    }

    void Update()
    {
        _Player = GameObject.FindGameObjectWithTag("Player");
        _lifepercent = (float)_life / (float)_maxlife;
        

        if(_state == -1) // sleep(등장) 상태
        {
            spriteRenderer.sprite = sleep.sprites[frame];

            _frameTime += Time.deltaTime;

            if (_frameTime >= sleep.frameTime)
            {
                _frameTime = 0;

                frame += 1;

                if (frame >= sleep.sprites.Length - 1)
                {
                    if(currepeat >= repeattime)
                    {
                        frame = 0;
                        _state = 4; // 펴지기 상태로 변경

                        // 브금 재생
                        SoundManager.instance.PlayBGM("Stage_01_Boss", 0.33f);
                    }
                    else
                    {
                        currepeat++;
                        frame = 0;
                    }
                }
            }
        }

        if (_state == 0) // Idle 상태
        {
            if (_Player != null) RotationSlerpToTarget(_Player);

            if (_lifepercent <= 1.0f && _lifepercent > 0.66f)
            {
                _bulletspawner.TurnOffAllFire();
                _bulletspawner._CanFireBigBullet = true;
            }
            if (_lifepercent <= 0.66f && _lifepercent > 0.33f)
            {
                _bulletspawner.TurnOffAllFire();
                _bulletspawner._CanFireBigBullet = true;
                _bulletspawner._CanFireArc = true;
            }
            if (_lifepercent <= 0.33f && _lifepercent > 0f)
            {
                _bulletspawner.TurnOffAllFire();
                _bulletspawner._CanFireBigBullet = true;
                _bulletspawner._CanFireArc2Way = true;
            }

            spriteRenderer.sprite = idle.sprites[frame];

            _frameTime += Time.deltaTime;

            if (_frameTime >= idle.frameTime)
            {
                _frameTime = 0;

                frame += 1;

                if (frame >= idle.sprites.Length - 1)
                {
                    frame = 0;
                    _rotations += 1;
                    if (_rotations >= rotations)
                    {
                        _rotations = 0;
                        _state += 1;
                    }
                }
            }
        }

        if (_state == 1) // 오므라들기
        {
            spriteRenderer.sprite = clench.sprites[frame];

            if (_lifepercent <= 1.0f && _lifepercent > 0.66f)
            {
                _bulletspawner.TurnOffAllFire();
            }
            if (_lifepercent <= 0.66f && _lifepercent > 0.33f)
            {
                _bulletspawner.TurnOffAllFire();
            }
            if (_lifepercent <= 0.33f && _lifepercent > 0f)
            {
                _bulletspawner.TurnOffAllFire();
            }

            _frameTime += Time.deltaTime;


            if (_frameTime >= clench.frameTime)
            {
                _frameTime = 0;

                frame += 1;

                if (frame >= clench.sprites.Length - 1)
                {
                    frame = 0;
                    _state += 1;
                }
            }
        }

        if (_state == 2) // 펀치 공격 돌진
        {
            InitRotation();

            if (_lifepercent <= 1.0f && _lifepercent > 0.66f)
            {
                _bulletspawner.TurnOffAllFire();
            }
            if (_lifepercent <= 0.66f && _lifepercent > 0.33f)
            {
                _bulletspawner.TurnOffAllFire();
            }
            if (_lifepercent <= 0.33f && _lifepercent > 0f)
            {
                _bulletspawner.TurnOffAllFire();
            }

            if (_SE_Punch == false)
            {
                SoundManager.instance.PlaySoundEffectOneShot("Boss1_PunchAttack", 0.5f);
                _SE_Punch = true;
            }
            

            if (transform.localPosition.x >= punchingRange)
            {
                _state += 1;
            }
            else
            {
                _speed += punchingPower * Time.deltaTime;

                transform.Translate(Vector3.right * _speed * Time.deltaTime);

                spriteRenderer.sprite = attack.sprites[frame];

                _frameTime += Time.deltaTime;

                if (_frameTime >= clench.frameTime)
                {
                    _frameTime = 0;

                    frame += 1;

                    if (frame >= attack.sprites.Length - 1)
                    {
                        frame = 0;
                    }
                }
            }
        }

        if (_state == 3) // 펀치 공격 되돌아오기
        {
            InitRotation();

            if (_lifepercent <= 1.0f && _lifepercent > 0.66f)
            {
                _bulletspawner.TurnOffAllFire();
            }
            if (_lifepercent <= 0.66f && _lifepercent > 0.33f)
            {
                _bulletspawner.TurnOffAllFire();
            }
            if (_lifepercent <= 0.33f && _lifepercent > 0f)
            {
                _bulletspawner.TurnOffAllFire();
            }

            if (transform.localPosition.x <= 0)
            {
                transform.localPosition = new Vector3(0, transform.localPosition.y, transform.localPosition.z);
                frame = 0;
                _state += 1;
            }
            else
            {
                _speed -= punchingPower * Time.deltaTime;

                transform.Translate(Vector3.left * (_speed + 1) * Time.deltaTime);

                spriteRenderer.sprite = attack.sprites[frame];

                _frameTime += Time.deltaTime;

                if (_frameTime >= attack.frameTime)
                {
                    _frameTime = 0;

                    frame += 1;

                    if (frame >= attack.sprites.Length - 1)
                    {
                        frame = 0;
                    }
                }
            }

            _SE_Punch = false;
        }

        if (_state == 4) // 다시 펴지기 상태
        {
            if (_lifepercent <= 1.0f && _lifepercent > 0.66f)
            {
                _bulletspawner.TurnOffAllFire();
            }
            if (_lifepercent <= 0.66f && _lifepercent > 0.33f)
            {
                _bulletspawner.TurnOffAllFire();
            }
            if (_lifepercent <= 0.33f && _lifepercent > 0f)
            {
                _bulletspawner.TurnOffAllFire();
            }


            spriteRenderer.sprite = open.sprites[frame];

            _frameTime += Time.deltaTime;

            if (_frameTime >= open.frameTime)
            {
                _frameTime = 0;

                frame += 1;

                if (frame >= open.sprites.Length - 1)
                {
                    frame = 0;
                    _state += 1;
                }
            }
        }

        if (_state == 5) // charge 상태
        {
            if (_lifepercent <= 1.0f && _lifepercent > 0.66f)
            {
                _bulletspawner.TurnOffAllFire();
            }
            if (_lifepercent <= 0.66f && _lifepercent > 0.33f)
            {
                _bulletspawner.TurnOffAllFire();
                _bulletspawner._CanFireMultiRandomshotToPlayer = true;
            }
            if (_lifepercent <= 0.33f && _lifepercent > 0f)
            {
                _bulletspawner.TurnOffAllFire();
                _bulletspawner._CanFireMultiRandomshotToPlayer = true;
            }

            if (_chargeeffecton == false)
            {
                _chargeeffecton = true;
                // Enemy_Claw의 원본 x 스케일이 -1이라서 x 스케일 값에 -1을 넣어줘야 함
                EffectManager.instance.SpawnEffect("Effect_Boss_Laser_Charge", transform.position, new Vector3(0, 0, 0), new Vector3(-1, 1, 1), this.transform);
                SoundManager.instance.PlaySoundEffectOneShot("Boss_Weapon_Charging", 0.33f);
            }

            _chargetime += Time.deltaTime;
            

            if (_Player != null) RotationSlerpToTarget(_Player);

            spriteRenderer.sprite = idle.sprites[frame];

            _frameTime += Time.deltaTime;

            if (_frameTime >= idle.frameTime)
            {
                _frameTime = 0;

                frame += 1;

                if (frame >= idle.sprites.Length - 1)
                {
                    frame = 0;

                    if (_chargetime >= 3.0f)
                    {
                        _chargetime = 0;
                        _chargeeffecton = false;
                        _rotations = 0;
                        _state += 1;
                    }
                }
            }
        }

        if (_state == 6) // ChargeAttack 상태
        {
            if (_lifepercent <= 1.0f && _lifepercent > 0.66f)
            {
                _bulletspawner.TurnOffAllFire();
            }
            if (_lifepercent <= 0.66f && _lifepercent > 0.33f)
            {
                _bulletspawner.TurnOffAllFire();
                _bulletspawner._CanFireCircle = true;
            }
            if (_lifepercent <= 0.33f && _lifepercent > 0f)
            {
                _bulletspawner.TurnOffAllFire();
                _bulletspawner._CanFireCircle = true;
            }

            if (_Player != null) RotationSlerpToTarget(_Player);

            if (_chargeattackeffecton == false)
            {
                _chargeattackeffecton = true;

                // Enemy_Claw가 좀 특수한 상태라서 180도를 더해줘야 함
                // 참고로 이펙트의 z축을 회전시켜야 됨
                // https://dydvn.tistory.com/28
                Vector3 _myangle = transform.rotation.eulerAngles + new Vector3(0, 0, 180); // Enemy_Claw의 정확한 앞 방향
                                                                                           // 회전값(월드좌표)  // 스케일 값                                 
                EffectManager.instance.SpawnEffect("Effect_Boss_Laser", transform.position, _myangle, new Vector3(1.15f, 1, 1), this.transform);
                SoundManager.instance.PlaySoundEffectOneShot("Boss_Weapon_ChargeShot1", 0.5f);
            }

            _chargeattacktime += Time.deltaTime;

            spriteRenderer.sprite = chargeattack.sprites[frame];

            _frameTime += Time.deltaTime;

            if (_frameTime >= chargeattack.frameTime)
            {
                _frameTime = 0;

                frame += 1;

                if (frame >= chargeattack.sprites.Length - 1)
                {
                    frame = 0;

                    if(_chargeattacktime >= 6.0f)
                    {
                        _chargeattacktime = 0;
                        _chargeattackeffecton = false;
                        _rotations = 0;
                        _state = 0;
                    }
                }
            }
        }

        if (_state == 0 || _state == 1 || _state == 4)
        {
            transform.localPosition = new Vector3(transform.localPosition.x, Mathf.Sin(_y) * movementRange, transform.localPosition.z);

            _y += (movementSpeed * Time.deltaTime) * 1.5f;
        }
    }

    void InitRotation()
    {
        transform.rotation = Quaternion.Euler(new Vector3(0, 0, 0));
    }

    void RotationSlerpToTarget(GameObject _target)
    {
        // 방향 벡터
        Vector3 _dir = _Player.transform.position - transform.position;
        // 플레이어를 바라보는 방향
        float _angle = Mathf.Atan2(-_dir.y, -_dir.x) * Mathf.Rad2Deg;
       // Debug.Log(_angle + 180f);

        // 부드러운 회전
        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.AngleAxis(_angle, Vector3.forward), Time.deltaTime * 1f);
        // 즉각 회전
        // _rigid.rotation = _angle;
    }

    void OnHit(int damage) // 피격 판정
    {
        _life -= damage;

        if (_life <= 0)
        {
            _Player = GameObject.FindGameObjectWithTag("Player");
            PlayerController playerinfo = _Player.GetComponent<PlayerController>();
            playerinfo.AddScore(_score);

            SoundManager.instance.PlaySoundEffectOneShot("Boss01_Death", 1.0f);
            SoundManager.instance.PlaySoundEffectOneShot("Boss_Defeat", 1.0f);
            EffectManager.instance.SpawnEffect("Effect_Explosion_Cyberspark", transform.position, Vector3.zero, new Vector3(5.0f, 5.0f, 5.0f));
            EffectManager.instance.SpawnEffect("Effect_Explosion_Redspark", transform.position, Vector3.zero, new Vector3(5.0f, 5.0f, 5.0f));

            if (_gm != null)
            {
                _gm.SetEnemyCnt(1);
                _gm._CanBossSpawn = false;
                _gm._CanSpawnEnemy = true;
                _gm._WarningSound = false;
                _gm._isBossSpawn = false;
            }

            // 죽으면 스테이지 클리어 BGM이 재생 (모드에 따라서 다르게 할 필요 있음)
            if(_gm._gamemode == Define.GameMode.Campaign)
            {
                SoundManager.instance.PlayBGM("Stage_Clear", 0.75f, false);

                gameObject.SetActive(false);

                // 게임 완료 UI 띄우기 (Stage_Clear_Canvas)
                // 게임 일시 정지
            }
            else if(_gm._gamemode == Define.GameMode.Infinite)
            {
                SoundManager.instance.PlayBGM("Stage_01_2", 0.75f, true);

                Init();
                transform.parent.gameObject.SetActive(false);                
            }
            
        }
    }

    void Init()
    {
        Debug.Log("Claw Init");

        _life = _maxlife;
        _lifepercent = 1f;
        _state = -1;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {

        }
        else if (collision.gameObject.tag == "PlayerBullet") // 플레이어 기체 공격에 피격
        {
            // 닿은 플레이어의 탄환 정보(공격력)를 가져온다
            Bullet_Base bullet = collision.gameObject.GetComponent<Bullet_Base>();
            OnHit(bullet._damage);

            if (collision.gameObject.name == "Player Bullet Default(Clone)")
            {
                EffectManager.instance.SpawnEffect("Effect_PlayerBullet_Hit03_2", transform.position, Vector3.one);
            }
            else if (collision.gameObject.name == "Player Bullet Waveform(Clone)")
            {
                EffectManager.instance.SpawnEffect("Effect_PlayerBullet_Hit03_2", transform.position, Vector3.one);
            }

            // 닿은 플레이어의 탄환 제거
            collision.gameObject.SetActive(false);
        }
        else if (collision.gameObject.tag == "PlayerBullet_Charged")
        {
            // 닿은 플레이어의 탄환 정보(공격력)를 가져온다
            Bullet_Base bullet = collision.gameObject.GetComponent<Bullet_Base>();
            OnHit(bullet._damage);
        }
    }

    void SimpleMoveLeft() { }
    void SimpleMoveUp() { }
    void SimpleMoveDown() { }
}

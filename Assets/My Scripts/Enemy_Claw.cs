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

    public int rotations = 5;
    public float movementRange = 1;
    public float movementSpeed = 1;
    public int punchingRange = 17;
    public float punchingPower = 15;

    public Animation idle;
    public Animation clench;
    public Animation attack;
    public Animation open;
    public Animation charge;
    public Animation chargeattack;

    [SerializeField]
    GameObject _Player = null;

    Rigidbody2D _rigid;

    float _frameTime;
    int _rotations;
    float _speed;
    [SerializeField]
    int _state;
    float _y;
    [SerializeField]
    float _chargetime = 0;
    bool _chargeeffecton = false;
    [SerializeField]
    float _chargeattacktime = 0;
    bool _chargeattackeffecton = false;

    public int frame;

    public SpriteRenderer spriteRenderer;

    void Awake()
    {
        if (spriteRenderer == null) spriteRenderer = GetComponent<SpriteRenderer>();

        _rigid = GetComponent<Rigidbody2D>();

        _Player = GameObject.FindGameObjectWithTag("Player");
    }

    void Update()
    {
        _Player = GameObject.FindGameObjectWithTag("Player");

        if (_state == 0) // Idle ����
        {
            if(_Player != null) RotationSlerpToTarget(_Player);

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

        if (_state == 1) // ���Ƕ���
        {
            InitRotation();

            spriteRenderer.sprite = clench.sprites[frame];

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

        if (_state == 2) // ��ġ ���� ����
        {
            InitRotation();

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

        if (_state == 3) // ��ġ ���� �ǵ��ƿ���
        {
            InitRotation();

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
        }

        if (_state == 4) // �ٽ� ������ ����
        {
            InitRotation();

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

        if (_state == 5) // charge ����
        {
            if (_chargeeffecton == false)
            {
                _chargeeffecton = true;
                // Enemy_Claw�� ���� x �������� -1�̶� x ������ ���� -1�� �־���� ��
                EffectManager.instance.SpawnEffect("Effect_Boss_Laser_Charge", transform.position, new Vector3(0, 0, 0), new Vector3(-1, 1, 1), this.transform);
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

                    if (_chargetime >= 4.0f)
                    {
                        _chargetime = 0;
                        _chargeeffecton = false;
                        _rotations = 0;
                        _state += 1;
                    }
                }
            }
        }

        if (_state == 6) // ChargeAttack ����
        {
            if (_Player != null) RotationSlerpToTarget(_Player);

            if (_chargeattackeffecton == false)
            {
                _chargeattackeffecton = true;

                Vector3 _dir = _Player.transform.position - transform.position;
                // Enemy_Claw�� �� Ư���� ���¶� 180���� ������� ��
                // ����� ����Ʈ�� z���� ȸ�����Ѿ� ��
                float _angle = Mathf.Atan2(-_dir.y, -_dir.x) * Mathf.Rad2Deg + 180f;

                                                                                            // ȸ����                 // ������ ��                                 
                EffectManager.instance.SpawnEffect("Effect_Boss_Laser", transform.position, new Vector3(0, 0, _angle), new Vector3(1, 1, 1), this.transform);
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

        //if (_state == 0 || _state == 1 || _state == 4)
        //{
        //    transform.localPosition = new Vector3(transform.localPosition.x, Mathf.Sin(_y) * movementRange, transform.localPosition.z);

        //    _y += movementSpeed * Time.deltaTime;
        //}
    }

    void InitRotation()
    {
        transform.rotation = Quaternion.Euler(new Vector3(0, 0, 0));
    }

    void RotationSlerpToTarget(GameObject _target)
    {
        // ���� ����
        Vector3 _dir = _Player.transform.position - transform.position;
        // �÷��̾ �ٶ󺸴� ����
        float _angle = Mathf.Atan2(-_dir.y, -_dir.x) * Mathf.Rad2Deg;


        // �ε巯�� ȸ��
        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.AngleAxis(_angle, Vector3.forward), Time.deltaTime * 1f);
        // �ﰢ ȸ��
        // _rigid.rotation = _angle;
    }
}

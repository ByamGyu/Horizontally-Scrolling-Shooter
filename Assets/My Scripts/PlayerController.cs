using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    // ���� �Ŵ��� ����
    public GameManager manager;

    // �÷��̾� �ɷ�ġ
    [SerializeField]
    float _speed = 5.0f; // �̵� �ӵ� ����
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

    // �÷��̾� �̵� ����
    [SerializeField]
    bool _isTouchTop = false;
    [SerializeField]
    bool _isTouchBottom = false;
    [SerializeField]
    bool _isTouchRight = false;
    [SerializeField]
    bool _isTouchLeft = false;

    // ������Ʈ ���� ����
    [SerializeField]
    public GameObject _Bullet1; // �ʿ��� ������Ʈ�� ���� �� �ִ�. (ī�޶�, ��ü ���)
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
        if (_invincibleTime <= 0) // �����ð��� ������
        {
            _invincibleTime = 0.0f;
        }
        else // �����ð��� ����������
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
        // Input.GetAxisRaw()�� ���� ���� �����ϴ� �Լ�
        float _height = Input.GetAxisRaw("Horizontal"); // Ű���� ���� �̵� �Է� �ޱ�
        float _vertical = Input.GetAxisRaw("Vertical"); // Ű���� ���� �̵� �Է� �ޱ�

        // ��輱 �̵� �����ϱ�
        if ((_isTouchTop == true && _vertical == 1) || (_isTouchBottom == true && _vertical == -1)) _vertical = 0;
        if ((_isTouchRight == true && _height == 1) || (_isTouchLeft == true && _height == -1)) _height = 0;

        Vector3 CurPos = transform.position; // ���� ��ġ
        Vector3 NextPos = new Vector3(_height, _vertical, 0) * _speed * Time.deltaTime; // �̵��ϴ� �Ÿ�

        transform.position = CurPos + NextPos; // ���� ��ġ
    }

    void Fire_Bullet()
    {
        // '['�� ������ �߻�
        if (!Input.GetKey(KeyCode.LeftBracket)) return;

        // �Ѿ� ���� ������ �ð� �Ǻ�
        if (_Bullet_Shot_Delay_Cur < _Bullet_Shot_Delay_Max) return;

        switch(_power) // �Ϲ� źȯ
        {
            case 1: // �Ŀ����� 1
                // ������(_Bullet1)�� ������Ʈ�� ����, ���� ��ġ, ���� ����
                GameObject bulletMid1 = Instantiate(_Bullet1, transform.position + Vector3.right * 0.5f, transform.rotation);

                // źȯ�� ���� ���� �����̰� �Ѵ�.
                Rigidbody2D rigid1 = bulletMid1.GetComponent<Rigidbody2D>();

                rigid1.AddForce(Vector2.right * 10, ForceMode2D.Impulse);

                // �Ѿ� ���� ������ �ð� �ʱ�ȭ
                _Bullet_Shot_Delay_Cur = 0.0f;
                break;
            case 2: // �Ŀ����� 2
                GameObject bulletTop2 = Instantiate(_Bullet1, transform.position + Vector3.up * 0.15f + Vector3.right * 0.5f, transform.rotation);
                GameObject bulletDown2 = Instantiate(_Bullet1, transform.position + Vector3.down * 0.15f + Vector3.right * 0.5f, transform.rotation);

                Rigidbody2D rigidTop2 = bulletTop2.GetComponent<Rigidbody2D>();
                Rigidbody2D rigidBottom2 = bulletDown2.GetComponent<Rigidbody2D>();

                rigidTop2.AddForce(Vector2.right * 10, ForceMode2D.Impulse);
                rigidBottom2.AddForce(Vector2.right * 10, ForceMode2D.Impulse);

                _Bullet_Shot_Delay_Cur = 0.0f;
                break;
            case 3: // �Ŀ����� 3
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
            case 4: // �Ŀ����� 4
                GameObject bulletMid4 = Instantiate(_Bullet5, transform.position + Vector3.right * 0.5f, transform.rotation);
                
                Rigidbody2D rigidMid1 = bulletMid4.GetComponent<Rigidbody2D>();

                rigidMid1.AddForce(Vector2.right * 10, ForceMode2D.Impulse);

                _Bullet_Shot_Delay_Cur = 0.0f;
                break;
            case 5: // �׽�Ʈ �뵵
                GameObject test = Instantiate(_Bullet6, transform.position + Vector3.right * 0.5f, transform.rotation);
                
                _Bullet_Shot_Delay_Cur = 0.0f;
                break;
        }
    }

    void Fire_ChargeAttack()
    {
        // ������ �ð��� ����ؼ� �������� Ű��� �������
        // ������ �ð��� ����ؼ� �������� ����
    }

    void Fire_GuideAttack()
    {
        // ����ź ������ �ð� �Ǻ�
        if (_Bullet_Shot_Delay_Cur < _Bullet_Shot_Delay_Max) return;

        // ����ź ������ �����ϴ� �������

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
        if (collision.gameObject.tag == "Border") // ��輱 �̵� ����
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
            // �������¸� return;
            if (_invincibleTime > 0) return;

            // �ѹ��� ������ �´� ���� ����
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

        // �ִ� ü���� 3
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
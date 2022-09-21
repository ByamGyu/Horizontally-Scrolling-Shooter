using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    // �÷��̾� �ɷ�ġ
    [SerializeField]
    float _speed = 5.0f; // �̵� �ӵ� ����
    [SerializeField]
    float _Bullet_Shot_Delay_Cur = 0.0f;
    [SerializeField]
    float _Bullet_Shot_Delay_Max = 0.2f;
    [SerializeField]
    int power = 1;

    // �÷��̾� �̵� ����
    [SerializeField]
    bool _isTouchTop = false;
    [SerializeField]
    bool _isTouchBottom = false;
    [SerializeField]
    bool _isTouchRight = false;
    [SerializeField]
    bool _isTouchLeft = false;

    // �÷��̾� �ִϸ��̼�?
    [SerializeField]
    Animator _anim;

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
        
    }
    void Update()
    {
        Move();
        Fire_Bullet();
        Bullet_Delay();
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

        switch(power) // �Ϲ� źȯ
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
        }

        
    }

    void Bullet_Delay()
    {
        if (_Bullet_Shot_Delay_Cur > 10) return;

        _Bullet_Shot_Delay_Cur += Time.deltaTime;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Border")
        {
            switch(collision.gameObject.name)
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
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField]
    float _speed = 5.0f; // �̵� �ӵ� ����
    [SerializeField]
    bool _isTouchTop = false;
    [SerializeField]
    bool _isTouchBottom = false;
    [SerializeField]
    bool _isTouchRight = false;
    [SerializeField]
    bool _isTouchLeft = false;
    [SerializeField]
    Animator anim;
    [SerializeField]
    public GameObject _obj; // �ʿ��� ������Ʈ�� ���� �� �ִ�. (ī�޶�, ��ü ���)

    void Start()
    {
        
    }
    void Update()
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
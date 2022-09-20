using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField]
    float _speed = 5.0f; // 이동 속도 변수
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
    public GameObject _obj; // 필요한 오브젝트를 붙일 수 있다. (카메라, 물체 등등)

    void Start()
    {
        
    }
    void Update()
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
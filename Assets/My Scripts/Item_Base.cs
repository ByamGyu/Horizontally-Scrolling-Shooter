using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item_Base : MonoBehaviour
{
    Rigidbody2D _rigid;
    [SerializeField]
    protected float _speed = 1.0f;
    [SerializeField]
    protected int _score = 50;

    void Start()
    {
        _rigid = GetComponent<Rigidbody2D>();
    }

    void FixedUpdate()
    {
        SimpleMoveLeft();
    }

    void SimpleMoveLeft()
    {
        //_rigid.velocity = Vector2.left * _speed;
        transform.position += new Vector3(0.017f * _speed * (-1), 0, 0);
    }
}

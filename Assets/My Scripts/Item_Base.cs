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

    private void Start()
    {
        _rigid = GetComponent<Rigidbody2D>();

        SimpleMoveLeft();
    }

    void SimpleMoveLeft()
    {
        _rigid.velocity = Vector2.left * _speed;
    }
}

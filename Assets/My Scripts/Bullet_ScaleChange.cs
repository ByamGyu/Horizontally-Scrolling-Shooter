using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet_ScaleChange : MonoBehaviour
{
    private float _y;
    private float _size;

    void Update()
    {
        _size = (Mathf.Sin(_y) * 0.25f) + 1;

        ChangeScaleSin(_size);

        _y += (1 * Time.deltaTime);
    }

    void ChangeScaleSin(float _scale)
    {
        transform.localScale = new Vector3(_scale, _scale, _scale);
    }
}

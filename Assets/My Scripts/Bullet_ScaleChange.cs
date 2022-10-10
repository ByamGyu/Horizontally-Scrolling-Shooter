using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet_ScaleChange : MonoBehaviour
{
    public float _y;
    public float _size;

    private void Update()
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

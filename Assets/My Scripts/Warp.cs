using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Warp : MonoBehaviour
{
    public Vector3 _Origin_Scale;
    public Vector3 _Change_Scale;
    public float _Time;
    public float _EndTime;
    public bool _isSpawnEnd;

    public ObjectManager objectManager;


    private void Awake()
    {
        Init();
    }

    void Start()
    {
        SoundManager.instance.StopBGM();
        SoundManager.instance.PlaySoundEffectOneShot("Teleport", 0.75f);
    }

    void Update()
    {
        _Time += Time.deltaTime;

        if(_Time >= 0 && _Time < 0.1f)
        {
            transform.localScale = _Change_Scale;
        }
        else if(_Time >= 0.1f && _Time < 0.2f)
        {
            transform.localScale = _Origin_Scale;
        }
        else if (_Time >= 0.2f && _Time < 0.3f)
        {
            transform.localScale = _Change_Scale;
        }
        else if (_Time >= 0.3f && _Time < 0.4f)
        {
            transform.localScale = _Origin_Scale;
        }
        else if (_Time >= 0.4f && _Time < 0.5f)
        {
            transform.localScale = _Change_Scale;
        }
        else if (_Time >= 0.5f && _Time < 0.6f)
        {
            transform.localScale = _Origin_Scale;
        }
        else if (_Time >= 0.6f && _Time < 0.7f)
        {
            transform.localScale = _Change_Scale;
        }
        else if (_Time >= 0.7f && _Time < 0.8f)
        {
            transform.localScale = _Origin_Scale;
        }
        else if (_Time >= 0.8f && _Time < 0.9f)
        {
            transform.localScale = _Change_Scale;
        }
        else if (_Time >= 0.9f && _Time < 1f)
        {
            transform.localScale = _Change_Scale;
        }

        if(_Time >= _EndTime && _isSpawnEnd == false)
        {
            GameObject go = objectManager.MakeObj("Enemy_Claw");
            go.transform.position = transform.position;
            
            _isSpawnEnd = true;
            gameObject.SetActive(false);
        }
    }

    void Init()
    {
        _Time = 0;
        _isSpawnEnd = false;
    }
}

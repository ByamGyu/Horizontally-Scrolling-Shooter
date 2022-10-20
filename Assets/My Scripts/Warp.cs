using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Warp : MonoBehaviour
{
    [SerializeField]
    Vector3 _Origin_Scale;
    [SerializeField]
    Vector3 _Change_Scale;
    [SerializeField]
    float _Time;
    [SerializeField]
    float _EndTime;
    [SerializeField]
    bool _isSoundPlay = false;


    private void Awake()
    {
        Init();
    }

    void Start()
    {
        
    }

    void Update()
    {
        _Time += Time.deltaTime;

        if(_isSoundPlay == false)
        {
            _isSoundPlay = true;
            SoundManager.instance.StopBGM();
            SoundManager.instance.PlaySoundEffectOneShot("Teleport", 0.75f);
        }

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

        if(_Time >= _EndTime)
        {
            GameObject go = ObjectManager.instance.MakeObj("Enemy_Claw");
            go.transform.position = transform.position;

            Init();
            gameObject.SetActive(false);
        }
    }

    void Init()
    {
        _Time = 0;
        _isSoundPlay = false;
    }
}

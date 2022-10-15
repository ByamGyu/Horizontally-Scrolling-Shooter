using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectManager : MonoBehaviour
{
    // 스프라이트 렌더러를 이용한 이펙트를 제어하는 매니저
    // 일반 이펙트(파티클 시스템?)도 해야할듯

    public static EffectManager instance;

    [SerializeField]
    string[] _EffectNames;
    [SerializeField]
    GameObject[] _EffectLists;

    // return 전용 변수
    GameObject _effecttmp;

    public Dictionary<string, GameObject> _DicEffectStorage = new Dictionary<string, GameObject>();


    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this);
        }
        else
        {
            Debug.Log("이펙트매니저가 2개 이상 존재함!");
            Destroy(gameObject);
        }

        InitDictionary();
    }

    void InitDictionary()
    {
        // 꼭 이 함수 실행시켜!

        if (_DicEffectStorage.Count != 0) _DicEffectStorage.Clear();

        for(int i = 0; i < _EffectNames.Length; i++)
        {
            string tmp_name = _EffectNames[i];
            GameObject tmp_go = _EffectLists[i];

            _DicEffectStorage.Add(tmp_name, tmp_go);
        }
    }

    // pos: 위치, transform: 회전값, parent: 이펙트를 어느 오브젝트 자식으로 둘지
    public void SpawnEffect(string _effectname, Vector3 _pos, Vector3 _transform, Transform _parent = null)
    {
        if(_DicEffectStorage.ContainsKey(_effectname))
        {
            if (_parent != null) // 부모 지정할 객체의 transform을 _parent에 넣어주면 됨
            {
                GameObject go = Instantiate(_DicEffectStorage[_effectname], _pos, Quaternion.Euler(_transform.x, _transform.y, _transform.z));
                go.transform.SetParent(_parent);
            }
            else
            {
                Instantiate(_DicEffectStorage[_effectname], _pos, Quaternion.Euler(_transform.x, _transform.y, _transform.z));
            }
        }
        else
        {
            Debug.Log("Effect: " + _effectname + " is not exist");
        }
    }

    public void SpawnEffect(string _effectname, Vector3 _pos, Vector3 _transform, Vector3 _scale, Transform _parent = null)
    {
        if (_DicEffectStorage.ContainsKey(_effectname))
        {
            if (_parent != null) // 부모 지정할 객체의 transform을 _parent에 넣어주면 됨
            {
                GameObject go = Instantiate(_DicEffectStorage[_effectname], _pos, Quaternion.Euler(_transform.x, _transform.y, _transform.z));
                go.transform.localScale = new Vector3(_scale.x, _scale.y, _scale.z);
                go.transform.SetParent(_parent);
            }
            else
            {
                Instantiate(_DicEffectStorage[_effectname], _pos, Quaternion.Euler(_transform.x, _transform.y, _transform.z));
            }
        }
        else
        {
            Debug.Log("Effect: " + _effectname + " is not exist");
        }
    }

    // 생성된 이펙트를 반환해주는 함수
    public GameObject GetEffect(string _effectname, Vector3 _pos, Vector3 _transform, Transform _parent = null)
    {
        if (_DicEffectStorage.ContainsKey(_effectname))
        {
            if (_parent != null) // 부모 지정할 객체의 transform을 _parent에 넣어주면 됨
            {
                _effecttmp = Instantiate(_DicEffectStorage[_effectname], _pos, Quaternion.Euler(_transform.x, _transform.y, _transform.z));
                _effecttmp.transform.SetParent(_parent);
            }
            else
            {
                Instantiate(_DicEffectStorage[_effectname], _pos, Quaternion.Euler(_transform.x, _transform.y, _transform.z));
            }
        }
        else
        {
            Debug.Log("Effect: " + _effectname + " is not exist");
        }

        return _effecttmp;
    }

    public GameObject GetEffect(string _effectname, Vector3 _pos, Vector3 _transform, Vector3 _scale, Transform _parent = null)
    {
        if (_DicEffectStorage.ContainsKey(_effectname))
        {
            if (_parent != null) // 부모 지정할 객체의 transform을 _parent에 넣어주면 됨
            {
                _effecttmp = Instantiate(_DicEffectStorage[_effectname], _pos, Quaternion.Euler(_transform.x, _transform.y, _transform.z));
                _effecttmp.transform.localScale = new Vector3(_scale.x, _scale.y, _scale.z);
                _effecttmp.transform.SetParent(_parent);
            }
            else
            {
                Instantiate(_DicEffectStorage[_effectname], _pos, Quaternion.Euler(_transform.x, _transform.y, _transform.z));
            }
        }
        else
        {
            Debug.Log("Effect: " + _effectname + " is not exist");
        }

        return _effecttmp;
    }
}

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

    public Dictionary<string, GameObject> _DicEffectStorage = new Dictionary<string, GameObject>();


    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this);
        }
        else Destroy(gameObject);

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
    public void PlayEffect(string _effectname, Vector3 _pos, Vector3 _transform, Transform _parent = null)
    {
        if(_DicEffectStorage.ContainsKey(_effectname))
        {
            GameObject go = new GameObject("EffectObject: " + _effectname);
            go.AddComponent<SpriteRenderer>();
            SpriteRenderer renderer = GetComponent<SpriteRenderer>();
            

            go = Instantiate(_DicEffectStorage[_effectname], _pos, Quaternion.Euler(_transform.x, _transform.y, _transform.z));

            if (_parent != null)
            {                
                go.transform.SetParent(_parent);
            }            
        }
        else
        {
            Debug.Log("Effect: " + _effectname + " is not exist");
        }
    }

    public void DestroyThis()
    {
        Destroy(gameObject);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectManager : MonoBehaviour
{
    // ��������Ʈ �������� �̿��� ����Ʈ�� �����ϴ� �Ŵ���
    // �Ϲ� ����Ʈ(��ƼŬ �ý���?)�� �ؾ��ҵ�

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
        // �� �� �Լ� �������!

        if (_DicEffectStorage.Count != 0) _DicEffectStorage.Clear();

        for(int i = 0; i < _EffectNames.Length; i++)
        {
            string tmp_name = _EffectNames[i];
            GameObject tmp_go = _EffectLists[i];

            _DicEffectStorage.Add(tmp_name, tmp_go);
        }
    }

    // pos: ��ġ, transform: ȸ����, parent: ����Ʈ�� ��� ������Ʈ �ڽ����� ����
    public void SpawnEffect(string _effectname, Vector3 _pos, Vector3 _transform, Transform _parent = null)
    {
        if(_DicEffectStorage.ContainsKey(_effectname))
        {
            if (_parent != null) // �θ� ������ ��ü�� transform�� _parent�� �־��ָ� ��
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


    GameObject go;
    public GameObject GetEffect(string _effectname, Vector3 _pos, Vector3 _transform, Transform _parent = null)
    {
        

        if (_DicEffectStorage.ContainsKey(_effectname))
        {
            if (_parent != null) // �θ� ������ ��ü�� transform�� _parent�� �־��ָ� ��
            {
                go = Instantiate(_DicEffectStorage[_effectname], _pos, Quaternion.Euler(_transform.x, _transform.y, _transform.z));
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

        return go;
    }

    public void SpawnEffect(string _effectname, Vector3 _pos, Vector3 _transform, Vector3 _scale, Transform _parent = null)
    {
        if (_DicEffectStorage.ContainsKey(_effectname))
        {
            if (_parent != null) // �θ� ������ ��ü�� transform�� _parent�� �־��ָ� ��
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
}

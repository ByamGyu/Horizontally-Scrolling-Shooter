using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectSpawnTest2 : MonoBehaviour
{
    [SerializeField]
    GameObject _Player = null;

    // Start is called before the first frame update
    void Start()
    {
        _Player = GameObject.FindGameObjectWithTag("Player");
        Vector3 _dir = _Player.transform.position - transform.position;
        Debug.Log("transform.position.x : " + _dir.x);
        Debug.Log("transform.position.y : " + _dir.y);
        Debug.Log("transform.position.z : " + _dir.z);

        float _angle = Mathf.Atan2(-_dir.y, -_dir.x) * Mathf.Rad2Deg + 180f;

        EffectManager.instance.SpawnEffect("Effect_Boss_Laser", transform.position, new Vector3(0, 0, _angle), new Vector3(1, 1, 1), transform);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

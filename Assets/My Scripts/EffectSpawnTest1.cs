using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectSpawnTest1 : MonoBehaviour
{


    // Start is called before the first frame update
    void Start()
    {
        EffectManager.instance.SpawnEffect("Effect_Boss_Laser", transform.position, new Vector3(0, 0, 45), new Vector3(1, 1, 1), transform);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

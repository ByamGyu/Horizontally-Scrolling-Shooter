using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticlSystem_Destroy : MonoBehaviour
{
    private ParticleSystem _ps;

    void Start()
    {
        _ps = GetComponent<ParticleSystem>();
    }

    private void Update()
    {
        if(_ps != null)
        {
            if(_ps.IsAlive() == false)
            {
                Destroy(gameObject);
            }
        }
    }
}

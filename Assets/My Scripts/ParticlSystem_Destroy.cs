using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticlSystem_Destroy : MonoBehaviour
{
    void Start()
    {
        if(GetComponent<ParticleSystem>() == true) Destroy(gameObject, GetComponent<ParticleSystem>().duration);
    }
}

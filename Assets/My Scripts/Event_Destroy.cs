using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Event_Destroy : MonoBehaviour
{
    void DestroyThis()
    {
        Destroy(gameObject);
    }
}

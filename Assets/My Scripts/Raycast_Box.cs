using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Raycast_Box : MonoBehaviour
{
    [SerializeField]
    float _distance = 20.0f;
    [SerializeField]
    Color _color = Color.red;
    [SerializeField]
    public bool use = false;


    void FixedUpdate()
    {
        if(use == true)
        {
            RaycastOn();
        }
    }

    string RaycastOn()
    {
        string _targettag = "";

        Vector2 look = transform.TransformDirection(Vector2.left);

        Debug.DrawRay(transform.position, look * _distance, _color);

        RaycastHit2D hitInfo = Physics2D.Raycast(transform.position, look * _distance);
        if(hitInfo.collider != null)
        {
            if (hitInfo.collider.tag == "Player")
            {
                Debug.Log("Boss RayCast: "  + hitInfo.collider.name);
                _targettag = hitInfo.collider.tag;

                EffectManager.instance.SpawnEffect("Effect_Boss_Laser_Hit", hitInfo.collider.transform.position, new Vector3(0, 0, 0));
            }
        }

        return _targettag;
    }
}
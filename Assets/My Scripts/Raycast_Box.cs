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

    [SerializeReference]
    public GameObject _Player = null;


    void FixedUpdate()
    {
        _Player = GameObject.FindGameObjectWithTag("Player");

        if (use == true)
        {
            RaycastOn();
        }
    }

    void RaycastOn()
    {
        float _distance = 20.0f;

        Vector2 startPos = transform.position + new Vector3(-1, 0, 0);
        // 레이캐스트의 방향은 무조건 앞으로!
        Vector2 _dir = Vector2.left;

        float _angle = Mathf.Atan2(-_dir.y, -_dir.x) * Mathf.Rad2Deg;

        

        RaycastHit2D hitInfo = Physics2D.BoxCast(
            startPos, 
            new Vector2(1, 1), 
            0f,
            _dir,
            _distance
            );

        Debug.DrawRay(startPos, _dir * _distance, _color);

        if (hitInfo.collider != null)
        {
            if (hitInfo.collider.tag == "Player")
            {
                Debug.Log("Boss RayCast: "  + hitInfo.collider.name);

                EffectManager.instance.SpawnEffect("Effect_Boss_Laser_Hit", hitInfo.collider.transform.position, new Vector3(0, 0, 0));
            }
            //if(hitInfo.collider.tag != "Player")
            //{
            //    Debug.Log("wtf: " + hitInfo.collider.name);
            //}
        }
    }
}
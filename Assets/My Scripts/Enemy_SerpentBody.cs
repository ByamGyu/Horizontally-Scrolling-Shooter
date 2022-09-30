using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_SerpentBody : MonoBehaviour
{
    public Transform head;
    public float distanceToHead;

    void Update()
    {
        UpdateBodyPart();
    }

    public void UpdateBodyPart()
    {
        // 몸통(자신)의 위치에서 머리 방향을 구함
        var direction = head.position - transform.position;
        // 머리로 회전하는 방향을 구함
        var angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        // 회전을 적용함
        transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);

        // 몸통에서 머리 사이의 거리
        var distance = Vector3.Distance(head.position, transform.position);

        // 
        if (distance > distanceToHead)
        {
            var position = distance - distanceToHead;

            var x = Mathf.Cos(angle * Mathf.Deg2Rad) * position;
            var y = Mathf.Sin(angle * Mathf.Deg2Rad) * position;

            transform.position = new Vector3(x, y, 0f) + transform.position;
        }
    }
}

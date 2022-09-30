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
        // ����(�ڽ�)�� ��ġ���� �Ӹ� ������ ����
        var direction = head.position - transform.position;
        // �Ӹ��� ȸ���ϴ� ������ ����
        var angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        // ȸ���� ������
        transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);

        // ���뿡�� �Ӹ� ������ �Ÿ�
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

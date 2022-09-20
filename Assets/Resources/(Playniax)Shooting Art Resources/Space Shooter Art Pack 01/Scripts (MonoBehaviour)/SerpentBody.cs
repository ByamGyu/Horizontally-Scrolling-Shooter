using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Playniax.SpaceShooterArtPack02
{
    public class SerpentBody : MonoBehaviour
    {
        public Transform head;
        public float distanceToHead;

        void Update()
        {
            UpdateBodyPart();
        }

        public void UpdateBodyPart()
        {
            var direction = head.position - transform.position;
            var angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);

            var distance = Vector3.Distance(head.position, transform.position);

            if (distance > distanceToHead)
            {
                var position = distance - distanceToHead;

                var x = Mathf.Cos(angle * Mathf.Deg2Rad) * position;
                var y = Mathf.Sin(angle * Mathf.Deg2Rad) * position;

                transform.position = new Vector3(x, y, 0f) + transform.position;
            }
        }
    }
}
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Tilemaps;

namespace Playniax.Pyro.Framework
{
    public class TileCollider2D : MonoBehaviour
    {
        public TilemapCollider2D tilemapCollider;
        public Collider2D[] colliders;
        public UnityEvent onCollision;

        void Start()
        {
            if (colliders.Length == 0) colliders = GetComponentsInChildren<Collider2D>();
        }

        void Update()
        {
            for (int i = 0; i < colliders.Length; i++)
            {
                if (tilemapCollider.Distance(colliders[i]).isOverlapped)
                {
                    onCollision.Invoke();

                    break;
                }
            }
            {
                //collisionState.Kill();
            }
        }
    }
}

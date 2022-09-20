// https://www.jacksondunstan.com/articles/3753

using UnityEngine;

namespace Playniax.Pyro.Framework
{
    public class CollisionBase2D : MonoBehaviour
    {
        public string group = "Enemy";
        public Collider2D[] colliders;

        public virtual void Awake()
        {
            if (colliders.Length == 0) colliders = GetComponents<Collider2D>();
        }
        public bool isSuspended
        {
            get { return _suspended; }
            set { _suspended = value; }
        }
        public virtual void OnEnable()
        {
            if (colliders.Length > 0) CollisionMonitor2D.Add(group, this);
        }
        public virtual void OnDisable()
        {
            CollisionMonitor2D.Remove(group, this);
        }
        public virtual void OnCollision(CollisionBase2D collision)
        {
        }

        bool _suspended;
    }
}
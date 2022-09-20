using System.Collections.Generic;
using UnityEngine;

namespace Playniax.Pyro.Framework
{
    // Determines what objects or group of objects can detect collisions with eachother.
    public class CollisionMonitor2D : MonoBehaviour
    {
        public class Group
        {
            public string id;

            public List<CollisionBase2D> list = new List<CollisionBase2D>();
        }

        public static List<Group> list = new List<Group>();

        // Group 1.
        public string group1 = "Player";
        // Group 2.
        public string group2 = "Enemy";
        public static void Add(string id, CollisionBase2D collisionBase2D)
        {
            for (int i = 0; i < list.Count; i++)
            {
                if (list[i].id == id)
                {
                    list[i].list.Add(collisionBase2D);

                    return;
                }
            }

            var collisionList = new Group();
            collisionList.id = id;
            list.Add(collisionList);
            Add(id, collisionBase2D);
        }

        public static void Remove(string id, CollisionBase2D collisionBase2D)
        {
            for (int i = 0; i < list.Count; i++)
            {
                if (list[i].id == id)
                {
                    list[i].list.Remove(collisionBase2D);
                }
            }
        }

        public static Group Get(string id)
        {
            for (int i = 0; i < list.Count; i++)
            {
                if (list[i].id == id)
                {
                    return list[i];
                }
            }
            return null;
        }
        void LateUpdate()
        {
            _Check();
        }

        void _Check()
        {
            var a = Get(group1);
            if (a == null) return;

            var b = Get(group2);
            if (b == null) return;

            _Check(a.list.ToArray(), b.list.ToArray(), group1 == group2);
        }

        void _Check(CollisionBase2D[] group1, CollisionBase2D[] group2, bool identical)
        {
            for (int a = 0; a < group1.Length; a++)
            {
                for (int b = a * (identical ? 1 : 0); b < group2.Length; b++)
                {
                    //if (group1[a] == group2[b]) continue;

                    if (_Check(group1[a]) == false) continue;
                    if (_Check(group2[b]) == false) continue;

                    if (_Check(group1[a].colliders, group2[b].colliders))
                    {
                        if (_Check(group1[a]) == false) continue;
                        if (_Check(group2[b]) == false) continue;

                        group1[a].OnCollision(group2[b]);
                    }
                }
            }
        }

        bool _Check(CollisionBase2D collision)
        {
            if (this == null) return false;
            if (gameObject == null) return false;
            if (isActiveAndEnabled == false) return false;

            if (collision == null) return false;
            if (collision.gameObject == null) return false;
            if (collision.isActiveAndEnabled == false) return false;
            if (collision.isSuspended == true) return false;

            return true;
        }
        bool _Check(Collider2D[] colliders1, Collider2D[] colliders2)
        {
            for (int a = 0; a < colliders1.Length; a++)
            {
                for (int b = 0; b < colliders2.Length; b++)
                {
                    if (colliders1[a] != colliders2[b] && colliders1[a].Distance(colliders2[b]).isOverlapped) return true;
                }
            }
            return false;
        }
    }
}
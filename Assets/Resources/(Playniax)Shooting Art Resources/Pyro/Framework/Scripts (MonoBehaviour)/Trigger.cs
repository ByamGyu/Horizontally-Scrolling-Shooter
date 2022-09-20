using System.Collections.Generic;
using UnityEngine;

namespace Playniax.Pyro.Framework
{
    public class Trigger : MonoBehaviour
    {
        public string id;
        public KeyCode key = KeyCode.None;
        public KeyCode altKey = KeyCode.None;
        public List<BulletSpawnerBase> spawners
        {
            get
            {
                return _spawners;
            }
        }
        void LateUpdate()
        {
            if (Input.GetKey(key) || Input.GetKey(altKey))
            {
                for (int i = 0; i < spawners.Count; i++)
                {
                    spawners[i].UpdateSpawner();
                }
            }
            else if (Input.GetKeyUp(key) || Input.GetKeyUp(altKey))
            {
                for (int i = 0; i < spawners.Count; i++)
                {
                    spawners[i].timer.timer = 0;
                }
            }
        }
        List<BulletSpawnerBase> _spawners = new List<BulletSpawnerBase>();
    }
}
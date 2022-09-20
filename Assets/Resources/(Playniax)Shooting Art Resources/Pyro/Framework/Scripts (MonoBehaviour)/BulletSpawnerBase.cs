using UnityEngine;
using Playniax.Ignition.Framework;

namespace Playniax.Pyro.Framework
{
    public class BulletSpawnerBase : MonoBehaviour
    {
        public string id;
        public string triggerId;
        public Trigger trigger;
        public bool automatically = true;
        public Timer timer;
        public static BulletSpawnerBase GetSpawner(string id)
        {
            var spawners = FindObjectsOfType<BulletSpawnerBase>();

            for (int i = 0; i < spawners.Length; i++)
            {
                if (spawners[i].id == id) return spawners[i];
            }

            return null;
        }
        public virtual void LateUpdate()
        {
            if (automatically == true && trigger == null) UpdateSpawner();
        }
        void OnEnable()
        {
            if (trigger == null) trigger = _GetTrigger();

            if (trigger) trigger.spawners.Add(this);
        }
        void OnDisable()
        {
            if (trigger) trigger.spawners.Remove(this);
        }
        public virtual GameObject OnSpawn()
        {
            return null;
        }
        public virtual void UpdateSpawner()
        {
            if (timer.Update()) OnSpawn();
        }
        Trigger _GetTrigger()
        {
            var triggers = GetComponentsInChildren<Trigger>();

            for (int i = 0; i < triggers.Length; i++)
            {
                if (triggers[i].id != "" && triggers[i].id == triggerId) return triggers[i];
            }

            return null;
        }
    }
}

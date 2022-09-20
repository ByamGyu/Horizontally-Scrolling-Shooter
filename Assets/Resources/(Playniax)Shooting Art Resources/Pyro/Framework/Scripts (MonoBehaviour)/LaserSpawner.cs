using UnityEngine;
using Playniax.Ignition.Framework;

namespace Playniax.Pyro.Framework
{
    public class LaserSpawner : BulletSpawnerBase
    {
        public GameObject prefab;
        public int playerIndex = -1;
        public float size = 1;
        public int orderInLayer = 100;
        public float range = 0;
        public int damage = 1000;
        public float ttl = .25f;
        public int index;
        public AudioProperties audioProperties;

        public override void UpdateSpawner()
        {
            Laser.Fire(prefab, playerIndex, orderInLayer, timer, gameObject, range, ttl, size, damage, audioProperties, index);
        }
        void Awake()
        {
            if (prefab.scene.rootCount > 0) prefab.SetActive(false);
        }
    }
}
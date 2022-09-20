using UnityEngine;
using Playniax.Pyro.Framework;

namespace Playniax.Pyro.UI
{
    public class BulletSpawnerUI : MonoBehaviour
    {
        public enum Mode { Horizontal, Vertical };

        public string bulletSpawnerId;
        public BulletSpawnerBase bulletSpawner;
        public int max = 100;
        public Mode mode;
        void Awake()
        {
            if (bulletSpawner == null) bulletSpawner = _GetSpawner();

            _transform = GetComponent<Transform>();

            _Update();
        }
        void Update()
        {
            _Update();
        }
        BulletSpawnerBase _GetSpawner()
        {
            var spawners = FindObjectsOfType<BulletSpawnerBase>();

            for (int i = 0; i < spawners.Length; i++)
            {
                if (spawners[i].id != "" && spawners[i].id == bulletSpawnerId) return spawners[i];
            }

            return null;
        }
        void _Update()
        {
            if (bulletSpawner == null) return;
            if (bulletSpawner.timer.counter < 0) return;
            if (_transform == null) return;

            var scale = 1f / max * bulletSpawner.timer.counter;

            if (scale < 0) scale = 0;
            if (scale > 1) scale = 1;

            if (mode == Mode.Horizontal)
            {
                _transform.localScale = new Vector2(scale, _transform.localScale.y);
            }
            else
            {
                _transform.localScale = new Vector2(_transform.localScale.x, scale);
            }
        }

        Transform _transform;
    }
}
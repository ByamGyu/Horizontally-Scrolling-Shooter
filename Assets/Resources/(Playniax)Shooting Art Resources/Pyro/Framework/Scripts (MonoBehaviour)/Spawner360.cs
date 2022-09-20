using UnityEngine;
using Playniax.Ignition.Framework;

namespace Playniax.Pyro.Framework
{
    public class Spawner360 : BulletSpawnerBase
    {
        public GameObject prefab;
        public int count = 10;
        public float speed = 8;
        public Vector3 position;
        public bool autoDestroy;
        public virtual void Awake()
        {
            if (prefab && prefab.scene.rootCount > 0) prefab.SetActive(false);
        }
        public override GameObject OnSpawn()
        {
            if (prefab == null) return null;

            for (int i = 0; i < count; i++)
            {
                var clone = Instantiate(prefab, transform);
                if (clone)
                {
                    clone.transform.position = transform.position;
                    clone.transform.rotation = transform.rotation;
                    clone.transform.Translate(position, Space.Self);

                    var bulletBase = clone.GetComponent<BulletBase>();
                    if (bulletBase)
                    {
                        float angle = i * (360f / count) * Mathf.Deg2Rad;

                        clone.transform.eulerAngles = new Vector3(0, 0, angle * Mathf.Rad2Deg);

                        bulletBase.velocity = new Vector3(Mathf.Cos(angle), Mathf.Sin(angle)) * speed;
                    }
                    else
                    {
                        var rb = clone.GetComponent<Rigidbody2D>();
                        if (rb)
                        {
                            float angle = i * (360f / count) * Mathf.Deg2Rad;

                            clone.transform.eulerAngles = new Vector3(0, 0, angle * Mathf.Rad2Deg);

                            rb.velocity = new Vector3(Mathf.Cos(angle), Mathf.Sin(angle)) * speed;
                        }
                    }

                    clone.transform.parent = null;

                    clone.SetActive(true);
                }
            }

            if (autoDestroy && timer.counter == 0) Destroy(gameObject);

            return null;
        }
    }
}
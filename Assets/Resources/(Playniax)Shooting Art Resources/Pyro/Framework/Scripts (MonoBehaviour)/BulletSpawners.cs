using UnityEngine;
using Playniax.Ignition.Framework;

namespace Playniax.Pyro.Framework
{
    public class BulletSpawners : BulletSpawnerBase
    {
        [System.Serializable]
        public class SpawnPoints
        {
            public string name;
            public int group;
            public Vector3 position;
            public Vector3 rotation;

            public AlternativeSettings alternativeSettings;
        }

        [System.Serializable]
        public class AlternativeSettings
        {
            public GameObject prefab;
            public float bulletSpeed;
            public AudioProperties audioProperties;
        }

        public int group;
        public GameObject prefab;
        public float bulletSpeed = 16;
        public AudioProperties audioProperties;
        public SpawnPoints[] spawnPoints = new SpawnPoints[1];
        public Vector3 rotation;
        public void FireBullet(int i)
        {
            var obj = Instantiate(_GetPrefab(), transform);
            if (obj)
            {
                obj.transform.position = transform.position;
                obj.transform.rotation = transform.rotation;
                obj.transform.Rotate(rotation);
                obj.transform.Translate(spawnPoints[i].position);
                obj.transform.Rotate(spawnPoints[i].rotation);

                var bulletBase = obj.GetComponent<BulletBase>();
                if (bulletBase) bulletBase.velocity = obj.transform.rotation * new Vector3(_GetBulletSpeed(), 0, 0);

                obj.transform.parent = null;

                obj.SetActive(true);

                spawnPoints[i].alternativeSettings.audioProperties.Play();
            }

            float _GetBulletSpeed()
            {
                if (spawnPoints[i].alternativeSettings.bulletSpeed > 0) return spawnPoints[i].alternativeSettings.bulletSpeed; else return bulletSpeed;
            }

            GameObject _GetPrefab()
            {
                if (spawnPoints[i].alternativeSettings.prefab) return spawnPoints[i].alternativeSettings.prefab; else return prefab;
            }
        }

        void Awake()
        {
            if (prefab && prefab.scene.rootCount > 0) prefab.SetActive(false);

            for (int i = 0; i < spawnPoints.Length; i++)
            {
                if (spawnPoints[i].alternativeSettings.prefab && spawnPoints[i].alternativeSettings.prefab.scene.rootCount > 0) spawnPoints[i].alternativeSettings.prefab.SetActive(false);
            }
        }
        public override GameObject OnSpawn()
        {
            //        if ((ObjectCounter.objects > 0) && timer.Update())
            for (int i = 0; i < spawnPoints.Length; i++)
            {
                if (spawnPoints[i].group == group)
                {
                    FireBullet(i);
                }
            }

            audioProperties.Play();

            return null;
        }
    }
}
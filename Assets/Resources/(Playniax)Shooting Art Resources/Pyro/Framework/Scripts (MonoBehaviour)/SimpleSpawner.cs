using UnityEngine;
using Playniax.Ignition.Framework;

namespace Playniax.Pyro.Framework
{
    [AddComponentMenu("Playniax/Pyro/Framework/SimpleSpawner")]
    public class SimpleSpawner : SpawnerBase
    {
        public enum StartPosition { Left, Right, Top, Bottom, LeftOrRight, TopOrBottom, Random, Fixed };

        public GameObject[] prefab;
        public Transform parent;
        public StartPosition startPosition = StartPosition.Random;
        public float timer;
        public float delay;
        public int counter;
        public bool trackProgress;
        public override void Awake()
        {
            if (_GetPrefabs() == 0)
            {
                enabled = false;

                return;
            }

            base.Awake();

            for (int i = 0; i < prefab.Length; i++)
            {
                if (prefab[i] && prefab[i].scene.rootCount > 0) prefab[i].SetActive(false);
            }

            if (trackProgress)
            {
                GameData.progressScale += counter;
                GameData.progress += counter;
            }
        }
        public virtual GameObject OnSpawn()
        {
            var index = Random.Range(0, prefab.Length);

            var clone = Instantiate(prefab[index]);
            if (clone)
            {
                if (trackProgress)
                {
                    var progress = clone.GetComponent<ProgressCounter>();
                    if (progress == null) progress = clone.AddComponent<ProgressCounter>();
                }

                SetPosition(clone);

                clone.transform.SetParent(parent);

                clone.SetActive(true);
            }

            return clone;
        }
        public virtual void SetPosition(GameObject obj)
        {
            if (startPosition == StartPosition.Left)
            {
                _SetPosition(obj, 0);
            }
            else if (startPosition == StartPosition.Right)
            {
                _SetPosition(obj, 1);
            }
            else if (startPosition == StartPosition.Top)
            {
                _SetPosition(obj, 2);
            }
            else if (startPosition == StartPosition.Bottom)
            {
                _SetPosition(obj, 3);
            }
            else if (startPosition == StartPosition.LeftOrRight)
            {
                _SetPosition(obj, Random.Range(0, 2));
            }
            else if (startPosition == StartPosition.TopOrBottom)
            {
                _SetPosition(obj, Random.Range(2, 4));
            }
            else if (startPosition == StartPosition.Random)
            {
                _SetPosition(obj, Random.Range(0, 4));
            }
            else if (startPosition == StartPosition.Fixed)
            {
                obj.transform.position = transform.position;
            }
        }
        public virtual void UpdateSpawner()
        {
            if (_GetPrefabs() == 0)
            {
                enabled = false;

                return;
            }
            else if (counter == -1)
            {
                if (timer <= 0)
                {
                    OnSpawn();

                    timer = delay;
                }
                else
                {
                    timer -= 1 * Time.deltaTime;
                }
            }
            else if (counter > 0)
            {
                if (timer <= 0)
                {
                    OnSpawn();

                    counter -= 1;

                    if (counter > 0)
                    {
                        timer = delay;

                        UpdateSpawner();
                    }
                    else
                    {
                        enabled = false;
                    }
                }
                else
                {
                    timer -= 1 * Time.deltaTime;
                }
            }
        }
        void Update()
        {
            UpdateSpawner();
        }
        int _GetPrefabs()
        {
            var prefabs = 0;

            for (int i = 0; i < prefab.Length; i++)
            {
                if (prefab[i]) prefabs += 1; ;
            }

            return prefabs;
        }
        void _SetPosition(GameObject obj, int segment)
        {
            // Segment:

            // 0 = left
            // 1 = right
            // 2 = top
            // 3 = bottom

            var size = RendererHelpers.GetSize(obj) * .5f;

            var min = Camera.main.ViewportToWorldPoint(new Vector3(0, 1, transform.position.z - Camera.main.transform.position.z));
            var max = Camera.main.ViewportToWorldPoint(new Vector3(1, 0, transform.position.z - Camera.main.transform.position.z));

            min.x -= size.x;
            max.x += size.x;

            min.y += size.y;
            max.y -= size.y;

            var position = Vector3.zero;

            if (segment == 0)
            {
                position.x = min.x;
                position.y = Random.Range(min.y + size.y, max.y - size.y);
            }
            else if (segment == 1)
            {
                position.x = max.x;
                position.y = Random.Range(min.y + size.y, max.y - size.y);
            }
            else if (segment == 2)
            {
                position.x = Random.Range(min.x - size.x, max.x + size.x);
                position.y = min.y;
            }
            else if (segment == 3)
            {
                position.x = Random.Range(min.x - size.x, max.x + size.x);
                position.y = max.y;
            }

            obj.transform.position = position;
        }
    }
}
using UnityEngine;
using Playniax.Ignition.Framework;

namespace Playniax.Spawners
{
    public class Portal : SpawnerBase
    {
        public class Glow : MonoBehaviour
        {
            public float position;
            public float speed;
            public SpriteRenderer spriteRenderer;
            public Portal portal;

            void Update()
            {
                spriteRenderer.sortingOrder = portal.spriteRenderer.sortingOrder + Random.Range(-1, 1);

                var x = Mathf.Cos(position) * portal.distance;
                var y = Mathf.Sin(position) * portal.distance;

                transform.localPosition = new Vector3(x, y);

                position += speed * Time.deltaTime;

            }
        }

        public class Intro : MonoBehaviour
        {
            public Vector3 scale;
            public int steps = 10;
            void Update()
            {
                var localScale = transform.localScale;

                localScale.x += (scale.x - localScale.x) / steps;
                localScale.y += (scale.y - localScale.y) / steps;
                localScale.z += (scale.y - localScale.z) / steps;

                transform.localScale = localScale;

                if (transform.localScale == scale) Destroy(this);
            }
        }
        [System.Serializable]
        public class SpawnerSettings
        {
            [System.Serializable]
            public class SoundSettings
            {
                public AudioProperties open;
                public AudioProperties intro;
                public AudioProperties close;
            }

            public GameObject[] prefab;
            public Transform parent;
            public float timer;
            public float delay;
            public int counter = 1;
            public int maxAtOnce = 1;
            public int introSteps = 10;
            public float tweak = 1f;
            public bool trackProgress;
            public SoundSettings soundSettings;
        }

        public Sprite glowSprite;
        public float glowSize = .5f;
        public int particles = 25;
        public float distance = 1.11f;
        public float speed = 3;
        public SpriteRenderer spriteRenderer;
        public Material material;

        public SpawnerSettings spawnerSettings;

        public virtual bool GetOK()
        {
            return true;
        }

        public virtual bool OnOpen(float size)
        {
            return true;
        }
        public virtual GameObject OnSpawn()
        {
            var clone = Instantiate(spawnerSettings.prefab[_index]);
            if (clone)
            {
                if (spawnerSettings.trackProgress)
                {
                    var progress = clone.GetComponent<ProgressCounter>();
                    if (progress == null) progress = clone.AddComponent<ProgressCounter>();
                }

                var intro = clone.AddComponent<Intro>();
                intro.steps = spawnerSettings.introSteps;
                intro.scale = clone.transform.localScale;
                clone.transform.localScale = Vector3.zero;
                clone.transform.position = transform.position;
                clone.transform.SetParent(spawnerSettings.parent);
                clone.SetActive(true);

                spawnerSettings.soundSettings.intro.Play();
            }

            return clone;
        }

        public override void Awake()
        {
            base.Awake();

            _InitGlow();
            _InitSpawner();
        }

        void Update()
        {
            if (_GetPrefabs() == 0) return;

            if (_state == 0 && GetOK())
            {
                if (spawnerSettings.counter == -1)
                {
                    if (spawnerSettings.timer <= 0)
                    {
                        _GetPrefab();

                        _state = 1;

                        spawnerSettings.timer = spawnerSettings.delay;
                    }
                    else
                    {
                        spawnerSettings.timer -= 1 * Time.deltaTime;
                    }
                }
                else if (spawnerSettings.counter > 0)
                {
                    if (spawnerSettings.timer <= 0)
                    {
                        _GetPrefab();

                        _state = 1;

                        spawnerSettings.counter -= 1;

                        if (spawnerSettings.counter > 0)
                        {
                            spawnerSettings.timer = spawnerSettings.delay;
                        }
                    }
                    else
                    {
                        spawnerSettings.timer -= 1 * Time.deltaTime;
                    }
                }

            }
            else if (_state == 1)
            {
                if (OnOpen(_size) == true)
                {
                    spriteRenderer.gameObject.SetActive(true);

                    spawnerSettings.soundSettings.open.Play();

                    _state = 2;
                }
            }
            else if (_state == 2)
            {
                transform.localScale += new Vector3(1, 1, 0) * Time.deltaTime * .5f;

                if (transform.localScale.x > _size * spawnerSettings.tweak)
                {
                    _space = 1;

                    _state = 3;
                }
            }
            else if (_state == 3)
            {
                _space -= Time.deltaTime;

                if (_space <= 0)
                {
                    if (spawnerSettings.maxAtOnce <= 1)
                    {
                        _state = 4;
                    }
                    else
                    {
                        if (spawnerSettings.counter == -1)
                        {
                            _multipleCounter = Random.Range(0, spawnerSettings.maxAtOnce);
                        }
                        else
                        {
                            _multipleCounter = Random.Range(0, spawnerSettings.maxAtOnce);

                            if (_multipleCounter > spawnerSettings.counter) _multipleCounter = spawnerSettings.counter;

                            spawnerSettings.counter -= _multipleCounter;
                        }

                        _state = 5;
                    }
                }
            }
            else if (_state == 4)
            {
                OnSpawn();

                _space = 1;

                _state = 6;
            }
            else if (_state == 5)
            {
                _space -= Time.deltaTime;

                if (_space <= 0)
                {
                    OnSpawn();

                    _multipleCounter -= 1;

                    if (_multipleCounter < 0)
                    {
                        _space = 1;

                        _state = 6;
                    }
                    else
                    {
                        _space = .5f;
                    }
                }
            }
            else if (_state == 6)
            {
                _space -= Time.deltaTime;

                if (_space <= 0)
                {
                    _state = 7;
                }

            }
            else if (_state == 7)
            {
                transform.localScale -= new Vector3(1, 1, 0) * Time.deltaTime * .5f;

                if (transform.localScale.x <= 0)
                {
                    if (spawnerSettings.counter == 0)
                    {
                        Destroy(gameObject);
                    }
                    else
                    {
                        spriteRenderer.gameObject.SetActive(false);

                        _state = 0;
                    }

                    spawnerSettings.soundSettings.close.Play();
                }
            }
        }

        int _GetPrefabs()
        {
            var prefabs = 0;

            for (int i = 0; i < spawnerSettings.prefab.Length; i++)
            {
                if (spawnerSettings.prefab[i]) prefabs += 1;
            }

            return prefabs;
        }

        void _GetPrefab()
        {
            _index = Random.Range(0, spawnerSettings.prefab.Length);

            var size = RendererHelpers.GetSize(spawnerSettings.prefab[_index]);

            _size = Mathf.Max(size.x, size.y) * .5f;
        }
        void _InitGlow()
        {
            for (int i = 0; i < particles; i++)
            {
                var glow = new GameObject("Glow " + (i + 1)).AddComponent<Glow>();
                glow.portal = this;
                glow.speed = Random.Range(-speed, speed);
                glow.position = Random.Range(0, 359) * Mathf.Deg2Rad;
                glow.transform.localScale *= Random.Range(glowSize / 2, glowSize);

                glow.spriteRenderer = glow.gameObject.AddComponent<SpriteRenderer>();
                glow.spriteRenderer.sprite = glowSprite;
                glow.spriteRenderer.material = material;
                glow.spriteRenderer.sortingOrder = spriteRenderer.sortingOrder - 1;

                glow.transform.SetParent(spriteRenderer.transform);
            }
        }
        void _InitSpawner()
        {
            if (_GetPrefabs() > 0)
            {
                transform.localScale = new Vector3(0, 0, 1);

                spriteRenderer.gameObject.SetActive(false);
            }

            for (int i = 0; i < spawnerSettings.prefab.Length; i++)
            {
                if (spawnerSettings.prefab[i] && spawnerSettings.prefab[i].scene.rootCount > 0) spawnerSettings.prefab[i].SetActive(false);
            }

            if (spawnerSettings.trackProgress)
            {
                GameData.progressScale += spawnerSettings.counter;
                GameData.progress += spawnerSettings.counter;
            }
        }

        int _index;
        int _multipleCounter;
        float _size;
        int _state;
        float _space;
    }
}

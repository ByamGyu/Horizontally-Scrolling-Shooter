// https://docs.unity3d.com/ScriptReference/EditorGUI.MinMaxSlider.html

using System.Collections.Generic;
using UnityEngine;
using Playniax.Ignition.Framework;

namespace Playniax.ParticleSystem
{
    public class Emitter : MonoBehaviour
    {
        /*
        public class Plugin
        {
            public virtual void PostInstantiate(GameObject gameObject)
            {
            }
            public virtual void PostPlay(GameObject gameObject)
            {
            }
        }

        public static Plugin plugin;
        */

        public GameObject prefab;
        public string id;
        public int initialize = 16;
        public bool autoGrow = false;
        public bool hideInHierarchy = true;
        public Sprite sprite;
        public Shader shader;
        public string delay;
        public string ttl = "1";
        public string particles = "8";
        public string origin = "0";
        public string scale = "1";
        public string angle = "0,359";
        public string startScale = "1";
        public string targetScale = "1";
        public Vector2 fixedScale;
        public Vector2 constant;
        public string speed = "1";
        public bool implode;
        public string friction = "0";
        public string gravity = "0";
        public string spin = "0";
        public Color startColor = new Color(1, 1, 1, 1);
        public Color targetColor;
        public bool randomColorRange = false;
        public string orderInLayer = "0";
        //public int peak;

        void Awake()
        {
            _emitters.Add(this);

            if (prefab && prefab.scene.rootCount > 0) prefab.SetActive(false);
        }

        void OnDestroy()
        {
            _emitters.Remove(this);
        }

        public static Emitter Find(string id)
        {
            if (_emitters == null) return null;

            for (int i = 0; i < _emitters.Count; i++)
            {
                if (_emitters[i].id == id) return _emitters[i];
            }
            return null;
        }

        public static int FindIndex(string id)
        {
            if (_emitters == null) return -1;

            for (int i = 0; i < _emitters.Count; i++)
            {
                if (_emitters[i].id == id) return i;
            }
            return -1;
        }

        public static void Play(string id, Vector3 position, Transform parent = null, float scale = 1, int sortingOrder = 0)
        {
            foreach (Emitter e in _emitters)
            {
                if (e.id == id) e.Play(position, parent, scale, sortingOrder);
            }
        }

        public void Play(Vector3 position, Transform parent = null, float scale = 1, int sortingOrder = 0)
        {
            _Init();

            scale *= _RandomFloat(this.scale, 1);

            for (int i = 0; i < _RandomInt(particles, 8); i++)
            {

                var gameObject = _GetAvailableObject();
                if (gameObject == null) return;

                var spriteRendererComponent = gameObject.GetComponent<SpriteRenderer>();
                var particleComponent = gameObject.GetComponent<Particle>();

                spriteRendererComponent.enabled = false;

                spriteRendererComponent.sortingOrder = sortingOrder + _RandomInt(orderInLayer);

                particleComponent.delay = _RandomFloat(delay);

                particleComponent.startScale = _Vec2Parse(startScale) * scale;
                particleComponent.targetScale = _Vec2Parse(targetScale) * scale;

                if (fixedScale.x != 0)
                {
                    particleComponent.startScale.x = fixedScale.x * scale;
                    particleComponent.targetScale.x = fixedScale.x * scale;
                }

                if (fixedScale.y != 0)
                {
                    particleComponent.startScale.y = fixedScale.y * scale;
                    particleComponent.targetScale.y = fixedScale.y * scale;
                }

                particleComponent.friction = _RandomFloat(friction, 1);

                particleComponent.gravity = float.Parse(gravity);
                particleComponent.constant = constant;

                if (randomColorRange == false)
                {
                    particleComponent.startColor = startColor;
                    particleComponent.targetColor = targetColor;
                }
                else
                {
                    particleComponent.startColor = _RandomColor(startColor, targetColor, startColor.a);
                    particleComponent.targetColor = _RandomColor(startColor, targetColor, targetColor.a);
                }

                if (shader) spriteRendererComponent.material.shader = shader;

                particleComponent.ttl = _RandomFloat(ttl, 3000);

                var a = _RandomInt(angle);
                var x = Mathf.Cos(a * Mathf.Deg2Rad);
                var y = Mathf.Sin(a * Mathf.Deg2Rad);
                var s = _RandomFloat(speed) * scale;
                var o = _RandomFloat(origin) * scale;
                var v = new Vector3(x, y);

                particleComponent.velocity = v * s * (implode ? -1 : 1);

                particleComponent.spin = new Vector3(0, 0, _RandomFloat(spin));

                gameObject.transform.SetParent(parent);
                gameObject.transform.position = position;
                gameObject.transform.position += v * o;

                gameObject.transform.localRotation = Quaternion.Euler(0, 0, a);

                gameObject.SetActive(true);

                //if (plugin != null) plugin.PostPlay(gameObject);
            }
        }

        Vector2 _Vec2Parse(string str)
        {
            var r = _RandomFloat(str);
            return new Vector2(r, r);
        }

        GameObject _CreateParticle()
        {
            GameObject particle;

            if (prefab)
            {
                particle = Instantiate(prefab);
            }
            else
            {
                particle = new GameObject();
            }

            particle.name = sprite.name + ObjectPooler.marker;
            particle.SetActive(false);
            var renderer = particle.AddComponent<SpriteRenderer>();
            renderer.sprite = sprite;
            renderer.enabled = false;
            particle.AddComponent<Particle>();

            if (hideInHierarchy) particle.hideFlags = HideFlags.HideInHierarchy;

            //if (plugin != null) plugin.PostInstantiate(particle);

            return particle;
        }

        GameObject _GetAvailableObject(int index = 0)
        {
            for (int i = 0; i < _pool.Count; i++)
            {
                if (_pool[i] && !_pool[i].activeInHierarchy) return _pool[i];
            }

            if (autoGrow && sprite)
            {
                var particle = _CreateParticle();
                _pool.Add(particle);
                //if (peak < _pool.Count) peak = _pool.Count;
                return particle;
            }

            return null;
        }

        void _Init()
        {
            if (initialize == _initialized)
                return;

            if (_pool == null)
                return;

            _pool.Clear();

            for (int i = 0; i < initialize; i++)
            {
                _pool.Add(_CreateParticle());
                //peak = _pool.Count;
            }

            _initialized = initialize;
        }

        Color _RandomColor(Color startColor, Color targetColor, float alpha)
        {
            float r = Random.Range(startColor.r, targetColor.r);
            float g = Random.Range(startColor.g, targetColor.g);
            float b = Random.Range(startColor.b, targetColor.b);
            return new Color(r, g, b, alpha);
        }

        float _RandomFloat(string str, float defaultValue = 0)
        {
            if (str.Trim() == "") return defaultValue;
            string[] r = str.Split(',');
            if (r.Length == 1) return float.Parse(str, System.Globalization.NumberStyles.Any, System.Globalization.CultureInfo.InvariantCulture);
            float min = float.Parse(r[0], System.Globalization.NumberStyles.Any, System.Globalization.CultureInfo.InvariantCulture);
            float max = float.Parse(r[1], System.Globalization.NumberStyles.Any, System.Globalization.CultureInfo.InvariantCulture);
            return Random.Range(min, max);
        }

        int _RandomInt(string str, int defaultValue = 0)
        {
            if (str.Trim() == "") return defaultValue;
            string[] r = str.Split(',');
            if (r.Length == 1) return int.Parse(str);
            int min = int.Parse(r[0]);
            int max = int.Parse(r[1]);
            return Random.Range(min, max);
        }

        static List<Emitter> _emitters = new List<Emitter>();

        List<GameObject> _pool = new List<GameObject>();
        int _initialized;
    }
}
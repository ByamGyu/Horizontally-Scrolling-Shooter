using UnityEngine;
using Playniax.Ignition.Framework;

namespace Playniax.ParticleSystem
{
    public class Particle : MonoBehaviour
    {
        public float delay;
        public float ttl;
        public Vector3 velocity;
        public Vector3 constant;
        public float friction;
        public float gravity;
        public Vector3 spin;
        public Color startColor;
        public Color targetColor;
        public Vector3 startScale;
        public Vector3 targetScale;
        public bool removeFromParent = false;

        void Awake()
        {
            _spriteRenderer = GetComponent<SpriteRenderer>();
        }

        void OnEnable()
        {
            if (_spriteRenderer != null) _Init();
        }

        void OnDisable()
        {
            _running = false;
        }

        public void Update()
        {
            _UpdateParticle();
        }

        void _Init()
        {
            if (_running == false)
            {
                _ttl = ttl;
                _spriteRenderer.color = startColor;
                transform.localScale = startScale;
                _running = true;
            }
        }

        void _UpdateParticle()
        {
            delay -= 1 * Time.deltaTime;

            if (delay > 0) return;

            if (_spriteRenderer.enabled == false) _spriteRenderer.enabled = true;

            transform.localPosition += constant * Time.deltaTime;
            transform.localPosition += velocity * Time.deltaTime;

            transform.Rotate(spin * Time.deltaTime);

            if (friction != 0) velocity *= 1 / (1 + (Time.deltaTime * friction));

            velocity.y -= gravity * Time.deltaTime;

            if (ttl > 0 && _ttl > 0)
            {
                _spriteRenderer.color = targetColor - (targetColor - startColor) * (ttl / _ttl);
                transform.localScale = targetScale - (targetScale - startScale) * (ttl / _ttl);

                ttl -= 1 * Time.deltaTime;
            }
            else
            {
                if (removeFromParent && transform.parent) transform.parent = null;

                if (name.Contains(ObjectPooler.marker))
                {
                    gameObject.SetActive(false);
                }
                else
                {
                    Destroy(gameObject);
                }

                _running = false;
            }
        }

        SpriteRenderer _spriteRenderer;
        bool _running;
        float _ttl;
    }
}
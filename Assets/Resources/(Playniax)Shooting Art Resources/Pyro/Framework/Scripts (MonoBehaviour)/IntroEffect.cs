using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Playniax.Pyro.Framework
{
    public class IntroEffect : MonoBehaviour
    {
        [System.Serializable]
        public class FlickerSettings
        {
            public int count = 15;
            public float sustain = 1f;
            public CollisionBase2D collisionBase;

            float _counter;
            float _timer;

            public void Update(IntroEffect instance)
            {
                if (collisionBase.isSuspended == false) return;

                if (_counter == count) collisionBase.isSuspended = false;

                _timer += Time.deltaTime;

                if (_timer > (sustain / 10))
                {
                    instance.spriteRenderer.enabled = !instance.spriteRenderer.enabled;
                    _counter += .5f;
                    _timer = 0;
                }
            }
        }
        public enum Mode { Flicker };
        public Mode mode;
        public FlickerSettings flickerSettings;
        public SpriteRenderer spriteRenderer;
        void Start()
        {
            if (spriteRenderer == null) spriteRenderer = GetComponent<SpriteRenderer>();

            if (mode == Mode.Flicker)
            {
                if (flickerSettings.collisionBase == null) flickerSettings.collisionBase = GetComponent<CollisionBase2D>();

                flickerSettings.collisionBase.isSuspended = true;
            }
        }

        void Update()
        {
            if (mode == Mode.Flicker)
            {
                flickerSettings.Update(this);
            }
        }
    }
}

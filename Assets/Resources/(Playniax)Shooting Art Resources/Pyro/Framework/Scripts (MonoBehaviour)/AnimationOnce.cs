using UnityEngine;

namespace Playniax.Pyro.Framework
{
    public class AnimationOnce : MonoBehaviour
    {
        public Sprite[] sprites;
        public float frameTime = .1f;
        public bool destroy = true;
        public SpriteRenderer spriteRenderer;

        void Start()
        {
            spriteRenderer = GetComponent<SpriteRenderer>();
        }
        void Update()
        {
            if (_supend) return;

            _frameTime += Time.deltaTime;

            if (_frameTime >= frameTime)
            {
                _frameTime = 0;

                _frame += 1;
            }

            if (_frame > sprites.Length - 1)
            {
                _frame = sprites.Length - 1;

                if (destroy)
                {
                    spriteRenderer.sprite = sprites[_frame];

                    Destroy(gameObject);

                    return;
                }
                else
                {
                    _supend = true;
                }
            }

            spriteRenderer.sprite = sprites[_frame];
        }

        int _frame;
        float _frameTime;
        bool _supend;
    }
}

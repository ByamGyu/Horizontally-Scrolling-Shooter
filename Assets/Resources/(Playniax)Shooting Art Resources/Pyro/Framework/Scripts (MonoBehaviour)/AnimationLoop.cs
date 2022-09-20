using UnityEngine;

namespace Playniax.Pyro.Framework
{
    public class AnimationLoop : MonoBehaviour
    {
        public Sprite[] sprites;
        public float frameTime = .1f;
        public SpriteRenderer spriteRenderer;

        void Start()
        {
            spriteRenderer = GetComponent<SpriteRenderer>();
        }
        void Update()
        {
            _frameTime += Time.deltaTime;

            if (_frameTime < frameTime) return;

            _frameTime = 0;

            _frame += 1;

            if (_frame < 0)
            {
                _frame = sprites.Length - 1;
            }
            else if (_frame > sprites.Length - 1)
            {
                _frame = 0;
            }

            spriteRenderer.sprite = sprites[_frame];
        }

        int _frame;
        float _frameTime;
    }
}

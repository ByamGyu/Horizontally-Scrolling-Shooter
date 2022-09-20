using UnityEngine;

namespace Playniax.Pyro.Framework
{
    public class AnimationPingPong : MonoBehaviour
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

            _frame += _pingPong;

            if (_frame <= 0)
            {
                _frame = 0;

                _pingPong = -_pingPong;
            }
            else if (_frame >= sprites.Length - 1)
            {
                _frame = sprites.Length - 1;

                _pingPong = -_pingPong;
            }

            spriteRenderer.sprite = sprites[_frame];
        }

        int _frame;
        float _frameTime;
        int _pingPong = 1;
    }
}

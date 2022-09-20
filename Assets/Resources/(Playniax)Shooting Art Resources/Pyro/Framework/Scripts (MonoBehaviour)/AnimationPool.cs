using UnityEngine;

namespace Playniax.Pyro.Framework
{
    public class AnimationPool : MonoBehaviour
    {
        public Animation[] animations;

        [System.Serializable]
        public class Animation
        {
            public string name = "Animation";
            public Sprite[] sprites;
            public void Play(SpriteRenderer spriteRenderer, float frameTime, bool backwards = false)
            {
                _frameTime += Time.deltaTime;

                if (_frameTime >= frameTime)
                {
                    _frameTime = 0;

                    _frame += backwards ? -1 : 1;
                }

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
            public bool PlayOnce(SpriteRenderer spriteRenderer, float frameTime, bool backwards = false)
            {
                _frameTime += Time.deltaTime;

                if (_frameTime >= frameTime)
                {
                    _frameTime = 0;

                    _frame += backwards ? -1 : 1;
                }

                if (_frame < 0)
                {
                    _frame = 0;

                    return true;
                }
                else if (_frame > sprites.Length - 1)
                {
                    _frame = sprites.Length - 1;

                    return true;
                }

                spriteRenderer.sprite = sprites[_frame];

                return false;
            }
            public void PlayPingPong(SpriteRenderer spriteRenderer, float frameTime)
            {
                _frameTime += Time.deltaTime;

                if (_frameTime >= frameTime)
                {
                    _frameTime = 0;

                    _frame += _pingPong;
                }

                if (_frame < 0)
                {
                    _frame = 0;

                    _pingPong = -_pingPong;
                }
                else if (_frame > sprites.Length - 1)
                {
                    _frame = sprites.Length - 1;

                    _pingPong = -_pingPong;
                }

                spriteRenderer.sprite = sprites[_frame];
            }
            public int GetFrame()
            {
                return _frame % sprites.Length;
            }
            public int GetFrames()
            {
                return sprites.Length;
            }
            public Sprite GetSprite()
            {
                return sprites[_frame];
            }
            public Sprite GetSprite(int frame)
            {
                return sprites[frame];
            }
            public bool isLastFrame
            {
                get
                {
                    if (_frame == sprites.Length - 1) return true;
                    return false;
                }
            }
            public void SetFrame(SpriteRenderer spriteRenderer, int frame)
            {
                _frame = frame % sprites.Length;

                spriteRenderer.sprite = sprites[_frame];
            }

            int _frame;
            float _frameTime;
            int _pingPong = 1;
        }
        public Animation Find(string name)
        {
            for (int i = 0; i < animations.Length; i++)
            {
                if (animations[i] != null && animations[i].name == name) return animations[i];
            }
            return null;
        }
    }
}

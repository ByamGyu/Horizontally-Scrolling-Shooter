using UnityEngine;
using Playniax.Pyro.Framework;

namespace Playniax.SpaceShooterArtPack02
{
    public class ClawBoss : MonoBehaviour
    {
        [System.Serializable]
        public class Animation
        {
            public Sprite[] sprites;
            public float frameTime = .05f;
        }

        public int rotations = 5;
        public float movementRange = 1;
        public float movementSpeed = 1;
        public int punchingRange = 10;
        public float punchingPower = 25;

        public Animation idle;
        public Animation clench;
        public Animation attack;
        public Animation open;

        public int frame;

        public BulletSpawnerBase bulletSpawner;
        public SpriteRenderer spriteRenderer;

        void Awake()
        {
            if (bulletSpawner == null) bulletSpawner = GetComponent<BulletSpawnerBase>();
            if (spriteRenderer == null) spriteRenderer = GetComponent<SpriteRenderer>();

            if (bulletSpawner) bulletSpawner.automatically = false;
        }

        void Update()
        {
            if (_state == 0)
            {
                if (bulletSpawner && bulletSpawner.automatically == false) bulletSpawner.UpdateSpawner();

                spriteRenderer.sprite = idle.sprites[frame];

                _frameTime += Time.deltaTime;

                if (_frameTime >= idle.frameTime)
                {
                    _frameTime = 0;

                    frame += 1;

                    if (frame >= idle.sprites.Length - 1)
                    {
                        frame = 0;
                        _rotations += 1;
                        if (_rotations >= rotations)
                        {
                            _rotations = 0;
                            _state += 1;
                        }
                    }
                }
            }

            if (_state == 1)
            {
                spriteRenderer.sprite = clench.sprites[frame];

                _frameTime += Time.deltaTime;

                if (_frameTime >= clench.frameTime)
                {
                    _frameTime = 0;

                    frame += 1;

                    if (frame >= clench.sprites.Length - 1)
                    {
                        frame = 0;
                        _state += 1;
                    }
                }
            }

            if (_state == 2)
            {
                if (transform.localPosition.x >= punchingRange)
                {
                    _state += 1;
                }
                else
                {
                    _speed += punchingPower * Time.deltaTime;

                    transform.Translate(Vector3.right * _speed * Time.deltaTime);

                    spriteRenderer.sprite = attack.sprites[frame];

                    _frameTime += Time.deltaTime;

                    if (_frameTime >= clench.frameTime)
                    {
                        _frameTime = 0;

                        frame += 1;

                        if (frame >= attack.sprites.Length - 1)
                        {
                            frame = 0;
                        }
                    }
                }
            }

            if (_state == 3)
            {
                if (transform.localPosition.x <= 0)
                {
                    transform.localPosition = new Vector3(0, transform.localPosition.y, transform.localPosition.z);
                    frame = 0;
                    _state += 1;
                }
                else
                {
                    _speed -= punchingPower * Time.deltaTime;

                    transform.Translate(Vector3.left * (_speed + 1) * Time.deltaTime);

                    spriteRenderer.sprite = attack.sprites[frame];

                    _frameTime += Time.deltaTime;

                    if (_frameTime >= attack.frameTime)
                    {
                        _frameTime = 0;

                        frame += 1;

                        if (frame >= attack.sprites.Length - 1)
                        {
                            frame = 0;
                        }
                    }
                }
            }

            if (_state == 4)
            {
                spriteRenderer.sprite = open.sprites[frame];

                _frameTime += Time.deltaTime;

                if (_frameTime >= open.frameTime)
                {
                    _frameTime = 0;

                    frame += 1;

                    if (frame >= open.sprites.Length - 1)
                    {
                        frame = 0;
                        _state = 0;
                    }
                }
            }

            if (_state == 0 || _state == 1 || _state == 4)
            {
                transform.localPosition = new Vector3(transform.localPosition.x, Mathf.Sin(_y) * movementRange, transform.localPosition.z);

                _y += movementSpeed * Time.deltaTime;
            }
        }

        float _frameTime;
        int _rotations;
        float _speed;
        int _state;
        float _y;
    }
}
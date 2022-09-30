using System.Collections.Generic;
using UnityEngine;
using Playniax.Ignition.Framework;

namespace Playniax.SpaceShooterArtPack02
{
    public class Serpent : MonoBehaviour
    {
        [System.Serializable]
        public class BodySettings
        {
            public GameObject prefab;
            public int count;
            public float space = .4f;
        }

        public enum StartPosition { Left, Right, Top, Bottom, Fixed };

        public StartPosition startPosition = StartPosition.Right;
        public bool tagetPlayer = true;
        public float rotationSpeed = 1;
        public float speed = 1;
        public BodySettings bodySettings;
        public GameObject tail;
        public float tailSpace = .36f;
        public float recalculatedistance = 1f;
        public float autoboundsMultiplier = 1f;
        public Vector2 bounds;

        void Awake()
        {
            // 카메라를 기준으로 이동 범위 잡기
            if (Camera.main.orthographic && bounds == Vector2.zero) bounds = CameraHelpers.OrthographicBounds(Camera.main).extents * autoboundsMultiplier;

            // 시작 위치 잡기
            if (startPosition != StartPosition.Fixed) _SetStartPosition();

            GameObject clone = default;

            var leader = transform;
            var space = bodySettings.space;

            // 몸통 복사 붙여넣기
            for (int i = 0; i < bodySettings.count; i++)
            {
                clone = Instantiate(bodySettings.prefab);

                _bodyParts.Add(clone);

                var serpentBody = clone.AddComponent<SerpentBody>();
                serpentBody.head = leader;
                serpentBody.distanceToHead = space;
                serpentBody.transform.position = transform.position;

                leader = clone.transform;
                space = bodySettings.space;
            }

            // 꼬리가 있으면 꼬리 달아주기
            if (tail)
            {
                _bodyParts.Add(tail);

                if (tail.transform.parent == transform) tail.transform.SetParent(null);

                var serpentBody = tail.AddComponent<SerpentBody>();
                serpentBody.head = clone.transform;
                serpentBody.distanceToHead = tailSpace;
                serpentBody.transform.position = transform.position;
            }

            // ???
            if (bodySettings.prefab.scene.rootCount > 0)
            {
                bodySettings.prefab.SetActive(false);

                //Destroy(bodySettings.prefab);
            }
        }

        void OnDestroy()
        {
            for (int i = 0; i < _bodyParts.Count; i++)
            {
                if (_bodyParts[i]) Destroy(_bodyParts[i]);
            }
        }

        void OnDisable()
        {
            for (int i = 0; i < _bodyParts.Count; i++)
            {
                if (_bodyParts[i]) _bodyParts[i].SetActive(false);
            }
        }

        void OnEnable()
        {
            for (int i = 0; i < _bodyParts.Count; i++)
            {
                if (_bodyParts[i]) _bodyParts[i].SetActive(true);
            }
        }


        void Update()
        {
            // 플레이어를 향하는 위치
            var direction = _targetPosition - transform.position;
            // 플레이어를 향하는 각도
            var angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            // 위에서 구한 각도 반영 및 부드럽게 회전
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.AngleAxis(angle, Vector3.forward), Time.deltaTime * rotationSpeed);

            // transform.Translate은 이동함수
            transform.Translate(Vector3.right * speed * Time.deltaTime);

            // 나와 타겟 사이의 거리가 recalculatedistance(1.0f)보다 작으면 새로운 타겟 잡기
            if (Vector3.Distance(transform.position, _targetPosition) < recalculatedistance)
            {
                GameObject player = GameObject.FindGameObjectWithTag("Player");

                if (tagetPlayer && player)
                {
                    _targetPosition = player.transform.position;
                }
                else
                {
                    // 타겟이 없으면 랜덤 위치 이동
                    _targetPosition = _RandomPosition();
                }
            }
        }

        void _SetStartPosition()
        {
            var size = Vector2.one;
            var spriteRenderer = GetComponent<SpriteRenderer>();
            if (spriteRenderer) size = spriteRenderer.bounds.size;

            var position = _RandomPosition();

            if (startPosition == StartPosition.Left)
            {
                position.x = -bounds.x - size.x / 2;
            }
            else if (startPosition == StartPosition.Right)
            {
                position.x = bounds.x + size.x / 2;
            }
            else if (startPosition == StartPosition.Top)
            {
                position.y = bounds.y + size.y / 2;
            }
            else if (startPosition == StartPosition.Bottom)
            {
                position.y = -bounds.y - size.y / 2;
            }

            transform.position = position;
        }

        Vector3 _RandomPosition()
        {
            var x = Random.Range(-bounds.x, bounds.x);
            var y = Random.Range(-bounds.y, bounds.y);

            return new Vector3(x, y);
        }

        Vector3 _targetPosition;

        List<GameObject> _bodyParts = new List<GameObject>();
    }
}
using UnityEngine;
using Playniax.Ignition.Framework;

namespace Playniax.Pyro.Framework
{
    public class Laser : MonoBehaviour
    {
        public SpriteRenderer beam;
        public SpriteRenderer sourceGlow;
        public SpriteRenderer targetGlow;
        public static void Fire(GameObject prefab, int playerIndex, int orderInLayer, Timer timer, GameObject source, float range = 0, float ttl = .25f, float size = 1, int damage = 1, AudioProperties audioProperties = null, int index = 0)
        {
            if (prefab == null) return;
            if (source == null) return;

            var target = LaserTarget.GetClosest(index, source, range);
            if (target == null) return;

            if (timer.counter == 0) return;

            if (target.gameObject == null) return;
            if (target.isActiveAndEnabled == false) return;

            if (timer.Update() == false) return;

            var laser = Instantiate(prefab).GetComponent<Laser>();
            laser.Fire(playerIndex, orderInLayer, source, target, ttl, size, damage);

            if (audioProperties != null) audioProperties.Play();

            target.scoreBase.Ghost();
        }

        public void Fire(int playerindex, int orderInLayer, GameObject source, LaserTarget target, float ttl = .25f, float size = 1, int damage = 1)
        {
            if (source == null || target == null) return;
            if (source.gameObject == null || target.gameObject == null) return;
            if (source.gameObject.activeSelf == false || target.isActiveAndEnabled == false) return;
            if (target.scoreBase == null) return;
            if (target.scoreBase.isActiveAndEnabled == false) return;

            gameObject.SetActive(true);

            beam.sortingOrder = orderInLayer;
            sourceGlow.sortingOrder = orderInLayer + 1;
            targetGlow.sortingOrder = orderInLayer + 1;

            _playerIndex = playerindex;
            _damage = damage;
            _size = size;
            _source = source;
            _target = target;
            _ttl = ttl;
            _ttlTimer = ttl;

            _target.scoreBase.isTargeted = gameObject;

            _UpdateLaser();
        }

        void LateUpdate()
        {
            _UpdateLaser();
        }

        float _GetAngle(float x1, float y1, float x2, float y2)
        {
            return Mathf.Atan2(y1 - y2, x1 - x2) * Mathf.Rad2Deg;
        }

        void _UpdateLaser()
        {
            if (_source != null && _target != null)
            {
                var angle = _GetAngle(_target.gameObject.transform.position.x, _target.gameObject.transform.position.y, _source.transform.position.x, _source.transform.position.y);

                beam.transform.localRotation = Quaternion.Euler(0, 0, angle);

                transform.GetChild(0).transform.position = _source.transform.position;

                beam.color = _targetColor - (_targetColor - _startColor) * (_ttlTimer / _ttl);

                beam.transform.localScale = new Vector3(Vector3.Distance(_source.transform.position, _target.gameObject.transform.position) / (beam.sprite.rect.width / beam.sprite.pixelsPerUnit), _size, 1);

                sourceGlow.color = new Color(sourceGlow.color.r, sourceGlow.color.g, sourceGlow.color.b, beam.color.a);
                targetGlow.color = new Color(targetGlow.color.r, targetGlow.color.g, targetGlow.color.b, beam.color.a);

                sourceGlow.transform.position = _source.transform.position;
                targetGlow.transform.position = _target.gameObject.transform.position;

                _ttlTimer -= 1 * Time.deltaTime;

                if (_ttlTimer < 0)
                {
                    _target.scoreBase.structuralIntegrity -= _damage;

                    if (_target.scoreBase.structuralIntegrity <= 0)
                    {
                        _target.scoreBase.structuralIntegrity = 0;

                        if (_playerIndex >= 0) PlayerData.Get(_playerIndex).scoreboard += _target.scoreBase.points;

                        //_target.gameSprite.isTargeted = null;

                        _target.scoreBase.Kill();

                        _target = null;
                    }

                    gameObject.SetActive(false);

                    Destroy(gameObject);
                }
            }
            else
            {
                _source = null;
                _target = null;

                gameObject.SetActive(false);

                Destroy(gameObject);
            }

        }

        int _playerIndex = -1;

        int _damage = 1;

        float _size = 1;
        float _ttl = 1;
        float _ttlTimer = 1;

        GameObject _source;
        LaserTarget _target;

        Color _startColor = new Color(1, 1, 1, 1);
        Color _targetColor = new Color(1, 1, 1, 0);
    }
}
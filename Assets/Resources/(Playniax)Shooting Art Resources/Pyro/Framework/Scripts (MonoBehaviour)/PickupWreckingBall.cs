using UnityEngine;
using Playniax.Ignition.Framework;

namespace Playniax.Pyro.Framework
{
    public class PickupWreckingBall : CollisionBase2D
    {
        [System.Serializable]
        public class MessengerSettings
        {
            public string activatedId = "Activated";
            public string reloadedId = "Reloaded";
        }
        public enum Mode { increase, straightToMax };

        public MessengerSettings messengerSettings;
        public string id;
        public GameObject prefab;

        public Mode mode;
        public int increase = 25;
        public int max = 100;

        public float distance = .75f;
        public float speed = 15;
        public string activatedText = "Wrecking Ball Activated At %";
        public string reloadedText = "Wrecking Ball At %";
        public override void Awake()
        {
            base.Awake();

            if (prefab && prefab.scene.rootCount > 0) prefab.SetActive(false);
        }
        public override void OnCollision(CollisionBase2D collision)
        {
            var pickup = collision as Pickup;
            if (pickup == null) return;
            if (pickup.id != id) return;

            if (prefab && _wreckingBall == null)
            {
                _wreckingBall = Instantiate(prefab, transform);

                if (_wreckingBall == null) return;

                _wreckingBall.SetActive(true);

                _scoreBase = _wreckingBall.GetComponent<IScoreBase>();

                if (_scoreBase == null) return;

                if (mode == Mode.increase)
                {
                    _scoreBase.structuralIntegrity = increase;

                    if (Messenger.instance) Messenger.instance.Create(messengerSettings.activatedId, activatedText.Replace("%", MathHelpers.Dif(max, _scoreBase.structuralIntegrity)), collision.transform.position);
                }
                else if (mode == Mode.straightToMax)
                {
                    _scoreBase.structuralIntegrity = max;

                    if (Messenger.instance) Messenger.instance.Create(messengerSettings.activatedId, activatedText.Replace("%", MathHelpers.Dif(max, _scoreBase.structuralIntegrity)), collision.transform.position);
                }
            }
            else if (prefab && _wreckingBall && _scoreBase != null)
            {
                if (mode == Mode.increase)
                {
                    _scoreBase.structuralIntegrity += increase;

                    if (_scoreBase.structuralIntegrity > max) _scoreBase.structuralIntegrity = max;

                    if (Messenger.instance) Messenger.instance.Create(messengerSettings.reloadedId, reloadedText.Replace("%", MathHelpers.Dif(max, _scoreBase.structuralIntegrity)), collision.transform.position);
                }
                else if (mode == Mode.straightToMax)
                {
                    _scoreBase.structuralIntegrity = max;

                    if (Messenger.instance) Messenger.instance.Create(messengerSettings.reloadedId, reloadedText.Replace("%", MathHelpers.Dif(max, _scoreBase.structuralIntegrity)), collision.transform.position);
                }
            }

            Destroy(collision.gameObject);
        }

        void Update()
        {
            _UpdateWreckingBall();
        }
        void _UpdateWreckingBall()
        {
            if (_wreckingBall == null) return;

            var x = Mathf.Cos(_wreckingBallPosition) * distance;
            var y = Mathf.Sin(_wreckingBallPosition) * distance;

            _wreckingBall.transform.localPosition = new Vector3(x, y);

            _wreckingBallPosition += speed * Time.deltaTime;
        }
        IScoreBase _scoreBase;
        GameObject _wreckingBall;
        float _wreckingBallPosition;
    }
}
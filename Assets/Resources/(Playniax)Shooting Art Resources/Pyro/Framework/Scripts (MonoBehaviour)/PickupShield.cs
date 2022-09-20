using UnityEngine;
using Playniax.Ignition.Framework;

namespace Playniax.Pyro.Framework
{
    public class PickupShield : CollisionBase2D
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
        public string activatedText = "Shield Activated At %";
        public string reloadedText = "Shield At %";
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

            if (prefab && _shield == null)
            {
                _shield = Instantiate(prefab, transform);

                if (_shield == null) return;

                _shield.SetActive(true);

                _scoreBase = _shield.GetComponent<IScoreBase>();

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
            else if (prefab && _shield && _scoreBase != null)
            {
                if (mode == Mode.increase)
                {
                    _scoreBase.structuralIntegrity += increase;

                    if (_scoreBase.structuralIntegrity > max) _scoreBase.structuralIntegrity = max;

                    if (Messenger.instance) Messenger.instance.Create(messengerSettings.reloadedId, reloadedText.Replace("%", MathHelpers.Dif(max, _scoreBase.structuralIntegrity)), collision.transform.position);
                }
                else if (mode == Mode.straightToMax && max > 0)
                {
                    _scoreBase.structuralIntegrity = max;

                    if (Messenger.instance) Messenger.instance.Create(messengerSettings.reloadedId, reloadedText.Replace("%", MathHelpers.Dif(max, _scoreBase.structuralIntegrity)), collision.transform.position);
                }
            }

            Destroy(collision.gameObject);
        }

        IScoreBase _scoreBase;
        GameObject _shield;
    }
}
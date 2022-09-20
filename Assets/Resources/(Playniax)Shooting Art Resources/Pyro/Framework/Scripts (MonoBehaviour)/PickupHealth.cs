using Playniax.Ignition.Framework;

namespace Playniax.Pyro.Framework
{
    public class PickupHealth : CollisionBase2D
    {
        [System.Serializable]
        public class MessengerSettings
        {
            public string genericId = "Generic";
        }
        public enum Mode { increase, straightToMax };

        public MessengerSettings messengerSettings;
        public string id;
        public Mode mode;
        public int increase = 25;
        public int max = 100;
        public string text = "Integrity At %";
        public override void Awake()
        {
            base.Awake();

            if (_scoreBase == null) _scoreBase = GetComponent<IScoreBase>();
        }

        public override void OnCollision(CollisionBase2D collision)
        {
            var pickup = collision as Pickup;
            if (pickup == null) return;
            if (pickup.id != id) return;
            if (_scoreBase == null) return;

            if (mode == Mode.increase)
            {
                _scoreBase.structuralIntegrity += increase;

                if (max > 0 && _scoreBase.structuralIntegrity > max) _scoreBase.structuralIntegrity = max;

                if (Messenger.instance) Messenger.instance.Create(messengerSettings.genericId, text.Replace("%", MathHelpers.Dif(max, _scoreBase.structuralIntegrity)), collision.transform.position);
            }
            else if (mode == Mode.straightToMax && max > 0)
            {
                _scoreBase.structuralIntegrity = max;

                if (Messenger.instance) Messenger.instance.Create(messengerSettings.genericId, text.Replace("%", MathHelpers.Dif(max, _scoreBase.structuralIntegrity)), collision.transform.position);
            }

            Destroy(collision.gameObject);
        }

        IScoreBase _scoreBase;
    }
}
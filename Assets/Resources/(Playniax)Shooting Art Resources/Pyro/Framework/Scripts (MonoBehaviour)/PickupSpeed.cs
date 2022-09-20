using UnityEngine;
using Playniax.Ignition.Framework;

namespace Playniax.Pyro.Framework
{
    public class PickupSpeed : CollisionBase2D
    {
        [System.Serializable]
        public class AdditionalSettings
        {
            public bool autoDecrease;
            public float decrease = .5f;
            public float sustain = 180;
            public int min = 1;

            public string text = "Speed at %";
        }

        [System.Serializable]
        public class MessengerSettings
        {
            public string genericId = "Generic";
        }

        public enum Mode { increase, straightToMax };

        public MessengerSettings messengerSettings;
        public string id;

        public Mode mode;
        public float increase = .5f;
        public int max = 3;

        public string text = "Speed At %";

        public AdditionalSettings additionalSettings;

        public override void Awake()
        {
            base.Awake();

            if (_playerControls == null) _playerControls = GetComponent<PlayerControls>();
        }

        public override void OnCollision(CollisionBase2D collision)
        {
            if (_playerControls == null) return;

            var pickup = collision as Pickup;
            if (pickup == null) return;
            if (pickup.id != id) return;

            if (_playerControls == null) _playerControls = GetComponent<PlayerControls>();
            if (_playerControls == null) return;

            if (mode == Mode.increase)
            {
                _playerControls.speed += increase;

                if (max > 0 && _playerControls.speed > max) _playerControls.speed = max;

                if (Messenger.instance) Messenger.instance.Create(messengerSettings.genericId, text.Replace("%", MathHelpers.Dif(max, _playerControls.speed)), collision.transform.position);
            }
            else if (mode == Mode.straightToMax)
            {
                _playerControls.speed = max;

                if (Messenger.instance) Messenger.instance.Create(messengerSettings.genericId, text.Replace("%", MathHelpers.Dif(max, _playerControls.speed)), collision.transform.position);
            }

            _sustain = 0;

            Destroy(collision.gameObject);
        }

        void Update()
        {
            _UpdateDrecrease();
        }

        void _UpdateDrecrease()
        {
            if (additionalSettings.autoDecrease == false) return;

            if (_playerControls == null) return;

            if (_playerControls.speed < additionalSettings.min)
            {
                _playerControls.speed = additionalSettings.min;

                return;
            }
            else
            {
                if (_playerControls.speed == additionalSettings.min) return;
            }

            if (additionalSettings.sustain > 0)
            {
                _sustain += 1 * Time.deltaTime;

                if (_sustain > additionalSettings.sustain)
                {
                    _playerControls.speed -= additionalSettings.decrease;

                    if (_playerControls.speed < additionalSettings.min) _playerControls.speed = additionalSettings.min;

                    _sustain = 0;

                    if (Messenger.instance) Messenger.instance.Create(messengerSettings.genericId, text.Replace("%", MathHelpers.Dif(max, _playerControls.speed)), transform.position);
                }
            }
        }
        PlayerControls _playerControls;
        float _sustain;
    }
}
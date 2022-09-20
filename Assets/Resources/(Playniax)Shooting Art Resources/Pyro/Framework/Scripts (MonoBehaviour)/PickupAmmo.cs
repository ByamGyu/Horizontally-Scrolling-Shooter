using System.Collections.Generic;
using Playniax.Ignition.Framework;

namespace Playniax.Pyro.Framework
{
    public class PickupAmmo : CollisionBase2D
    {
        [System.Serializable]
        public class MessengerSettings
        {
            public string activatedId = "Activated";
            public string deactivatedId = "Deactivated";
            public string reloadedId = "Reloaded";
        }

        public enum Mode { increase, straightToMax };

        public MessengerSettings messengerSettings;
        public string id;
        public BulletSpawner[] spawners;
        public string spawnerId;

        public Mode mode;
        public int increase = 250;
        public int max = 1000;

        public string activatedText = "Gun Activated At %";
        public string deactivatedText = "Gun Deactivated";
        public string reloadedText = "Gun At %";
        public override void Awake()
        {
            base.Awake();

            if ((spawners == null || spawners.Length == 0) && spawnerId != "") spawners = _GetSpawners(spawnerId);
        }
        public override void OnCollision(CollisionBase2D collision)
        {
            var pickup = collision as Pickup;
            if (pickup == null) return;
            if (pickup.id != id) return;

            if (mode == Mode.increase)
            {
                _Message(_GetCounters() / spawners.Length + increase);

                for (int i = 0; i < spawners.Length; i++)
                {
                    spawners[i].timer.counter += increase;

                    if (spawners[i].timer.counter > max) spawners[i].timer.counter = max;
                }
            }
            else if (mode == Mode.straightToMax)
            {
                _Message(_GetCounters() / spawners.Length + increase);

                for (int i = 0; i < spawners.Length; i++)
                {
                    spawners[i].timer.counter = max;
                }
            }

            void _Message(int increasedCounter)
            {
                if (Messenger.instance == null) return;

                if (increasedCounter > max) increasedCounter = max;

                if (_GetCounters() == 0)
                {
                    Messenger.instance.Create(messengerSettings.activatedId, activatedText.Replace("%", MathHelpers.Dif(max, increasedCounter)), collision.transform.position);
                }
                else
                {
                    Messenger.instance.Create(messengerSettings.reloadedId, reloadedText.Replace("%", MathHelpers.Dif(max, increasedCounter)), collision.transform.position);
                }
            }

            Destroy(collision.gameObject);
        }
        void Update()
        {
            if (_AllCountersReachedZeroState() == true && Messenger.instance) Messenger.instance.Create(messengerSettings.deactivatedId, deactivatedText, transform.position);
        }
        bool _AllCountersReachedZeroState()
        {
            for (int i = 0; i < spawners.Length; i++)
            {
                if (spawners[i].timer.GetCounterZeroState() == false) return false;
            }
            return true;
        }
        int _GetCounters()
        {
            var counters = 0;

            for (int i = 0; i < spawners.Length; i++)
            {
                if (spawners[i].timer.counter > 0) counters += spawners[i].timer.counter;
            }
            return counters;
        }
        BulletSpawner[] _GetSpawners(string id)
        {
            var spawners = GetComponentsInChildren<BulletSpawner>();
            var bulletSpawners = new List<BulletSpawner>();
            for (int i = 0; i < spawners.Length; i++)
            {
                if (spawners[i].id == id) bulletSpawners.Add(spawners[i]);
            }
            return bulletSpawners.ToArray();
        }
    }
}
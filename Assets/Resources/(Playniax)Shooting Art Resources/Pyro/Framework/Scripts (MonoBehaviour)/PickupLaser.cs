using Playniax.Ignition.Framework;

namespace Playniax.Pyro.Framework
{
    public class PickupLaser : CollisionBase2D
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
        public LaserSpawner spawner;
        public string spawnerId;

        public Mode mode;
        public int increase = 50;
        public int max = 200;

        public string activatedText = "Laser Activated At %";
        public string deactivatedText = "Laser Deactivated";
        public string reloadedText = "Laser At %";
        public override void Awake()
        {
            base.Awake();

            if ((spawner == null) && spawnerId != "") spawner = _GetSpawner(spawnerId);
            if (spawner == null) print("not found!");
        }

        public override void OnCollision(CollisionBase2D collision)
        {
            var pickup = collision as Pickup;
            if (pickup == null) return;
            if (pickup.id != id) return;

            if (mode == Mode.increase)
            {
                _Message(spawner.timer.counter + increase);

                spawner.timer.counter += increase;

                if (spawner.timer.counter > max) spawner.timer.counter = max;
            }
            else if (mode == Mode.straightToMax)
            {
                _Message(max);

                spawner.timer.counter = max;
            }

            void _Message(int increasedCounter)
            {
                if (Messenger.instance == null) return;

                if (increasedCounter > max) increasedCounter = max;

                if (spawner.timer.counter == 0)
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
            if (spawner && spawner.timer.GetCounterZeroState() == true) Messenger.instance.Create(messengerSettings.deactivatedId, deactivatedText, transform.position);
        }
        LaserSpawner _GetSpawner(string id)
        {
            var spawners = GetComponentsInChildren<LaserSpawner>();
            for (int i = 0; i < spawners.Length; i++)
            {
                if (spawners[i].id == id) return spawners[i];
            }
            return null;
        }
    }
}

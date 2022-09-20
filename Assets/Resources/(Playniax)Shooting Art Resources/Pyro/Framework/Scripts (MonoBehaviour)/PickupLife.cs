using Playniax.Ignition.Framework;

namespace Playniax.Pyro.Framework
{
    public class PickupLife : CollisionBase2D
    {
        [System.Serializable]
        public class MessengerSettings
        {
            public string genericId = "Generic";
        }

        public MessengerSettings messengerSettings;
        public string id;
        public int playerIndex;
        public string text = "One Up";

        public override void OnCollision(CollisionBase2D collision)
        {
            var pickup = collision as Pickup;
            if (pickup == null) return;
            if (pickup.id != id) return;

            PlayerData.Get(playerIndex).lives += 1;

            if (Messenger.instance) Messenger.instance.Create(messengerSettings.genericId, text, collision.transform.position);

            Destroy(collision.gameObject);
        }
    }
}
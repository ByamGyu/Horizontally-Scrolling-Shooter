using Playniax.Ignition.Framework;

namespace Playniax.Pyro.Framework
{
    public class PickupNuke : CollisionBase2D
    {
        public string id;
        public string emitter = "Nuke";

        public override void OnCollision(CollisionBase2D collision)
        {
            var pickup = collision as Pickup;
            if (pickup == null) return;
            if (pickup.id != id) return;

            Nukeable.Nuke();

            TaskBase.Play(emitter, default);

            Destroy(collision.gameObject);
        }
    }
}
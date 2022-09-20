using UnityEngine;
using Playniax.Ignition.Framework;

namespace Playniax.ParticleSystem
{
    public class EmitterGroup : TaskBase
    {

        public override void Play(Vector3 position, Transform parent = null, float scale = 1, int sortingOrder = 0)
        {
            for (int i = 0; i < _emitters.Length; i++)
            {
                _emitters[i].Play(position, parent, scale * this.scale, sortingOrder);
            }
        }
        public override void Start()
        {
            base.Start();

            _emitters = GetComponents<Emitter>();
        }
        public override void OnDestroy()
        {
            base.OnDestroy();
        }

        Emitter[] _emitters;
    }
}
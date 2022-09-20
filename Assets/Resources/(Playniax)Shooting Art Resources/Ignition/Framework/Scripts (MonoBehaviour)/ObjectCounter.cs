using UnityEngine;

namespace Playniax.Ignition.Framework
{
    [AddComponentMenu("Playniax/Ignition/Framework/ObjectCounter")]
    public class ObjectCounter : MonoBehaviour
    {
        public static int objects;
        void OnEnable()
        {
            objects += 1;
        }
        void OnDisable()
        {
            objects -= 1;
        }
    }
}
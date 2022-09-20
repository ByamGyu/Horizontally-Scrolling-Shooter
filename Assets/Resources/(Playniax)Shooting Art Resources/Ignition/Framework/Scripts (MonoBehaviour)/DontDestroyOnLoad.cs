using UnityEngine;

namespace Playniax.Ignition.Framework
{
    namespace Playniax.Ignition.Framework
    {
        [AddComponentMenu("Playniax/Ignition/Framework/DontDestroyOnLoad")]
        public class DontDestroyOnLoad : MonoBehaviour
        {
            public bool dontDestroyOnLoad = true;

            void Awake()
            {
                if (dontDestroyOnLoad) DontDestroyOnLoad(gameObject);
            }
        }
    }
}
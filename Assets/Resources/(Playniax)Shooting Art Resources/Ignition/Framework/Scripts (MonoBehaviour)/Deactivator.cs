using UnityEngine;

namespace Playniax.Ignition.Framework
{
    [AddComponentMenu("Playniax/Ignition/Framework/Deactivator")]
    public class Deactivator : MonoBehaviour
    {
        void OnEnable()
        {
            gameObject.SetActive(false);
        }
    }
}

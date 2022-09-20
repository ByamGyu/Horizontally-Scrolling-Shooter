using UnityEngine;

namespace Playniax.Ignition.Framework
{
    [AddComponentMenu("Playniax/Ignition/Framework/ProgressCounter")]
    public class ProgressCounter : MonoBehaviour
    {
        public virtual void Awake()
        {
            SpawnerBase.ProgressCounter.Reset();
        }

        void OnEnable()
        {
            if (_progressCounter == null) _progressCounter = GetComponent<SpawnerBase.ProgressCounter>();

            if (_progressCounter == null)
            {
                SpawnerBase.ProgressCounter.resetCounter++;

                GameData.progress += 1;
                GameData.progressScale += 1;
            }
        }

        void OnDisable()
        {
            if (_progressCounter == null)
            {
                SpawnerBase.ProgressCounter.resetCounter--;

                GameData.progress -= 1;
            }
        }

        SpawnerBase.ProgressCounter _progressCounter;
    }
}
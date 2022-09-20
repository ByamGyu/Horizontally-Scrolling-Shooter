using UnityEngine;

namespace Playniax.Ignition.Framework
{
    public class SpawnerBase : MonoBehaviour
    {
        public class ProgressCounter : MonoBehaviour
        {
            public static int resetCounter;

            public static void Reset()
            {
                if (resetCounter == 0)
                {
                    GameData.progress = 0;
                    GameData.progressScale = 0;
                }
            }

            void OnDisable()
            {
                GameData.progress -= 1;
            }
        }

        public static int spawners;

        public virtual void Awake()
        {
            ProgressCounter.Reset();
        }

        public virtual void OnEnable()
        {
            ProgressCounter.resetCounter++;

            spawners++;
        }
        public virtual void OnDisable()
        {
            ProgressCounter.resetCounter--;

            spawners--;
        }
    }
}
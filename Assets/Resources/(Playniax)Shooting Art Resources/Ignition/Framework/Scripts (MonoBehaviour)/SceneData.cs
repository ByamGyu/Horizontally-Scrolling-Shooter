using UnityEngine;

namespace Playniax.Ignition.Framework
{
    public class SceneData : MonoBehaviour
    {
        public static SceneData instance
        {
            get
            {
                if (_instance == null) _instance = new GameObject("Scene Data").AddComponent<SceneData>();

                return _instance;
            }
        }

        public Config scene = new Config();

        public Config[] player;

        void OnDestroy()
        {
            _instance = null;
        }

        public Config Get(int index)
        {
            if (index < 0) return null;
            if (instance.player == null) System.Array.Resize(ref instance.player, index + 1);
            if (index >= instance.player.Length) System.Array.Resize(ref instance.player, index + 1);
            if (instance.player[index] == null) instance.player[index] = new Config();

            return player[index];
        }

        static SceneData _instance;
    }
}

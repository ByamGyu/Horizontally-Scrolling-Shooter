using UnityEngine;
using UnityEngine.UI;
using Playniax.Ignition.Framework;

namespace Playniax.Ignition.UI
{
    // Shows game progress based on spawner and sequencer state.
    public class GameProgressUI : MonoBehaviour
    {
        public int format;
        public string prefix;
        public string suffix = "%";
        public Text text;

        void Awake()
        {
            if (text == null) text = GetComponent<Text>();
        }

        void Update()
        {
            if (GameData.progressScale == 0) return;

            var count = "0";

            if (GameData.progressScale > 0) count = ((int)(100f - ((float)GameData.progress / GameData.progressScale * 100))).ToString();

            if (format > 0) count = StringHelpers.Format(count, format);

            if (prefix != "") count = prefix + count;
            if (suffix != "") count += suffix;

            text.text = count;
        }
    }
}
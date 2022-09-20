using UnityEngine;
using UnityEngine.UI;
using Playniax.Ignition.Framework;

namespace Playniax.Ignition.UI
{
    public class ScoreboardUI : MonoBehaviour
    {
        public int playerIndex;
        public int format = 8;
        public string prefix;
        public string suffix;
        public Text text;

        public bool update = true;
        void Awake()
        {
            if (text == null) text = GetComponent<Text>();
        }
        void OnEnable()
        {
            _Update();
        }
        void Update()
        {
            if (update) _Update();
        }
        void _Update()
        {
            var count = PlayerData.Get(playerIndex).scoreboard.ToString();

            if (format > 0) count = StringHelpers.Format(count, format);

            if (prefix != "") count = prefix + count;
            if (suffix != "") count += suffix;

            text.text = count;
        }
    }
}
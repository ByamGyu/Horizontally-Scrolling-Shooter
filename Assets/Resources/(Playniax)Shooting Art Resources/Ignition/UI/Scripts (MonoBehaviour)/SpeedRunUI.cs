using UnityEngine;
using UnityEngine.UI;
using Playniax.Ignition.Framework;

namespace Playniax.Ignition.UI
{
    public class SpeedRunUI : MonoBehaviour
    {
        public Text text;

        void Awake()
        {
            if (text == null) text = GetComponent<Text>();

            if (text != null) text.text = GameData.GetSpeedRun();
        }

        void Update()
        {
            if (text != null) text.text = GameData.GetSpeedRun();
        }
    }
}
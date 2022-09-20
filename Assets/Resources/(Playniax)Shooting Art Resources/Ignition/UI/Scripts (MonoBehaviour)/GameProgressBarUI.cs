using UnityEngine;
using Playniax.Ignition.Framework;

namespace Playniax.Ignition.UI
{
    public class GameProgressBarUI : MonoBehaviour
    {
        public enum Mode { Horizontal, Vertical };

        public Mode mode;
        public bool selectedPlayerOnly = true;
        public bool reverse;

        void Awake()
        {
            _transform = GetComponent<Transform>();

            _Update();
        }

        void Update()
        {
            _Update();
        }

        void _Update()
        {
            if (_transform == null) return;

            var progressScale = GameData.progressScale;
            if (progressScale == 0) return;

            var progress = GameData.progress;

            if (reverse) progress = progressScale - progress;

            var scale = 1.0f / progressScale * progress;

            if (scale < 0) scale = 0;
            if (scale > 1) scale = 1;

            if (mode == Mode.Horizontal)
            {
                _transform.localScale = new Vector3(scale, _transform.localScale.y);
            }
            else
            {
                _transform.localScale = new Vector3(_transform.localScale.x, scale);
            }
        }

        Transform _transform;
    }
}
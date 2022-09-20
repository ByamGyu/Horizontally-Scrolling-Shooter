using UnityEngine;
using Playniax.Ignition.Framework;
using Playniax.Pyro.Framework;

namespace Playniax.Pyro.UI
{
    public class HealthBarUI : MonoBehaviour
    {
        public enum Mode { Horizontal, Vertical };

        public string id = "Player 1";
        public Mode mode;
        public int maximum;
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
        int _GetStructuralIntegrity(string id, bool selectedPlayerOnly)
        {
            int count = 0;

            var list = PlayersGroup.GetList();
            if (list == null) return 0;

            for (int i = 0; i < list.Count; i++)
            {
                if (list[i] && list[i].id == id && list[i].gameObject)
                {
                    //if (selectedPlayerOnly && list[i] && list[i].gameObject != PlayersGroup.GetSelected()) continue;

                    var scoreBase = list[i].GetComponent<IScoreBase>();
                    if (scoreBase != null) count += scoreBase.structuralIntegrity;
                }
            }

            return count;
        }
        void _Update()
        {
            if (_transform == null) return;
            if (maximum == 0) maximum += _GetStructuralIntegrity(id, selectedPlayerOnly);
            if (maximum == 0) return;

            var value = _GetStructuralIntegrity(id, selectedPlayerOnly);

            if (reverse) value = maximum - value;

            var scale = 1.0f / maximum * value;

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
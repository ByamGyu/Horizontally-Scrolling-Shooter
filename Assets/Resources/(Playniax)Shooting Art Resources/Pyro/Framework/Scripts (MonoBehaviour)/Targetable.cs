using System.Collections.Generic;
using UnityEngine;

namespace Playniax.Pyro.Framework
{
    public class Targetable : MonoBehaviour
    {
        public static List<Targetable> list = new List<Targetable>();

        static List<Targetable> _targets = new List<Targetable>();

        public IScoreBase scoreBase;

        public static Targetable GetClosest(GameObject source, bool toughestFirst = false, float targetRange = 0)
        {
            if (list.Count == 0) return null;

            if (source == null) return null;
            if (source.activeInHierarchy == false) return null;

            for (int i = 0; i < list.Count; i++)
            {
                if (list[i] == null) continue;
                if (list[i].gameObject == null) continue;
                if (list[i].isActiveAndEnabled == false) continue;
                if (list[i].scoreBase == null) continue;
                if (list[i].scoreBase.isVisible == false) continue;
                if (list[i].scoreBase.isTargeted != null) continue;
                if (_InRange(source, list[i].gameObject, targetRange) == false) continue;

                _targets.Add(list[i]);
            }

            if (_targets.Count == 0) return null;

            Targetable target = null;

            if (_targets.Count > 0) target = _targets[0];

            if (toughestFirst)
            {
                for (int i = 0; i < _targets.Count; i++)
                {
                    if (_targets[i].scoreBase.structuralIntegrity > target.scoreBase.structuralIntegrity && _targets[i] != target && _targets[i].gameObject.activeInHierarchy == true && Vector3.Distance(source.transform.position, _targets[i].gameObject.transform.position) < Vector3.Distance(source.transform.position, target.gameObject.transform.position)) target = _targets[i];
                }
            }
            else
            {
                for (int i = 0; i < _targets.Count; i++)
                {
                    if (Vector3.Distance(source.transform.position, _targets[i].gameObject.transform.position) < Vector3.Distance(source.transform.position, target.gameObject.transform.position)) target = _targets[i];
                }
            }

            _targets.Clear();

            return target;
        }

        static bool _InRange(GameObject a, GameObject b, float range)
        {
            if (range == 0) return true;

            var distance = Vector3.Distance(a.transform.position, b.transform.position);
            if (distance > range) return false;

            return true;
        }
        void Awake()
        {
            if (scoreBase == null) scoreBase = GetComponent<IScoreBase>();
        }
        void OnEnable()
        {
            if (scoreBase != null) list.Add(this);
        }

        void OnDisable()
        {
            list.Remove(this);
        }
    }
}
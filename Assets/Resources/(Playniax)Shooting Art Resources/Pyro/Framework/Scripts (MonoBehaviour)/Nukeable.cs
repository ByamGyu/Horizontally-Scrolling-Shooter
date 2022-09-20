using System.Collections.Generic;
using UnityEngine;

namespace Playniax.Pyro.Framework
{
    public class Nukeable : MonoBehaviour
    {
        public static List<Nukeable> list = new List<Nukeable>();

        public static void Nuke()
        {
            if (list.Count == 0) return;

            var nukables = new List<Nukeable>();

            for (int i = 0; i < list.Count; i++)
            {
                if (list[i] != null && list[i].gameObject && list[i].enabled == true) nukables.Add(list[i]);
            }

            for (int i = 0; i < nukables.Count; i++)
            {
                if (nukables[i] != null) Destroy(nukables[i].gameObject);
            }
        }

        void OnEnable()
        {
            _scoreBase = GetComponent<IScoreBase>();

            if (_scoreBase != null) list.Add(this);
        }

        void OnDisable()
        {
            if (_scoreBase != null) list.Remove(this);
        }

        IScoreBase _scoreBase;
    }
}
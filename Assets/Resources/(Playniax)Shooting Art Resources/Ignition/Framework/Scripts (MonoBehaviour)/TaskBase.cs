using System.Collections.Generic;
using UnityEngine;

namespace Playniax.Ignition.Framework
{
    // A TaskBase based call resides dynamically in memory can can be called by any script.
    public class TaskBase : MonoBehaviour
    {
        // Task id.
        public string id;
        // Task scale.
        public float scale = 1;
        // Task size.
        public int size;
        public static TaskBase Get(string id)
        {
            for (int i = 0; i < _tasks.Count; i++)
            {
                if (_tasks[i].id == id) return _tasks[i];
            }
            return null;
        }
        public static void Play(string id, Vector3 position, Transform parent = null, float scale = 1, int sortingOrder = 0)
        {
            for (int i = 0; i < _tasks.Count; i++)
            {
                if (_tasks[i].id == id) _tasks[i].Play(position, parent, scale * _tasks[i].scale, sortingOrder);
            }
        }

        public virtual void Play(Vector3 position, Transform parent = null, float scale = 1, int sortingOrder = 0)
        {
        }
        public virtual void Start()
        {
            _tasks.Add(this);
        }
        public virtual void OnDestroy()
        {
            _tasks.Remove(this);
        }

        static List<TaskBase> _tasks = new List<TaskBase>();
    }
}


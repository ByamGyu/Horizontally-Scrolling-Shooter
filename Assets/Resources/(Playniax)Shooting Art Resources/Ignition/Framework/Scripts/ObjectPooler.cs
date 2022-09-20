using System.Collections.Generic;
using UnityEngine;

namespace Playniax.Ignition.Framework
{
    [System.Serializable]
    // Instantiate() and Destroy() are useful and necessary methods during gameplay. Each generally requires minimal CPU time. However, for objects created during gameplay that have a short lifespan and get destroyed in vast numbers per second, the CPU needs to allocate considerably more time. Additionally, Unity uses Garbage Collection to deallocate memory that’s no longer in use. Repeated calls to Destroy() frequently trigger this task, and it has a knack for slowing down CPUs and introducing pauses to gameplay. This behavior is critical in resource-constrained environments such as mobile devices and web builds. Object pooling is where you pre-instantiate all the objects you’ll need at any specific moment before gameplay — for instance, during a loading screen. Instead of creating new objects and destroying old ones during gameplay, your game reuses objects from a 'pool'.
    public class ObjectPooler
    {
        public const string marker = "(Pooled Game Object)";

        // The object to pre-instantiate.
        public GameObject prefab;
        // Number of objects to pre-instantiate.
        public int initialize;

        // Returns the first available object.
        public GameObject GetAvailableObject(bool allowGrowth = true)
        {
            if (prefab == null) return null;

            if (initialize > 0 && _pool.Count == 0) Init();

            for (int i = 0; i < _pool.Count; i++)
            {
                if (_pool[i] && _pool[i].activeInHierarchy == false)
                {
                    var o = _pool[i];
                    o.SetActive(false);
                    return o;
                }
            }

            if (allowGrowth == false) return null;

            var n = Object.Instantiate(prefab);
            n.SetActive(false);
            n.name = n.name.Replace("(Clone)", marker);
            _pool.Add(n);
            return n;
        }

        public void Init()
        {
            if (prefab == null) return;

            if (prefab.scene.rootCount > 0) prefab.SetActive(false);

            for (int i = 0; i < initialize; i++)
            {
                var obj = Object.Instantiate(prefab);
                obj.SetActive(false);
                obj.name = obj.name.Replace("(Clone)", marker);
                _pool.Add(obj);
            }
        }

        List<GameObject> _pool = new List<GameObject>();
    }
}
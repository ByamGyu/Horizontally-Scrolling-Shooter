using System.Collections.Generic;
using UnityEngine;

namespace Playniax.Ignition.Framework
{
    public class Register : MonoBehaviour
    {
        public static List<GameObject> register = new List<GameObject>();

        public static GameObject GetGameObject(string name)
        {
            for (int i = 0; i < register.Count; i++)
            {
                if (register[i].name == name) return register[i];
            }
            return null;
        }

        void OnEnable()
        {
            register.Add(gameObject);
        }

        void OnDisable()
        {
            register.Remove(gameObject);
        }
    }
}
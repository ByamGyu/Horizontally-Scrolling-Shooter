using UnityEditor;
using UnityEngine;

namespace Playniax.ParticleSystem
{
    [CustomEditor(typeof(EmitterGroup))]
    public class EmitterGroupEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();

            EmitterGroup myScript = (EmitterGroup)target;
            if (GUILayout.Button("Test"))
            {
                myScript.Play(Vector3.zero, myScript.transform.parent, 1, 0); ;
            }
        }
    }
}
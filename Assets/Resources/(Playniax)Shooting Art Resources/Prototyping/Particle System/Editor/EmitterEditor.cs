using UnityEditor;
using UnityEngine;

namespace Playniax.ParticleSystem
{
    [CustomEditor(typeof(Emitter))]
    public class EmitterEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();

            Emitter myScript = (Emitter)target;
            if (GUILayout.Button("Test"))
            {
                myScript.Play(Vector3.zero, myScript.transform.parent, 1, 0);
            }
        }
    }
}
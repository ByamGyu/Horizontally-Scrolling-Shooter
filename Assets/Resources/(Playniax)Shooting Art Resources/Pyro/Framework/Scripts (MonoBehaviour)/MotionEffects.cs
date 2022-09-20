#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;

namespace Playniax.Pyro.Framework
{
    [AddComponentMenu("Playniax/Pyro/Framework/MotionEffects")]
    // Misc motion effects with support for rotating and linear motions.
    public class MotionEffects : MonoBehaviour
    {
#if UNITY_EDITOR
        [CanEditMultipleObjects]
        [CustomEditor(typeof(MotionEffects))]
        public class Inspector : Editor
        {
            SerializedProperty linearSettings;
            SerializedProperty rotateSettings;
            void OnEnable()
            {
                linearSettings = serializedObject.FindProperty("linearSettings");
                rotateSettings = serializedObject.FindProperty("rotateSettings");
            }
            override public void OnInspectorGUI()
            {
                EditorGUI.BeginChangeCheck();

                serializedObject.Update();

                var myScript = target as MotionEffects;

                EditorGUI.BeginDisabledGroup(true);
                EditorGUILayout.ObjectField("Script", MonoScript.FromMonoBehaviour(myScript), typeof(MotionEffects), false);
                EditorGUI.EndDisabledGroup();

                myScript.mode = (Mode)EditorGUILayout.EnumPopup("Mode", myScript.mode);

                if (myScript.mode == Mode.Linear)
                {
                    EditorGUILayout.PropertyField(linearSettings, new GUIContent("Linear Settings"));
                }
                else if (myScript.mode == Mode.Rotate)
                {
                    EditorGUILayout.PropertyField(rotateSettings, new GUIContent("Rotate Settings"));
                }

                if (EditorGUI.EndChangeCheck())
                {
                    EditorUtility.SetDirty(myScript);

                    serializedObject.ApplyModifiedProperties();
                }
            }
        }
#endif

        [System.Serializable]
        // Settings linear mode.
        public class LinearSettings
        {
            // Velocity.
            public Vector3 velocity = new Vector3(1, 0, 0);
            // Friction.
            public float friction;

            public void Update(MotionEffects instance)
            {
                instance.transform.position += velocity * Time.deltaTime;

                if (friction != 0) velocity *= 1 / (1 + (Time.deltaTime * friction));
            }
        }

        [System.Serializable]
        // Settings rotate mode.
        public class RotateSettings
        {
            // Rotation speed.
            public Vector3 rotation = new Vector3(0, 0, 100);
            // Determines if the object starts from a random angle or not.
            public bool startRandom;

            public void Update(MotionEffects instance)
            {
                instance.transform.Rotate(rotation * Time.deltaTime);
            }
        }
        public enum Mode { Linear, Rotate };

        // Mode can be Mode.Linear or Mode.Rotate.
        public Mode mode = Mode.Linear;
        // Settings linear mode.
        public LinearSettings linearSettings;
        // Settings rotate mode.
        public RotateSettings rotateSettings;

        void Awake()
        {
            if (mode == Mode.Linear)
            {
            }
            else if (mode == Mode.Rotate)
            {
                if (rotateSettings.startRandom == true) transform.Rotate(0, 0, Random.Range(0, 359));
            }
        }

        void FixedUpdate()
        {
            if (mode == Mode.Linear)
            {
                linearSettings.Update(this);
            }
            else if (mode == Mode.Rotate)
            {
                rotateSettings.Update(this);
            }
        }
    }
}
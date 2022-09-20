#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;

namespace Playniax.Pyro.Framework
{
    [AddComponentMenu("Playniax/Pyro/Framework/AlphaEffects")]
    // Sprite alpha effects with support for fade in, fade out and ping pong mode.
    public class AlphaEffects : MonoBehaviour
    {
#if UNITY_EDITOR
        [CanEditMultipleObjects]
        [CustomEditor(typeof(AlphaEffects))]
        public class Inspector : Editor
        {
            SerializedProperty fadeInSettings;
            SerializedProperty fadeOutSettings;
            SerializedProperty pingPongSettings;
            void OnEnable()
            {
                fadeInSettings = serializedObject.FindProperty("fadeInSettings");
                fadeOutSettings = serializedObject.FindProperty("fadeOutSettings");
                pingPongSettings = serializedObject.FindProperty("pingPongSettings");
            }
            override public void OnInspectorGUI()
            {
                EditorGUI.BeginChangeCheck();

                serializedObject.Update();

                var myScript = target as AlphaEffects;

                EditorGUI.BeginDisabledGroup(true);
                EditorGUILayout.ObjectField("Script", MonoScript.FromMonoBehaviour(myScript), typeof(AlphaEffects), false);
                EditorGUI.EndDisabledGroup();

                myScript.mode = (Mode)EditorGUILayout.EnumPopup("Mode", myScript.mode);

                if (myScript.mode == Mode.PingPong)
                {
                    EditorGUILayout.PropertyField(pingPongSettings, new GUIContent("PingPong Settings"));
                }
                else if (myScript.mode == Mode.FadeIn)
                {
                    EditorGUILayout.PropertyField(fadeInSettings, new GUIContent("Fade In Settings"));
                }
                else if (myScript.mode == Mode.FadeOut)
                {
                    EditorGUILayout.PropertyField(fadeOutSettings, new GUIContent("Fade Out Settings"));
                }

                myScript.spriteRenderer = (SpriteRenderer)EditorGUILayout.ObjectField("Sprite Renderer", myScript.spriteRenderer, typeof(SpriteRenderer), true);

                if (EditorGUI.EndChangeCheck())
                {
                    EditorUtility.SetDirty(myScript);

                    serializedObject.ApplyModifiedProperties();
                }
            }

        }
#endif
        [System.Serializable]
        // Fade in datatype.
        public class FadeInSettings
        {
            // Fade speed.
            public float speed = 1;
        }

        [System.Serializable]
        // Fade out datatype.
        public class FadeOutSettings
        {
            // Fade speed.
            public float speed = 1;
        }

        [System.Serializable]
        // Ping pong datatype.
        public class PingPongSettings
        {
            // Fade speed.
            public float speed = 1;
            // Lowest possible value.
            public float min = .25f;
            // Highest possible value.
            public float max = 1;
            // Whether to start at random value or not.
            public bool startRandom;
        }

        public enum Mode { PingPong, FadeOut, FadeIn }

        // Mode can be Mode.PingPong, Mode.FadeOut or Mode.FadeIn
        public Mode mode = Mode.PingPong;
        // Have a look at the fade in datatype for settings.
        public FadeInSettings fadeInSettings;
        // Have a look at the fade out datatype for settings.
        public FadeOutSettings fadeOutSettings;
        // Have a look at the ping pong datatype for settings.
        public PingPongSettings pingPongSettings;
        // spriteRenderer AlphaEffects is using.
        SpriteRenderer spriteRenderer;

        void Awake()
        {
            if (spriteRenderer == null) spriteRenderer = GetComponent<SpriteRenderer>();

            if (mode == Mode.PingPong)
            {
                if (pingPongSettings.startRandom)
                {
                    var color = spriteRenderer.color;

                    color.a = Random.Range(pingPongSettings.min, pingPongSettings.max);

                    if (color.a < pingPongSettings.min)
                    {
                        color.a = pingPongSettings.min;

                        pingPongSettings.speed = Mathf.Abs(pingPongSettings.speed);
                    }

                    if (color.a > pingPongSettings.max)
                    {
                        color.a = pingPongSettings.max;

                        pingPongSettings.speed = -Mathf.Abs(pingPongSettings.speed);
                    }

                    spriteRenderer.color = color;
                }
            }
            else if (mode == Mode.FadeIn)
            {
                var color = spriteRenderer.color;
                color.a = 0;
                spriteRenderer.color = color;
            }

        }

        void Update()
        {
            if (mode == Mode.PingPong)
            {
                _UpdatePingPong();
            }
            else if (mode == Mode.FadeOut)
            {
                _UpdateFadeOut();
            }
            else if (mode == Mode.FadeIn && spriteRenderer.color.a != 1)
            {
                _UpdateFadeIn();
            }
        }

        void _UpdateFadeIn()
        {
            var color = spriteRenderer.color;

            color.a += fadeOutSettings.speed * Time.deltaTime;

            if (color.a > 1)
            {
                color.a = 1;

                spriteRenderer.color = color;
            }
            else
            {
                spriteRenderer.color = color;
            }
        }

        void _UpdateFadeOut()
        {
            var color = spriteRenderer.color;

            color.a -= fadeOutSettings.speed * Time.deltaTime;

            if (color.a < 0)
            {
                color.a = 0;

                spriteRenderer.color = color;

                Destroy(gameObject);
            }
            else
            {
                spriteRenderer.color = color;
            }
        }

        void _UpdatePingPong()
        {
            var color = spriteRenderer.color;

            color.a += pingPongSettings.speed * Time.deltaTime;

            if (color.a < pingPongSettings.min)
            {
                color.a = pingPongSettings.min;

                pingPongSettings.speed = Mathf.Abs(pingPongSettings.speed);
            }

            if (color.a > pingPongSettings.max)
            {
                color.a = pingPongSettings.max;

                pingPongSettings.speed = -Mathf.Abs(pingPongSettings.speed);
            }

            spriteRenderer.color = color;
        }
    }
}
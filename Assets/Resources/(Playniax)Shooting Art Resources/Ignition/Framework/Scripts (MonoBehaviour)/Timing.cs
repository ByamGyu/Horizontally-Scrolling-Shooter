using UnityEngine;

// https://docs.unity3d.com/ScriptReference/Application-targetFrameRate.html

namespace Playniax.Ignition.Framework
{
    [AddComponentMenu("Playniax/Ignition/Framework/Timing")]
    // Timing initiator to set the targetFrameRate, timeScale, vSyncCount of a scene.
    public class Timing : MonoBehaviour
    {
        // Instructs the game to try to render at a specified frame rate.
        public int targetFrameRate = -1;
        // The scale at which time passes.
        public float timeScale = 1;
        // The VSync Count.
        public int vSyncCount;
#if UNITY_EDITOR
        // Define a runtime pause key for debugging (editor mode only).
        public KeyCode pauseKey = KeyCode.None;
#endif
        // Set or get Paused state.
        public static bool Paused
        {
            get
            {
                if (_timeScale == -1) _timeScale = Time.timeScale;

                if (Time.timeScale == 0) return true;
                return false;
            }
            set
            {
                if (_timeScale == -1) _timeScale = Time.timeScale;

                if (value == true)
                {
                    if (Time.timeScale != 0)
                    {
                        _timeScale = Time.timeScale;

                        Time.timeScale = 0;
                    }
                }
                else
                {
                    Time.timeScale = _timeScale;
                }
            }
        }

        void LateUpdate()
        {
#if UNITY_EDITOR
            if (Input.GetKeyDown(pauseKey)) Timing.Paused = !Timing.Paused;
#endif
        }

        void OnEnable()
        {
            OnValidate();
        }

        void OnDisable()
        {
            Application.targetFrameRate = -1;
            QualitySettings.vSyncCount = 0;
            Time.timeScale = 1;
        }
        void OnValidate()
        {
            Application.targetFrameRate = targetFrameRate;
            QualitySettings.vSyncCount = vSyncCount;
            Time.timeScale = timeScale;
        }

        static float _timeScale = -1;
    }
}
// https://answers.unity.com/questions/45676/making-a-timer-0000-minutes-and-seconds.html

using UnityEngine;

namespace Playniax.Ignition.Framework
{
    // GameData can be used to temporarily store statistics of the game like progress.
    // progress and progressScale are built-in data fields.
    // Custom fields can be added to custom and supports floats, integers, booleans and strings.
    // This information will not be saved and is only avaiable at runtime.
    public class GameData
    {
        public static int bodyCount;
        public static int progress;
        public static int progressScale;
        public static float speedRun;

        // Custom data storage.
        public Config custom = new Config();

        public static string GetSpeedRun()
        {
            if (_unscaledTime == 0) _unscaledTime = Time.unscaledTime;

            System.TimeSpan time = System.TimeSpan.FromSeconds(speedRun);

            if (Time.timeScale > 0) speedRun += Time.unscaledTime - _unscaledTime;

            _unscaledTime = Time.unscaledTime;

            return string.Format("{0:D2}:{1:D2}:{2:D2}", time.Hours, time.Minutes, time.Seconds);
        }

        public static void ResetSpeedRun()
        {
            speedRun = 0;
            _unscaledTime = 0;
        }

        static float _unscaledTime;
    }
}
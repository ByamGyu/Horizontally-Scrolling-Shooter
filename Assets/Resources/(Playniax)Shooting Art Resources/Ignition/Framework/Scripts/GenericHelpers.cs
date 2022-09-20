using UnityEngine;

namespace Playniax.Ignition.Framework
{
    [System.Serializable]
    // The Timer class is used by the spawners for example.
    public class Timer
    {
        // Timer.
        public float timer;
        // Interval.
        public float delay = 0.15f;
        // Countdown. -1 is endless.
        public int counter = -1;
        public bool counterReachedZero;
        public bool GetCounterZeroState()
        {
            var state = counterReachedZero;
            counterReachedZero = false;
            return state;
        }
        // Update will return true when timer reaches zero and resets the timer.
        public bool Update()
        {
            if (counter == 0) return false;

            timer -= Time.deltaTime;

            if (timer >= 0) return false;

            if (counter > 0)
            {
                counter -= 1;
                if (counter == 0) counterReachedZero = true;
            }

            if (counter != 0) timer = delay;

            return true;
        }
    }
}
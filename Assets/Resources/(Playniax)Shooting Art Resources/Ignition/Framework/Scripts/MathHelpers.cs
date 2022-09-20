using UnityEngine;

namespace Playniax.Ignition.Framework
{
    public class MathHelpers
    {
        public static string Dif(float current, float increase, string suffix = "%")
        {
            var increaseInPercentage = (current + increase - current) / current * 100;

            return increaseInPercentage.ToString("F0") + suffix;
        }
        public static Vector3 GetVelocity(Vector3 origin, Vector3 target)
        {
            float x = target.x - origin.x;
            float z = target.z - origin.z;
            float y = target.y - origin.y;

            return new Vector3(x, y, z);
        }

        public static float Mod(float devidend, float devider)
        {
            return devidend - devider * Mathf.Floor(devidend / devider);
        }
    }
}
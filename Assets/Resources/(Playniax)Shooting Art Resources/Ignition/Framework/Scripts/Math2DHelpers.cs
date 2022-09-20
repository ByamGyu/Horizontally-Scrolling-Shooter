using UnityEngine;

namespace Playniax.Ignition.Framework
{
    // Collection of 2d math functions.
    public class Math2DHelpers
    {
        // Returns the angle between objects a & b.
        public static float GetAngle(GameObject a, GameObject b)
        {
            //if (a == null || b == null) return Random.Range(0, 359) * Mathf.Deg2Rad;

            return Mathf.Atan2(a.transform.position.y - b.transform.position.y, a.transform.position.x - b.transform.position.x);
        }
        // Returns whether point is inside the rectangle or not.
        public static bool PointInsideRect(float pointX, float pointY, float x, float y, float width, float height, float pivotX = .5f, float pivotY = .5f)
        {
            x -= width * pivotX;
            y -= height * pivotY;

            var leftX = x;
            var rightX = x + width;
            var topY = y;
            var bottomY = y + height;

            return leftX <= pointX && pointX <= rightX && topY <= pointY && pointY <= bottomY;
        }
    }
}
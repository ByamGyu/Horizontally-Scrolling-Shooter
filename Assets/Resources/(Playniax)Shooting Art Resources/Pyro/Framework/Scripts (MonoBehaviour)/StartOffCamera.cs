using UnityEngine;
using Playniax.Ignition.Framework;

namespace Playniax.Pyro.Framework
{
    [AddComponentMenu("Playniax/Pyro/Framework/StartOffCamera")]
    // Places a sprite just outside the camera view.
    public class StartOffCamera : MonoBehaviour
    {
        public enum StartPosition { Left, Right, Top, Bottom };

        // Can be Left, Right, Top or Bottom.
        public StartPosition startPosition = StartPosition.Left;

        void OnEnable()
        {
            _Init();
        }

        void _Init()
        {
            var size = RendererHelpers.GetSize(gameObject) * .5f;

            var min = Camera.main.ViewportToWorldPoint(new Vector3(0, 1, transform.position.z - Camera.main.transform.position.z));
            var max = Camera.main.ViewportToWorldPoint(new Vector3(1, 0, transform.position.z - Camera.main.transform.position.z));

            min.x -= size.x;
            max.x += size.x;

            min.y += size.y;
            max.y -= size.y;

            var position = transform.position;

            if (startPosition == StartPosition.Left)
            {
                position.x = min.x;
            }
            else if (startPosition == StartPosition.Right)
            {
                position.x = max.x;
            }
            else if (startPosition == StartPosition.Top)
            {
                position.y = min.y;
            }
            else if (startPosition == StartPosition.Bottom)
            {
                position.y = max.y;
            }

            transform.position = position;
        }
    }
}
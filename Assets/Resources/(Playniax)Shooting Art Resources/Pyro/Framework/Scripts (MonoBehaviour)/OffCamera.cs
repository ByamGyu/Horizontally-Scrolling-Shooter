using UnityEngine;
using Playniax.Ignition.Framework;

namespace Playniax.Pyro.Framework
{
    [AddComponentMenu("Playniax/Pyro/Framework/OffCamera")]
    // Destroys sprite off camera or resets it just outside the camera view for looping.
    public class OffCamera : MonoBehaviour
    {
        public enum Mode { Destroy, Loop };
        // Mode can be Mode.Destroy or Mode.Loop
        public Mode mode;
        // A sprite is 'cut off' exactly once it's outside the camera view. With margin you can give it extra space.
        public float margin;
        void LateUpdate()
        {
            _Update();
        }
        void _Update()
        {
            var size = RendererHelpers.GetSize(gameObject) * .5f;

            var min = Camera.main.ViewportToWorldPoint(new Vector3(0, 1, transform.position.z - Camera.main.transform.position.z));
            var max = Camera.main.ViewportToWorldPoint(new Vector3(1, 0, transform.position.z - Camera.main.transform.position.z));

            min.x -= size.x;
            max.x += size.x;

            min.y += size.y;
            max.y -= size.y;

            min.x -= margin;
            max.x += margin;

            min.y += margin;
            max.y -= margin;

            var position = transform.position;

            if (mode == Mode.Destroy && (position.x < min.x || position.x > max.x || position.y < max.y || position.y > min.y))
            {
                gameObject.SetActive(false);

                Destroy(gameObject);
                //if (name.Contains(ObjectPooler.marker) == false) Destroy(gameObject);
                //Destroy(gameObject.transform.root.gameObject);
            }
            else if (mode == Mode.Loop && transform.position.x < min.x)
            {
                position.x = max.x;

                transform.position = position;
            }
            else if (mode == Mode.Loop && transform.position.x > max.x)
            {
                position.x = min.x;

                transform.position = position;
            }
            else if (mode == Mode.Loop && transform.position.y > min.y)
            {
                position.y = max.y;

                transform.position = position;
            }
            else if (mode == Mode.Loop && transform.position.y < max.y)
            {
                position.y = min.y;

                transform.position = position;
            }
        }
    }
}
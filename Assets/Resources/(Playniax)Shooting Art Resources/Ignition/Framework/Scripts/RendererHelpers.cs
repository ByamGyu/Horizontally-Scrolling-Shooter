using UnityEngine;

namespace Playniax.Ignition.Framework
{
    // Collection of renderer functions.
    public class RendererHelpers
    {
        // Returns the bounds of an object with multiple renderers.
        public static Bounds GetBounds(GameObject gameObject)
        {
            Bounds bounds;

            var renderer = gameObject.GetComponent<Renderer>();
            if (renderer != null)
            {
                bounds = renderer.bounds;
            }
            else
            {
                bounds = new Bounds(Vector3.zero, Vector3.zero);
            }

            if (bounds.extents.x == 0)
            {
                bounds = new Bounds(gameObject.transform.position, Vector3.zero);

                foreach (Transform child in gameObject.transform)
                {
                    renderer = child.GetComponent<Renderer>();

                    if (renderer)
                    {
                        bounds.Encapsulate(renderer.bounds);
                    }
                    else
                    {
                        bounds.Encapsulate(GetBounds(child.gameObject));
                    }
                }
            }

            return bounds;
        }
        // Returns the size of an object with multiple renderers.
        public static Vector2 GetSize(GameObject gameObject)
        {
            return GetBounds(gameObject).size;
        }
        public static void IncreaseOrder(GameObject gameObject, int value)
        {
            var renderers = gameObject.GetComponentsInChildren<Renderer>();

            for (int i = 0; i < renderers.Length; i++)
            {
                renderers[i].sortingOrder += value;
            }
        }
        // Sets order in layer.
        public static void SetOrder(GameObject gameObject, int orderInLayer)
        {
            var renderers = gameObject.GetComponentsInChildren<Renderer>();

            for (int i = 0; i < renderers.Length; i++)
            {
                renderers[i].sortingOrder = orderInLayer;
            }
        }
    }
}
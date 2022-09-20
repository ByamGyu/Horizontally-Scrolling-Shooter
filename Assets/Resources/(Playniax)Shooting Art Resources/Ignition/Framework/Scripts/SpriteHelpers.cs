using UnityEngine;

namespace Playniax.Ignition.Framework
{
    public class SpriteHelpers
    {
        public static void SetColor(GameObject gameObject, Color color)
        {
            var spriteRenderers = gameObject.GetComponentsInChildren<SpriteRenderer>();

            for (int i = 0; i < spriteRenderers.Length; i++)
            {
                spriteRenderers[i].color = color;
            }
        }
    }
}
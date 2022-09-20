using UnityEngine;

namespace Playniax.Pyro.Framework
{
    // Scales the sprite to fit screen.
    public class SpriteToScreenSize : MonoBehaviour
    {
        [System.Serializable]
        public class AdditionalSettings
        {
            public SpriteRenderer spriteRenderer;
        }
        public enum Mode { Fill, Horizontal, Vertical };
        // Mode can be Mode.Fill, Mode.Horizontal or Mode.Vertical.
        public Mode mode;
        public AdditionalSettings additionalSettings;

        void Start()
        {
            _Scale();
        }

        void _Scale()
        {
            if (additionalSettings.spriteRenderer == null) additionalSettings.spriteRenderer = GetComponent<SpriteRenderer>();
            if (additionalSettings.spriteRenderer == null) return;

            var cameraSize = Camera.main.ViewportToWorldPoint(new Vector3(1, 1, transform.position.z - Camera.main.transform.position.z)) * 2;
            var spriteSize = additionalSettings.spriteRenderer.sprite.bounds.size;

            if (mode == Mode.Fill)
            {
                if (cameraSize.x / spriteSize.x > cameraSize.y / spriteSize.y)
                {
                    transform.localScale = Vector3.one * cameraSize.x / spriteSize.x;
                }
                else
                {
                    transform.localScale = Vector3.one * cameraSize.y / spriteSize.y;
                }
            }
            else if (mode == Mode.Horizontal)
            {
                transform.localScale = Vector3.one * cameraSize.x / spriteSize.x;
            }
            else if (mode == Mode.Vertical)
            {
                transform.localScale = Vector3.one * cameraSize.y / spriteSize.y;
            }
        }
    }
}
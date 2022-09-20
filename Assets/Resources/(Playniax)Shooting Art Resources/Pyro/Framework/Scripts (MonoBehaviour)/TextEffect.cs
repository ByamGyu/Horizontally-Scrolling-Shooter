using UnityEngine;

namespace Playniax.Pyro.Framework
{
    // Creates a text effect with support for fading and motion.
    public class TextEffect : MonoBehaviour
    {
        public static TextEffect current;

        public float sustain = .5f;
        public float fadeTime = .25f;
        public Color targetColor = new Color(1, 1, 1, 0);
        public Vector3 targetScale = Vector3.one;
        public Vector3 velocity;
        public TextMesh textMesh;
        public MeshRenderer meshRenderer;

        public static TextEffect Create(string text, Vector3 position, Font font, int fontSize = 21, float sustain = .25f, float fadeTime = .25f)
        {
            var textEffect = new GameObject(text).AddComponent<TextEffect>();

            textEffect._Init();

            textEffect.textMesh.text = text;
            textEffect.textMesh.font = font;
            textEffect.textMesh.fontSize = fontSize;

            textEffect.sustain = sustain;
            textEffect.fadeTime = fadeTime;

            textEffect.transform.position = position;

            if (font) textEffect.meshRenderer.material = font.material;

            textEffect.meshRenderer.enabled = false;

            return textEffect;
        }

        void Awake()
        {
            _Init();

            if (meshRenderer.enabled == true) _Set();
        }

        void Update()
        {
            if (meshRenderer.enabled == false) _Set();

            if (_sustain < sustain)
            {
                _sustain += 1 * Time.deltaTime;
            }
            else
            {
                if (fadeTime > 0 && _fadeTime > 0)
                {
                    transform.position += velocity * Time.deltaTime;
                    textMesh.color = targetColor - (targetColor - _startColor) * (fadeTime / _fadeTime);
                    transform.localScale = targetScale - (targetScale - _startScale) * (fadeTime / _fadeTime);
                    fadeTime -= 1 * Time.deltaTime;
                }
                else
                {
                    Destroy(gameObject);
                }
            }
        }

        void _Init()
        {
            if (textMesh == null) textMesh = GetComponent<TextMesh>();
            if (textMesh == null)
            {
                textMesh = gameObject.AddComponent<TextMesh>();
                textMesh.text = "Hello World";
                textMesh.anchor = TextAnchor.MiddleCenter;
                textMesh.alignment = TextAlignment.Center;
                textMesh.fontSize = 21;
                textMesh.fontStyle = FontStyle.Bold;
                textMesh.characterSize = .1f;
            }

            if (meshRenderer == null) meshRenderer = GetComponent<MeshRenderer>();
            if (meshRenderer == null)
            {
                meshRenderer = gameObject.AddComponent<MeshRenderer>();
                meshRenderer.sortingOrder = 100;
            }
        }
        void _Set()
        {
            _startColor = textMesh.color;

            _startScale = transform.localScale;

            _fadeTime = fadeTime;

            meshRenderer.enabled = true;
        }

        float _fadeTime;
        Vector3 _startScale;
        Color _startColor;
        float _sustain;
    }
}
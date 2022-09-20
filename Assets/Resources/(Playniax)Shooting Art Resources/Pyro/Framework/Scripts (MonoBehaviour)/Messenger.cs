using System.Collections.Generic;
using UnityEngine;
using Playniax.Ignition.Framework;

namespace Playniax.Pyro.Framework
{
    public class Messenger : MonoBehaviour
    {
        [System.Serializable]
        public class MessageSettings
        {
            public MessageSettings(string id, Display display, Color color, bool queued)
            {
                this.id = id;
                this.display = display;
                this.color = color;
                this.queued = queued;
            }
            public enum Display { ObjectPosition, ScreenPosition };

            public string id;
            public Display display;
            public Vector3 offset;
            public float sustain = 1.5f;
            public float fadeTime = 1;
            public Font font;
            public int fontSize = 21;
            public FontStyle fontStyle;
            public Color color;
            public Vector3 velocity;
            public Vector3 targetScale = new Vector3(.95f, .95f, 0);
            public int orderInLayer = 100;
            public AudioProperties sound;
            public bool queued;
            public bool enabled = true;
            public TextEffect Create(string text, Vector3 position)
            {
                var textEffect = TextEffect.Create(text, position, font, fontSize, sustain, fadeTime);
                textEffect.velocity = velocity;
                textEffect.targetColor = color; textEffect.targetColor.a = 0;
                textEffect.targetScale = targetScale;
                textEffect.textMesh.color = color;
                textEffect.meshRenderer.sortingOrder = orderInLayer;

                if (display == Display.ObjectPosition)
                {
                    textEffect.transform.position += offset;

                    textEffect.transform.SetParent(Camera.main.transform, true);
                }
                else if (display == Display.ScreenPosition)
                {
                    var offset = this.offset;

                    offset.z = -Camera.main.transform.position.z;

                    textEffect.transform.position = offset;

                    textEffect.transform.SetParent(Camera.main.transform, false);
                }

                sound.Play();

                return textEffect;
            }
        }

        public MessageSettings[] messageSettings = new MessageSettings[] 
        {
            new MessageSettings("Score", MessageSettings.Display.ObjectPosition, new Color(0, 1, 0, 1), false),
            new MessageSettings("Generic", MessageSettings.Display.ScreenPosition, new Color(1, 1, 1, 1), true), 
            new MessageSettings("Activated", MessageSettings.Display.ScreenPosition, new Color(0, 1, 0, 1), true), 
            new MessageSettings("Deactivated", MessageSettings.Display.ScreenPosition, new Color(1, 0, 0, 1), true), 
            new MessageSettings("Reloaded", MessageSettings.Display.ScreenPosition, new Color(0, 1, 0, 1), true),
            new MessageSettings("Destroyed", MessageSettings.Display.ScreenPosition, new Color(1, 0, 0, 1), true)
        };

        public List<GameObject> queue = new List<GameObject>();
        public static Messenger instance
        {
            get
            {
                if (_instance == null) _instance = FindObjectOfType<Messenger>();

                return _instance;
            }
        }
        public MessageSettings Get(string id)
        {
            for (int i = 0; i < messageSettings.Length; i++)
            {
                if (messageSettings[i].id == id) return messageSettings[i];
            }
            return null;
        }
        public TextEffect Create(string id, string text, Vector3 position)
        {
            var settings = Get(id);
            if (settings == null) return null;

            var textEffect = settings.Create(text, position);

            if (settings.queued)
            {
                textEffect.gameObject.SetActive(false);

                queue.Add(textEffect.gameObject);
            }

            return textEffect;
        }
        void LateUpdate()
        {
            for (int i = 0; i < queue.Count; i++)
            {
                if (queue[i] && queue[i].gameObject && queue[i].activeInHierarchy == false)
                {
                    queue[i].SetActive(true);

                    break;
                }
                if (queue[i] && queue[i].gameObject && queue[i].activeInHierarchy == true)
                {
                    break;
                }
                if (queue[i] == null)
                {
                    _clear.Add(queue[i]);
                }
            }

            for (int i = 0; i < _clear.Count; i++)
            {
                queue.Remove(_clear[i]);
            }

            _clear.Clear();
        }

        static Messenger _instance;

        List<GameObject> _clear = new List<GameObject>();
    }
}
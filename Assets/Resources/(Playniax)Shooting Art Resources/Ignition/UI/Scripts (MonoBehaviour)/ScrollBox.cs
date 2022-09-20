using UnityEngine;
using UnityEngine.UI;
using Playniax.Ignition.Framework;

namespace Playniax.Ignition.UI
{
    // The scrollbox supports text, url links and images and it has its own scripting language. 
    public class ScrollBox : MonoBehaviour
    {
        public TextAsset externalScript;
        public bool useExternalScript;

        [TextArea(15, 20)]
        public string script;
        public string start;
        public string dataBreak = "&";
        public string lineBreak = "|";
        public bool allCaps = false;
        public AssetBank assetBank;
        public GameObject content;
        public float contentHeight;

        // Whether the mouse is hovering the scrollbox or not.
        public bool isMouseOver
        {
            get
            {
                var mousePosition = _rectTransform.InverseTransformPoint(Input.mousePosition);
                if (_rectTransform.rect.Contains(mousePosition)) return true;
                return false;
            }
        }

        // Sets the scrollbox position by key.
        public void SetPosition(string key)
        {
            var child = content.transform.Find(key);
            if (child == null) return;

            var position = new Vector3(child.transform.localPosition.x, -child.transform.localPosition.y, child.transform.localPosition.z);

            position.y += _rectTransform.sizeDelta.y * .5f;
            position.y -= child.GetComponent<Text>().fontSize * .5f;

            content.transform.localPosition = position;
        }

        void Awake()
        {
            _rectTransform = GetComponent<RectTransform>();

            content = new GameObject("Content");

            content.transform.SetParent(transform, false);
        }

        void Start()
        {
            if (useExternalScript && externalScript)
            {
                contentHeight = PageEngine.ExecuteScript(content.transform, _rectTransform.sizeDelta, externalScript.text, assetBank, allCaps, dataBreak, lineBreak);
            }
            else
            {
                contentHeight = PageEngine.ExecuteScript(content.transform, _rectTransform.sizeDelta, script, assetBank, allCaps, dataBreak, lineBreak);
            }

            if (start != "") SetPosition(start);
        }

        RectTransform _rectTransform;
    }
}
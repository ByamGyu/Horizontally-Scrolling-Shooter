using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using Playniax.Ignition.Framework;
using Playniax.Ignition.UI;
using Playniax.Menu.Essentials;

namespace Playniax.Menu.Ignition.UI
{
    public class Menu : EditorBase
    {
        [MenuItem("GameObject/UI/Playniax/Ignition/ScrollBox", false, 101)]
        public static void Add_ScrollBox()
        {
            var canvas = Get_Canvas();
            if (canvas == null) canvas = Add_Canvas();

            var rectTransform = new GameObject("ScrollBox", typeof(RectTransform)).GetComponent<RectTransform>();
            rectTransform.sizeDelta = new Vector2(640, 320);
            rectTransform.transform.SetParent(canvas.transform);
            rectTransform.transform.localPosition = Vector3.zero;

            var image = rectTransform.gameObject.AddComponent<Image>();
            image.color = new Color(.25f, .25f, .25f, .25f);
            image.maskable = true;

            rectTransform.gameObject.AddComponent<Mask>();

            var script = AssetDatabase.LoadAssetAtPath("Assets/Playniax/Ignition/Examples/02 - UI/Scrollbox/_script example.txt", typeof(TextAsset)) as TextAsset;
            var font = Resources.GetBuiltinResource<Font>("Arial.ttf");

            var scrollBox = rectTransform.gameObject.AddComponent<ScrollBox>();
            scrollBox.script = script.text;
            scrollBox.assetBank = new AssetBank();
            scrollBox.assetBank.assets = new Object[1] { font };

            var autoScroll = scrollBox.gameObject.AddComponent<ScrollBoxAutoScroll>();
            //autoScroll.speed = 10;

            Undo.RegisterCreatedObjectUndo(scrollBox.gameObject, "Create object");

            Selection.activeGameObject = scrollBox.gameObject;
        }
    }
}

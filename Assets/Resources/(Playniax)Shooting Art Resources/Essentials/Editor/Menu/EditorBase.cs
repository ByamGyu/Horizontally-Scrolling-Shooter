// https://docs.unity3d.com/ScriptReference/MenuItem.html
// https://answers.unity.com/questions/22947/adding-to-the-context-menu-of-the-hierarchy-tab.html

using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

namespace Playniax.Menu.Essentials
{
    public class EditorBase
    {
        public static GameObject Add(string path)
        {
            var gameObject = GetAssetAtPath(path);

            return gameObject;
        }
        public static GameObject GetAssetAtPath(string path)
        {
            GameObject gameObject;

            var active = Selection.activeGameObject;

            if (active)
            {
                gameObject = Object.Instantiate(AssetDatabase.LoadAssetAtPath(path, typeof(GameObject)) as GameObject, active.transform);
            }
            else
            {
                gameObject = Object.Instantiate(AssetDatabase.LoadAssetAtPath(path, typeof(GameObject)) as GameObject);
            }

            if (gameObject)
            {
                gameObject.name = gameObject.name.Replace("(Clone)", "");

                Undo.RegisterCreatedObjectUndo(gameObject, "Create object");

                Selection.activeGameObject = gameObject;
            }

            return gameObject;
        }

        public static Canvas Add_Canvas()
        {
            var canvas = new GameObject("Canvas").AddComponent<Canvas>();

            canvas.renderMode = RenderMode.ScreenSpaceOverlay;
            canvas.gameObject.AddComponent<CanvasScaler>();
            canvas.gameObject.AddComponent<GraphicRaycaster>();

            Undo.RegisterCreatedObjectUndo(canvas.gameObject, "Create object");

            Selection.activeGameObject = canvas.gameObject;

            return canvas;
        }
        public static Canvas Get_Canvas()
        {
            Canvas canvas;

            var all = Object.FindObjectsOfType<Canvas>();
            if (all.Length == 0) return null;

            var active = Selection.activeGameObject;

            if (active)
            {
                canvas = active.GetComponent<Canvas>();
                if (canvas) return canvas;
            }

            if (all[0]) return all[0];

            return null;
        }
    }
}

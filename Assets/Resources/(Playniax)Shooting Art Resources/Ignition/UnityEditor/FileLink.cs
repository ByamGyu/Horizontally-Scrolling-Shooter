#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Playniax.Ignition.UnityEditor
{
    public class FileLink : MonoBehaviour, IPointerClickHandler
    {
        public string file = "GameData.cs";

        public void OnPointerClick(PointerEventData pointerEventData)
        {
            foreach (var lAssetPath in AssetDatabase.GetAllAssetPaths())
            {
                if (lAssetPath.EndsWith(file))
                {
                    var lScript = (MonoScript)AssetDatabase.LoadAssetAtPath(lAssetPath, typeof(MonoScript));
                    if (lScript != null)
                    {
                        AssetDatabase.OpenAsset(lScript);
                        break;
                    }
                }
            }
        }
    }
}
#endif
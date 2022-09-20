using UnityEngine;
using UnityEngine.UI;

namespace Playniax.Ignition.Framework
{
    [System.Serializable]
    // AssetBank allows you to allocate assets and 'Load' or 'Get' them at will.
    // The ScrollBox uses the AssetBank to load images and fonts for example.
    // Without adding the images or fonts to the AssetBank the scrollbox can not address them.
    public class AssetBank
    {
        // Variable to store assets.
        public Object[] assets;
        // Returns Object by name.
        public Object Get(string name)
        {
            for (int i = 0; i < assets.Length; i++)
            {
                if (assets[i].name == name) return assets[i];
            }
            return null;
        }
        // Returns Font by name.
        public Font GetFont(string name)
        {
            return Get(name) as Font;
        }
        // Returns Image by name.
        public Image GetImage(string name)
        {
            return Get(name) as Image;
        }
        // Returns Texture2D by name.
        public Texture2D GetTexture2D(string name)
        {
            return Get(name) as Texture2D;
        }
        // Returns Sprite by name.
        public Sprite GetSprite(string name)
        {
            return Get(name) as Sprite;
        }
        // Returns TextAsset by name.
        public TextAsset GetTextAsset(string name)
        {
            return Get(name) as TextAsset;
        }
    }
}
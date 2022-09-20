using System;
using System.Globalization;
using UnityEngine;
using UnityEngine.UI;
using Playniax.Ignition.Framework;

#if UNITY_EDITOR
using Playniax.Ignition.UnityEditor;
#endif

namespace Playniax.Ignition.UI
{
    public class PageEngine
    {
        public class Preset
        {
            public Color imageColor = Color.white;
            public Color textColor = Color.white;
            public Color linkColor = Color.red;
            public bool textCropping = true;
            public int textBorders;
            public string fontName;
            public int fontSize = 32;
            public float scale = 1;
            public bool verticalSpacing = true;
            public float imageRowSpacing = 8;
            public float lineSpacing = 1;
            public float rotation;
            public string textAlignment;
            public string imageAlignment;
        }
        public static float ExecuteScript(Transform transform, Vector2 size, string script, AssetBank assets, bool allCaps = false, string dataBreak = "&", string lineBreak = "|")
        {
            Preset[] settings = new Preset[16];

            float contentHeight = 0;
            float x = 0;
            float y = size.y * .5f;
            Color imageColor = Color.white;
            Color textColor = Color.white;
            Color linkColor = Color.red;
            bool textCropping = true;
            int textBorders = 0;
            string fontName = "";
            int fontSize = 32;
            float scale = 1;
            bool verticalSpacing = true;
            float imageRowSpacing = 8;
            float lineSpacing = 1;
            float rotation = 0;
            string textAlignment = "middle";
            string imageAlignment = "middle";

            script = script.Trim();

            script = script.Replace("\r", "\n");

            string[] lines = script.Split("\n"[0]);

            for (int i = 0; i < lines.Length; i++)
            {
                string[] line = lines[i].Split("="[0]);

                if (line.Length == 1)
                {
                    if (line[0] == "reset")
                    {
                        textColor = Color.white;
                        imageColor = Color.white;
                        linkColor = Color.red;
                        textCropping = true;
                        textBorders = 0;
                        fontSize = 32;
                        scale = 1;
                        verticalSpacing = true;
                        imageRowSpacing = 8;
                        lineSpacing = 1;
                        rotation = 0;
                        textAlignment = "middle";
                        imageAlignment = "middle";
                    }
                }
                else if (line.Length == 2)
                {
                    var data = line[1].Split(dataBreak[0]);

                    if (line[0] == "verticalSpacing")
                    {
                        verticalSpacing = _GetBool(line[1]);
                    }
                    else if (line[0] == "x")
                    {
                        x = float.Parse(line[1], CultureInfo.InvariantCulture) * scale;
                    }
                    else if (line[0] == "y")
                    {
                        y = float.Parse(line[1], CultureInfo.InvariantCulture) * scale;
                    }
                    else if (line[0] == "y+")
                    {
                        y -= float.Parse(line[1], CultureInfo.InvariantCulture) * scale;

                        contentHeight += float.Parse(line[1], CultureInfo.InvariantCulture) * scale;
                    }
                    else if (line[0] == "textColor")
                    {
                        textColor = _GetColor(line[1], textColor);
                    }
                    else if (line[0] == "linkColor")
                    {
                        linkColor = _GetColor(line[1], linkColor);
                    }
                    else if (line[0] == "imageAlignment")
                    {
                        imageAlignment = line[1];
                    }
                    else if (line[0] == "imageColor")
                    {
                        imageColor = _GetColor(line[1], imageColor);
                    }
                    else if (line[0] == "font")
                    {
                        fontName = line[1];
                    }
                    else if (line[0] == "fontSize")
                    {
                        fontSize = int.Parse(line[1], CultureInfo.InvariantCulture);
                    }
                    else if (line[0] == "imageRowSpacing")
                    {
                        imageRowSpacing = int.Parse(line[1], CultureInfo.InvariantCulture);
                    }
                    else if (line[0] == "lineSpacing")
                    {
                        lineSpacing = float.Parse(line[1], CultureInfo.InvariantCulture);
                    }
                    else if (line[0] == "rotation")
                    {
                        rotation = float.Parse(line[1], CultureInfo.InvariantCulture);
                    }
                    else if (line[0] == "scale")
                    {
                        scale = float.Parse(line[1], CultureInfo.InvariantCulture);
                    }
                    else if (line[0] == "textBorders")
                    {
                        textBorders = int.Parse(line[1], CultureInfo.InvariantCulture);
                    }
                    else if (line[0] == "textCropping")
                    {
                        textCropping = _GetBool(line[1]);
                    }
                    else if (line[0] == "textAlignment")
                    {
                        textAlignment = line[1];
                    }
                    else if (line[0] == "saveSettings")
                    {
                        var index = int.Parse(line[1], CultureInfo.InvariantCulture);

                        if (settings[index] == null) settings[index] = new Preset();

                        settings[index].imageColor = imageColor;
                        settings[index].textColor = textColor;
                        settings[index].linkColor = linkColor;
                        settings[index].textCropping = textCropping;
                        settings[index].textBorders = textBorders;
                        settings[index].fontName = fontName;
                        settings[index].fontSize = fontSize;
                        settings[index].scale = scale;
                        settings[index].verticalSpacing = verticalSpacing;
                        settings[index].imageRowSpacing = imageRowSpacing;
                        settings[index].lineSpacing = lineSpacing;
                        settings[index].rotation = rotation;
                        settings[index].imageAlignment = imageAlignment;
                        settings[index].textAlignment = textAlignment;
                    }
                    else if (line[0] == "loadSettings")
                    {
                        var index = int.Parse(line[1], CultureInfo.InvariantCulture);

                        imageColor = settings[index].imageColor;
                        textColor = settings[index].textColor;
                        linkColor = settings[index].linkColor;
                        textCropping = settings[index].textCropping;
                        textBorders = settings[index].textBorders;
                        fontName = settings[index].fontName;
                        fontSize = settings[index].fontSize;
                        scale = settings[index].scale;
                        verticalSpacing = settings[index].verticalSpacing;
                        imageRowSpacing = settings[index].imageRowSpacing;
                        lineSpacing = settings[index].lineSpacing;
                        rotation = settings[index].rotation;
                        imageAlignment = settings[index].imageAlignment;
                        textAlignment = settings[index].textAlignment;
                    }
                    else if (line[0] == "text" || line[0] == "textAsset" || line[0] == "link")
                    {
                        var font = assets.GetFont(fontName);

                        string text = "";

                        if (line[0] == "text" || line[0] == "link")
                        {
                            text = data[0];

                            text = text.Replace(lineBreak, "\n");
                        }
                        else if (line[0] == "textAsset")
                        {
                            var textAsset = assets.GetTextAsset(data[0]);

                            if (textAsset)
                            {
                                text = textAsset.text;

                                text = text.Replace(lineBreak, "\n");
                            }
                        }

                        text = text.Replace("\\n", "\n");

                        if (allCaps) text = text.ToUpper();

                        var textLines = text.Split("\n"[0]);

                        if (textCropping) textLines = _FitText(textLines, font, fontSize, size.x - textBorders);

                        for (int l = 0; l < textLines.Length; l++)
                        {
                            var newText = _AddText(transform, font, fontSize, scale, rotation, textColor, textLines[l]);

                            newText.GetComponent<RectTransform>().sizeDelta = new Vector2(newText.preferredWidth, newText.preferredHeight);

                            if (data.Length == 2 && line[0] == "link")
                            {
                                _CreateLink(newText, data[1], linkColor);
                            }
                            else
                            {
                                newText.raycastTarget = false;
                            }

                            if (verticalSpacing == true)
                            {
                                y -= newText.preferredHeight * .5f * scale;
                                contentHeight += newText.preferredHeight * .5f * scale;

                                y -= lineSpacing * scale;
                                contentHeight += lineSpacing * scale;
                            }

                            if (textAlignment == "middle")
                            {
                                newText.transform.localPosition = new Vector3(x, y);
                            }
                            if (textAlignment == "left")
                            {
                                newText.transform.localPosition = new Vector3(x - size.x / 2 + newText.preferredWidth / 2 + textBorders / 2, y);
                            }
                            if (textAlignment == "right")
                            {
                                newText.transform.localPosition = new Vector3(x + size.x / 2 - newText.preferredWidth / 2 - textBorders / 2, y);
                            }

                            if (verticalSpacing == true)
                            {
                                y -= newText.preferredHeight * .5f * scale;
                                contentHeight += newText.preferredHeight * .5f * scale;

                                y -= lineSpacing * scale;
                                contentHeight += lineSpacing * scale;
                            }
                        }
                    }
                    else if (line[0] == "image")
                    {
                        var sprite = assets.GetSprite(data[0]);
                        if (sprite == null) break;

                        var newImage = _AddImage(transform, sprite, scale, rotation, imageColor);

                        if (verticalSpacing == true)
                        {
                            y -= newImage.sprite.texture.height * .5f * scale;
                            contentHeight += newImage.sprite.texture.height * .5f * scale;

                            y -= lineSpacing * scale;
                            contentHeight += lineSpacing * scale;
                        }

                        if (imageAlignment == "middle")
                        {
                            newImage.transform.localPosition = new Vector3(x, y);
                        }
                        if (imageAlignment == "left")
                        {
                            newImage.transform.localPosition = new Vector3(x - size.x / 2 + newImage.preferredWidth / 2, y);
                        }
                        if (imageAlignment == "right")
                        {
                            newImage.transform.localPosition = new Vector3(x + size.x / 2 - newImage.preferredWidth / 2, y);
                        }

                        if (verticalSpacing == true)
                        {
                            y -= newImage.sprite.texture.height * .5f * scale;
                            contentHeight += newImage.sprite.texture.height * .5f * scale;

                            y -= lineSpacing * scale;
                            contentHeight += lineSpacing * scale;
                        }

                        if (data.Length == 2)
                        {
                            var link = newImage.gameObject.AddComponent<Link>();
                            link.link = data[1];
                        }
                        else
                        {
                            newImage.raycastTarget = false;
                        }
                    }
                    else if (line[0] == "backgroundImage")
                    {
                        var sprite = assets.GetSprite(line[1]);
                        if (sprite == null) break;

                        var newBackgroundImage = _AddImage(transform, sprite, scale, rotation, imageColor);
                        newBackgroundImage.raycastTarget = false;
                    }
                    else if (line[0] == "imageRow")
                    {
                        var sprites = line[1].Split(","[0]);
                        if (sprites == null) break;

                        var height = _AddImages(transform, assets, sprites, x, y, imageRowSpacing, scale, rotation, imageColor);

                        y -= height * scale;
                        y -= lineSpacing * scale;

                        contentHeight += height * scale;
                        contentHeight += lineSpacing * scale;
                    }
                }
            }

            return contentHeight;
        }

        static Image _AddImage(Transform transform, Sprite sprite, float scale, float rotation, Color color)
        {
            var obj = new GameObject(sprite.name);

            obj.transform.SetParent(transform, false);

            var imageComponent = obj.AddComponent<Image>();

            imageComponent.sprite = sprite;
            imageComponent.color = color;

            imageComponent.SetNativeSize();

            imageComponent.transform.localScale = new Vector3(scale, scale, scale);

            imageComponent.transform.Rotate(0, 0, rotation, Space.Self);

            //imageComponent.raycastTarget = false;

            return imageComponent;
        }

        static float _AddImages(Transform transform, AssetBank assets, string[] sprites, float x, float y, float space, float scale, float rotation, Color color)
        {
            float width = 0;
            float height = 0;

            for (int i = 0; i < sprites.Length; i++)
            {
                var image = assets.GetSprite(sprites[i]);
                if (image == null) return 0;

                width += image.texture.width;
                width += space;

                if (height < image.texture.height) height = image.texture.height;
            }

            var newImage = new Image[sprites.Length];

            for (int i = 0; i < sprites.Length; i++)
            {
                var image = assets.GetSprite(sprites[i]);

                newImage[i] = _AddImage(transform, image, scale, rotation, color);
            }

            if (newImage.Length > 1) x += -space * .5f * scale + newImage[0].sprite.texture.width * .5f * scale - width * .5f * scale;

            y -= height * .5f * scale;

            for (int i = 0; i < sprites.Length; i++)
            {
                newImage[i].transform.localPosition = new Vector3(x, y);

                x += space * scale + newImage[i].sprite.texture.width * .5f * scale;

                if (i < newImage.Length - 1) x += space * scale + newImage[i + 1].sprite.texture.width * .5f * scale;
            }

            return height;
        }

        static Text _AddText(Transform transform, Font font, int size, float scale, float rotation, Color color, string text)
        {
            var obj = new GameObject(text);

            obj.transform.SetParent(transform, false);

            var textComponent = obj.AddComponent<Text>();

            textComponent.text = text;
            textComponent.font = font;
            textComponent.fontSize = size;
            textComponent.horizontalOverflow = HorizontalWrapMode.Overflow;
            textComponent.verticalOverflow = VerticalWrapMode.Overflow;
            textComponent.alignment = TextAnchor.MiddleCenter;
            textComponent.transform.localScale = new Vector3(scale, scale, scale);
            textComponent.transform.Rotate(0, 0, rotation, Space.Self);
            textComponent.color = color;

            return textComponent;
        }

        static string[] _FitText(string[] text, Font font, int fontSize, float width)
        {
            var lines = new string[0];

            for (int i1 = 0; i1 < text.Length; i1++)
            {
                var line = "";
                var word = text[i1].Split(" "[0]);

                for (int i2 = 0; i2 < word.Length; i2++)
                {
                    if (_GetTextWidth(line + word[i2], font, fontSize) < width)
                    {
                        line += word[i2] + " ";
                    }
                    else
                    {
                        Array.Resize(ref lines, lines.Length + 1);
                        lines[lines.Length - 1] = line.Trim();
                        line = word[i2] + " ";
                    }
                }

                if (line != "")
                {
                    Array.Resize(ref lines, lines.Length + 1);
                    lines[lines.Length - 1] = line.Trim();
                }
            }

            return lines;
        }

        static void _CreateLink(Text text, string link, Color color)
        {
            text.color = color;

            if (link.StartsWith("http") || link.StartsWith("www"))
            {
                text.gameObject.AddComponent<Link>().link = link;
            }
#if UNITY_EDITOR
            else
            {
                text.gameObject.AddComponent<FileLink>().file = link;
            }
#endif
        }
        static bool _GetBool(string data)
        {
            if (data == "true") return true;
            return false;
        }

        static Color _GetColor(string data, Color color = default)
        {
            string[] rgba = data.Split(","[0]);

            if (rgba.Length < 3 || rgba.Length > 4) return color;

            color.r = float.Parse(rgba[0], CultureInfo.InvariantCulture);
            color.g = float.Parse(rgba[1], CultureInfo.InvariantCulture);
            color.b = float.Parse(rgba[2], CultureInfo.InvariantCulture);

            if (rgba.Length == 4) color.a = float.Parse(rgba[3], CultureInfo.InvariantCulture);

            return color;
        }

        static float _GetTextWidth(string text, Font font, int fontSize)
        {
            var obj = new GameObject(text);

            var textComponent = obj.AddComponent<Text>();

            textComponent.text = text;
            textComponent.font = font;
            textComponent.fontSize = fontSize;

            var width = textComponent.preferredWidth;

            MonoBehaviour.Destroy(obj);

            return width;
        }
    }
}
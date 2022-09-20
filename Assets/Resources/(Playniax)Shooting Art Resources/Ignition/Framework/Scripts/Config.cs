using System;
using System.Globalization;
using UnityEngine;

namespace Playniax.Ignition.Framework
{
    [System.Serializable]
    // The Config class can load data from and read data from a config file or store it temporarily to memory.
    //
    // Config files usually contain data to configure parameters and initial settings for your app.
    public class Config
    {
        [System.Serializable]
        public class Data
        {
            public string key;
            public string value;
        }
        public Config()
        {
            data = new Data[0];
        }
        // Creates Config object from string.
        public Config(string source, string splitter = "\n")
        {
            source = source.Trim();

            string[] lines = source.Split(splitter[0]);

            data = new Data[lines.Length];

            for (int i = 0; i < lines.Length; i++)
            {
                string[] line = lines[i].Split("="[0]);

                data[i] = new Data();

                if (line.Length == 1)
                {
                    data[i].key = line[0].Trim();
                }
                else if (line.Length == 2)
                {
                    data[i].key = line[0].Trim();
                    data[i].value = line[1].Trim();
                }
                else if (line.Length > 2)
                {
                    data[i].key = line[0].Trim();
                    data[i].value = lines[i].Trim().Replace(line[0].Trim() + "=", "");
                }
            }
        }
        // Removes all settings.
        public void Clear()
        {
            data = new Data[0];
        }
        // Returns key position (returns -1 if key is not found).
        public int Find(string key)
        {
            for (int i = 0; i < data.Length; i++)
            {
                if (data[i].key.ToLower() == key.ToLower()) return i;
            }

            return -1;
        }
        // Returns the value of key.

        // Returns defaultValue if key is not found.
        public bool GetBool(string key, bool defaultValue = false)
        {
            for (int i = 0; i < data.Length; i++)
            {
                if (data[i].key.ToLower() == key.ToLower()) return bool.Parse(data[i].value);
            }

            return defaultValue;
        }
        // Returns the value of key.

        // Returns defaultValue if key is not found.
        public Color GetColor(string key, Color color = default)
        {
            string[] rgba = GetString(key).Split(","[0]);

            if (rgba.Length < 3 || rgba.Length > 4) return color;

            color.r = float.Parse(rgba[0]);
            color.g = float.Parse(rgba[1]);
            color.b = float.Parse(rgba[2]);

            if (rgba.Length == 4) color.a = float.Parse(rgba[3]);

            return color;
        }
        // Returns the value of key.

        // Returns defaultValue if key is not found.
        public float GetFloat(string key, float defaultValue = 0)
        {
            for (int i = 0; i < data.Length; i++)
            {
                if (data[i].key.ToLower() == key.ToLower()) return float.Parse(data[i].value, CultureInfo.InvariantCulture.NumberFormat);
            }

            return defaultValue;
        }
        public float[] GetFloatData(string key)
        {
            if (_index >= data.Length) _index = 0;

            for (int i = 0; i < data.Length; i++)
            {
                if (data[_index].key.ToLower() == key.ToLower())
                {
                    //return System.Array.ConvertAll<string, float>(data[_index].key.Split(","[0]), float.Parse);
                    return _ConvertAll(data[_index].value);
                }
                else
                {
                    _index += 1; if (_index >= data.Length) _index = 0;
                }
            }

            return null;
        }
        // Returns the value of key.

        // Returns defaultValue if key is not found.
        public int GetInt(string key, int defaultValue = 0)
        {
            if (_index >= data.Length) _index = 0;

            for (int i = 0; i < data.Length; i++)
            {
                if (data[_index].key.ToLower() == key.ToLower())
                {
                    return int.Parse(data[_index].value);
                }
                else
                {
                    _index += 1; if (_index >= data.Length) _index = 0;
                }
            }

            return defaultValue;
        }
        public int[] GetIntData(string key)
        {
            if (_index >= data.Length) _index = 0;

            for (int i = 0; i < data.Length; i++)
            {
                if (data[_index].key.ToLower() == key.ToLower())
                {
                    return System.Array.ConvertAll<string, int>(data[_index].value.Split(","[0]), int.Parse);
                }
                else
                {
                    _index += 1; if (_index >= data.Length) _index = 0;
                }
            }

            return null;
        }
        public Config GetProperties(string key)
        {
            var data = GetString(key);

            if (data == "") return null;

            data = data.Replace("],[", "|");
            data = data.Replace("[", "");
            data = data.Replace("]", "");

            if (!data.Contains("=")) return null;

            var properties = new Config(data, "|");

            return properties;
        }
        // Returns the value of key.

        // Returns defaultValue if key is not found.
        public string GetString(string key, string defaultValue = "")
        {
            if (_index >= data.Length) _index = 0;

            for (int i = 0; i < data.Length; i++)
            {
                if (data[_index].key.ToLower() == key.ToLower())
                {
                    return data[_index].value;
                }
                else
                {
                    _index += 1; if (_index >= data.Length) _index = 0;
                }
            }

            return defaultValue;
        }
        public string[] GetStringData(string key)
        {
            if (_index >= data.Length) _index = 0;

            for (int i = 0; i < data.Length; i++)
            {
                if (data[_index].key.ToLower() == key.ToLower())
                {
                    return data[_index].value.Split(","[0]);
                }
                else
                {
                    _index += 1; if (_index >= data.Length) _index = 0;
                }
            }

            return null;
        }
        // Returns the value of key.

        // Returns defaultValue if key is not found.
        public Vector2 GetVector2(string key, Vector2 vector = default)
        {
            string[] xy = GetString(key).Split(","[0]);

            if (xy.Length < 1 || xy.Length > 2) return vector;

            vector.x = float.Parse(xy[0]);
            vector.y = float.Parse(xy[1]);

            return vector;
        }
        // Returns the value of key.

        // Returns defaultValue if key is not found.
        public Vector3 GetVector3(string key, Vector3 vector = default)
        {
            string[] xyz = GetString(key).Split(","[0]);

            if (xyz.Length < 1 || xyz.Length > 3) return vector;

            vector.x = float.Parse(xyz[0]);
            vector.y = float.Parse(xyz[1]);
            vector.z = float.Parse(xyz[2]);

            return vector;
        }
        public void Set(string source)
        {
            Set(source.Split("="[0]));
        }
        public void Set(string[] source)
        {
            if (source == null) return;
            if (source.Length < 2) return;
            if (source.Length > 2) return;

            if (source[0] == "") return;
            if (source[1] == "") return;

            var i = Find(source[0]);

            if (i == -1)
            {
                Array.Resize(ref data, data.Length + 1);

                data[data.Length - 1] = new Data();
                data[data.Length - 1].key = source[0];
                data[data.Length - 1].value = source[1];
            }
            else
            {
                data[i].value = source[1];
            }
        }
        // Sets value.
        public void SetBool(string key, bool value)
        {
            if (key != "") Set(new string[] { key, value.ToString() });
        }
        // Sets value.
        public void SetInt(string key, int value)
        {
            if (key != "") Set(new string[] { key, value.ToString() });
        }
        // Sets value.
        public void SetString(string key, string value)
        {
            if (key != "" && value != "") Set(new string[] { key, value });
        }
        public override string ToString()
        {
            string str = "";

            for (int i = 0; i < data.Length; i++)
            {
                if (str != "") str += "\n";
                str += data[i].key + "=" + data[i].value;
            }

            return str;
        }
        float[] _ConvertAll(string str)
        {
            var data = str.Split(","[0]);

            var floatData = new float[data.Length];

            for (int i = 0; i < data.Length; i++)
            {
                floatData[i] = float.Parse(data[i], CultureInfo.InvariantCulture.NumberFormat);
            }

            return floatData;
        }

        int _index = 0;

        public Data[] data;
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Playniax.Ignition.Framework
{
    // Collection of string functions.
    public class StringHelpers
    {
        // Format.
        public static string Format(string str, int chars)
        {
            var length = chars - str.Length;

            for (int i = 0; i < length; i++)
            {
                str = "0" + str;
            }

            return str;
        }
    }
}
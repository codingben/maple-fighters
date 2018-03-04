using System.Text.RegularExpressions;
using UnityEngine;

namespace Scripts.Gameplay
{
    public static class Utils
    {
        public static bool IsInLayerMask(int layer, LayerMask layermask)
        {
            return layermask == (layermask | (1 << layer));
        }

        /// <summary>
        /// Making a space between words (For instance, "AaaBbb" will be "Aaa Bbb")
        /// </summary>
        public static string MakeSpaceBetweenWords(this string value)
        {
            var result = Regex.Replace(value, "[A-Z]", " $0").Trim();
            return result;
        }
    }
}
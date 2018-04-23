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

        /// <summary>
        /// Removing a "(Clone)" from a name of the created game object.
        /// </summary>
        public static string RemoveCloneFromName(this string value)
        {
            var result = value.Replace("(Clone)", string.Empty);
            return result;
        }
    }
}
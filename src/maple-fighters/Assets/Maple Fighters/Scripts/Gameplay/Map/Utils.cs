using System.Text.RegularExpressions;
using UnityEngine;

namespace Scripts.Gameplay.Map
{
    public static class Utils
    {
        /// <summary>
        /// Checks if the layer exists.
        /// </summary>
        /// <param name="layer">
        /// The layer.
        /// </param>
        /// <param name="layermask">
        /// The layermask.
        /// </param>
        /// <returns>
        /// The <see cref="bool"/>.
        /// </returns>
        public static bool IsInLayerMask(int layer, LayerMask layermask)
        {
            return layermask == (layermask | (1 << layer));
        }

        /// <summary>
        /// Making a space between words (For instance, "AaaBbb" will be "Aaa Bbb")
        /// </summary>
        /// <param name="value">
        /// The value.
        /// </param>
        /// <returns>
        /// The <see cref="string"/>.
        /// </returns>
        public static string MakeSpaceBetweenWords(this string value)
        {
            return Regex.Replace(value, "[A-Z]", " $0").Trim();
        }

        /// <summary>
        /// Removing a "(Clone)" from a name of the created game object.
        /// </summary>
        /// <param name="value">
        /// The value.
        /// </param>
        /// <returns>
        /// The <see cref="string"/>.
        /// </returns>
        public static string RemoveCloneFromName(this string value)
        {
            return value.Replace("(Clone)", string.Empty);
        }
    }
}
using System.Text.RegularExpressions;
using UnityEngine;

namespace UI.Manager
{
    public static class Utils
    {
        /// <summary>
        /// Creates a new element from resources.
        /// </summary>
        /// <param name="path">
        /// The path.
        /// </param>
        /// <typeparam name="TUIElement">
        /// </typeparam>
        /// <returns>
        /// The <see cref="TUIElement"/>.
        /// </returns>
        public static TUIElement LoadAndCreateUIElement<TUIElement>(string path)
            where TUIElement : UIElement
        {
            var prefab = Resources.Load<TUIElement>(path);
            if (prefab == null)
            {
                throw new UtilsException(
                    $"The prefab in the path {path} not found.");
            }

            var gameObject = Object.Instantiate(
                prefab,
                Vector3.zero,
                Quaternion.identity);
            gameObject.name = prefab.name.RemoveCloneFromName();
            gameObject.name = prefab.name.MakeSpaceBetweenWords();
            gameObject.transform.position = prefab.transform.position;

            return gameObject.GetComponent<TUIElement>();
        }

        /// <summary>
        /// Makes a space between words ("AaaBbb" will be "Aaa Bbb").
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
        /// Removes the "(Clone)" from name of the created game object.
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
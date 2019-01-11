using System.Text.RegularExpressions;
using UnityEngine;

namespace UI.Manager
{
    public static class Utils
    {
        /// <summary>
        /// Loads and creates a new game object from the resources.
        /// </summary>
        /// <param name="path">
        /// The path.
        /// </param>
        /// <returns>
        /// The <see cref="GameObject"/>.
        /// </returns>
        public static GameObject LoadAndCreateGameObject(string path)
        {
            var prefab = Resources.Load(path) as GameObject;
            if (prefab != null)
            {
                var gameObject = Object.Instantiate(
                    prefab,
                    Vector3.zero,
                    Quaternion.identity);
                gameObject.name = prefab.name.RemoveCloneFromName();
                gameObject.transform.position = prefab.transform.position;

                return gameObject;
            }

            throw new UtilsException(
                $"The prefab in the path {path} not found.");
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
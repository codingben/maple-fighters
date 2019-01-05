using System.Text.RegularExpressions;
using UnityEngine;

namespace UserInterface
{
    public static class UiUtils
    {
        /// <summary>
        /// Creates a new element from resources.
        /// </summary>
        /// <typeparam name="TUiElement">
        /// </typeparam>
        /// <returns>
        /// The <see cref="TUiElement"/>.
        /// </returns>
        public static TUiElement CreateUiElement<TUiElement>()
            where TUiElement : class // UiElement
        {
            const string UiElementsPath = "UI/{0}";

            TUiElement uiElement;

            var userInterfaceObject =
                Resources.Load(string.Format(UiElementsPath, typeof(TUiElement).Name))
                    as GameObject;
            if (userInterfaceObject != null)
            {
                var userInterfaceGameObject = Object.Instantiate(
                    userInterfaceObject,
                    Vector3.zero,
                    Quaternion.identity);

                uiElement =
                    userInterfaceGameObject.GetComponent<TUiElement>();

                userInterfaceGameObject.name = 
                    userInterfaceGameObject.name.RemoveCloneFromName();

                userInterfaceGameObject.name = 
                    userInterfaceGameObject.name.MakeSpaceBetweenWords();
            }
            else
            {
                throw new UiException(
                    $"The UI element {typeof(TUiElement).Name} not found.");
            }

            return uiElement;
        }

        /// <summary>
        /// Makes a space between words ("AaaBbb" will be "Aaa Bbb")
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
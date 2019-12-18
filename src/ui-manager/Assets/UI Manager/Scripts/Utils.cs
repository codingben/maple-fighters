using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.UI;

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
        public static TUIElement LoadAndCreate<TUIElement>(string path)
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

        /// <summary>
        /// Creates the parent of the UI components.
        /// </summary>
        /// <param name="name">
        /// The name.
        /// </param>
        /// <param name="sortingOrder">
        /// The sorting Order.
        /// </param>
        /// <returns>
        /// The <see cref="Transform"/>.
        /// </returns>
        public static Transform CreateCanvas(string name, int sortingOrder)
        {
            // Game Object
            var canvasGameObject = new GameObject(name, typeof(Canvas));

            // Canvas
            var canvas = canvasGameObject.GetComponent<Canvas>();
            canvas.renderMode = RenderMode.ScreenSpaceOverlay;
            canvas.sortingOrder = sortingOrder;

            // Canvas Scaler
            var canvasScaler = canvasGameObject.AddComponent<CanvasScaler>();
            canvasScaler.uiScaleMode =
                CanvasScaler.ScaleMode.ScaleWithScreenSize;
            canvasScaler.matchWidthOrHeight = 1;

            // Graphic Raycaster
            canvasGameObject.AddComponent<GraphicRaycaster>();

            // Canvas Group
            var canvasGroup = canvasGameObject.AddComponent<CanvasGroup>();
            canvasGroup.alpha = 1;
            canvasGroup.interactable = true;
            canvasGroup.blocksRaycasts = true;

            return canvasGameObject.GetComponent<Transform>();
        }
    }
}
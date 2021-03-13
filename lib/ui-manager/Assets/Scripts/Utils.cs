using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Manager
{
    using ScaleMode = CanvasScaler.ScaleMode;

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
            var uiPrefab = Resources.Load<TUIElement>(path);
            var uiElement = Object.Instantiate(
                uiPrefab,
                Vector3.zero,
                Quaternion.identity);
            uiElement.name = uiPrefab.name.RemoveCloneFromName();
            uiElement.name = uiPrefab.name.MakeSpaceBetweenWords();
            uiElement.transform.position = uiPrefab.transform.position;

            return uiElement.GetComponent<TUIElement>();
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
        public static Transform CreateCanvas(
            string name,
            int sortingOrder,
            RenderMode renderMode = RenderMode.ScreenSpaceOverlay,
            ScaleMode uiScaleMode = ScaleMode.ScaleWithScreenSize,
            float matchWidthOrHeight = 0.5f,
            float alpha = 1f,
            bool interactable = true,
            bool blocksRaycasts = true)
        {
            // Game Object
            var uiCanvas = new GameObject(
                name,
                typeof(Canvas),
                typeof(CanvasScaler),
                typeof(GraphicRaycaster),
                typeof(CanvasGroup));

            // Canvas
            var canvas = uiCanvas.GetComponent<Canvas>();
            canvas.renderMode = renderMode;
            canvas.sortingOrder = sortingOrder;

            // Canvas Scaler
            var canvasScaler = uiCanvas.GetComponent<CanvasScaler>();
            canvasScaler.uiScaleMode = uiScaleMode;
            canvasScaler.matchWidthOrHeight = matchWidthOrHeight;

            // Canvas Group
            var canvasGroup = uiCanvas.GetComponent<CanvasGroup>();
            canvasGroup.alpha = alpha;
            canvasGroup.interactable = interactable;
            canvasGroup.blocksRaycasts = blocksRaycasts;

            return uiCanvas.GetComponent<Transform>();
        }
    }
}
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    using ScaleMode = CanvasScaler.ScaleMode;

    public static class Utils
    {
        /// <summary>
        /// Loads from resources and creates a UI element.
        /// </summary>
        /// <typeparam name="TUIElement">The UI element type.</typeparam>
        /// <param name="path">The path to UI resources.</param>
        /// <returns>The UI element.</returns>
        public static TUIElement LoadAndCreate<TUIElement>(string path)
            where TUIElement : UIElement
        {
            TUIElement uiElement = null;

            var uiPrefab = Resources.Load<TUIElement>(path);
            if (uiPrefab != null)
            {
                uiElement = Object.Instantiate(uiPrefab);
                uiElement.name = uiPrefab.name.RemoveCloneFromName();
                uiElement.name = uiPrefab.name.AddSpaceBetweenWords();
                uiElement.transform.position = uiPrefab.transform.position;
            }

            return uiElement != null ? uiElement.GetComponent<TUIElement>() : null;
        }

        /// <summary>
        /// Adds a space between words (e.g. "AaaBbb" will be "Aaa Bbb").
        /// </summary>
        public static string AddSpaceBetweenWords(this string value)
        {
            return Regex.Replace(value, "[A-Z]", " $0").Trim();
        }

        /// <summary>
        /// Removes the "(Clone)" from name of the created game object.
        /// </summary>
        public static string RemoveCloneFromName(this string value)
        {
            return value.Replace("(Clone)", string.Empty);
        }

        /// <summary>
        /// Creates a canvas containing all of the UI elements.
        /// </summary>
        /// <param name="name">The canvas name.</param>
        /// <param name="sortingOrder">The sorting order.</param>
        /// <param name="renderMode">The render mode.</param>
        /// <param name="uiScaleMode">The scale mode.</param>
        /// <param name="matchWidthOrHeight">The match width or height.</param>
        /// <param name="alpha">The alpha.</param>
        /// <param name="interactable">Is interactable?</param>
        /// <param name="blocksRaycasts">Is blocks raycasts?</param>
        /// <returns>The canvas.</returns>
        public static Transform CreateCanvas(
            string name,
            int sortingOrder,
            RenderMode renderMode = RenderMode.ScreenSpaceOverlay,
            ScaleMode scaleMode = ScaleMode.ScaleWithScreenSize,
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
            canvasScaler.uiScaleMode = scaleMode;
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
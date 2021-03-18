using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
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
        /// <returns>The canvas.</returns>
        public static Transform CreateCanvas(UICanvasType canvasType)
        {
            var uiConfiguration =
                UIConfiguration.GetInstance().GetCanvasConfig(canvasType);
            if (uiConfiguration != null)
            {
                // Canvas Config
                var name = uiConfiguration.Name;
                var sortingOrder = uiConfiguration.SortingOrder;
                var renderMode = uiConfiguration.RenderMode;

                // Canvas Scaler Config
                var scaleMode = uiConfiguration.UICanvasScalerConfig.ScaleMode;
                var referenceResolution = uiConfiguration.UICanvasScalerConfig.ReferenceResolution;
                var screenMatchMode = uiConfiguration.UICanvasScalerConfig.ScreenMatchMode;
                var matchWidthOrHeight = uiConfiguration.UICanvasScalerConfig.MatchWidthOrHeight;
                var referencePixelsPerUnit = uiConfiguration.UICanvasScalerConfig.ReferencePixelsPerUnit;

                // UI Canvas
                var uiCanvas =
                    new GameObject(name, typeof(Canvas), typeof(CanvasScaler), typeof(GraphicRaycaster));

                // Canvas
                var canvas = uiCanvas.GetComponent<Canvas>();
                canvas.sortingOrder = sortingOrder;
                canvas.renderMode = renderMode;

                // Canvas Scaler
                var canvasScaler = uiCanvas.GetComponent<CanvasScaler>();
                canvasScaler.uiScaleMode = scaleMode;
                canvasScaler.referenceResolution = referenceResolution;
                canvasScaler.screenMatchMode = screenMatchMode;
                canvasScaler.matchWidthOrHeight = matchWidthOrHeight;
                canvasScaler.referencePixelsPerUnit = referencePixelsPerUnit;

                return uiCanvas.GetComponent<Transform>();
            }

            return null;
        }
    }
}
using UnityEngine;
using UnityEngine.EventSystems;

namespace UI
{
    /// <summary>
    /// The creator of the UI and its elements.
    /// </summary>
    public class UICreator : MonoBehaviour
    {
        public static UICreator GetInstance()
        {
            if (instance == null)
            {
                var gameObject = new GameObject("UI Creator");
                instance = gameObject.AddComponent<UICreator>();
            }

            return instance;
        }

        private static UICreator instance;
        private UICanvas uiCanvas;

        private void Awake()
        {
            var eventSystem = FindObjectOfType<EventSystem>();
            if (eventSystem == null)
            {
                var gameObject =
                    new GameObject("Event System", typeof(EventSystem));
                gameObject.AddComponent<StandaloneInputModule>();
            }
        }

        /// <summary>
        /// Loads and creates a new UI element (e.g. SampleMessage : UIElement).
        /// </summary>
        /// <typeparam name="TUIElement">The UI element type.</typeparam>
        /// <param name="uiCanvasLayer">The canvas layer.</param>
        /// <param name="uiIndex">The UI index.</param>
        /// <param name="parent">The parent of the UI element.</param>
        /// <returns>The UI element.</returns>
        public TUIElement Create<TUIElement>(
            UICanvasLayer uiCanvasLayer = UICanvasLayer.Foreground,
            UIIndex uiIndex = UIIndex.Start,
            Transform uiParent = null,
            string uiPath = UIConstants.UIElementsPath)
            where TUIElement : UIElement
        {
            if (uiCanvas == null)
            {
                var background =
                    Utils.CreateCanvas("UI Background", (int)UICanvasLayer.Background);
                var foreground =
                    Utils.CreateCanvas("UI Foreground", (int)UICanvasLayer.Foreground);

                uiCanvas = new UICanvas(background, foreground);
            }

            var name = typeof(TUIElement).Name;
            var path = string.Format(uiPath, name);
            var uiElement = Utils.LoadAndCreate<TUIElement>(path);
            if (uiElement != null)
            {
                uiElement.transform.SetParent(uiParent ?? uiCanvas.GetCanvas(uiCanvasLayer), worldPositionStays: false);

                if (uiIndex == UIIndex.Start)
                {
                    uiElement.transform.SetAsFirstSibling();
                }
                else
                {
                    uiElement.transform.SetAsLastSibling();
                }
            }

            return uiElement;
        }
    }
}
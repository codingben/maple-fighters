using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace UI.Manager
{
    public class UIElementsCreator : MonoBehaviour
    {
        public static UIElementsCreator GetInstance()
        {
            if (instance == null)
            {
                var gameObject = new GameObject("UI Elements Creator");
                instance = gameObject.AddComponent<UIElementsCreator>();
            }

            return instance;
        }

        private static UIElementsCreator instance;
        private UILayers uiLayers;

        private void Awake()
        {
            var background =
                CreateCanvas("UI Background", (int)UILayer.Background);
            var foreground =
                CreateCanvas("UI Foreground", (int)UILayer.Foreground);

            uiLayers = new UILayers(background, foreground);

            CreateEventSystem();
        }

        public TUIElement Create<TUIElement>(UILayer uiLayer = UILayer.Foreground, UIIndex uiIndex = UIIndex.Start, Transform parent = null)
            where TUIElement : UIElement
        {
            parent = parent ?? uiLayers.GetLayer(uiLayer);

            var name = typeof(TUIElement).Name;
            var uiElement = Utils.LoadAndCreate<TUIElement>($"UI/{name}");
            uiElement.transform.SetParent(parent, false);

            if (uiIndex == UIIndex.Start)
            {
                uiElement.transform.SetAsFirstSibling();
            }
            else
            {
                uiElement.transform.SetAsLastSibling();
            }

            return uiElement;
        }

        private Transform CreateCanvas(string name, int sortingOrder)
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

        private void CreateEventSystem()
        {
            var eventSystem = FindObjectOfType<EventSystem>();
            if (eventSystem == null)
            {
                var gameObject = 
                    new GameObject("Event System", typeof(EventSystem));
                gameObject.AddComponent<StandaloneInputModule>();
            }
        }
    }
}
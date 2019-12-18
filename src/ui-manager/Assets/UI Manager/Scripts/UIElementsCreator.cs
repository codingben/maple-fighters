using UnityEngine;
using UnityEngine.EventSystems;

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
        private UILayers? uiLayers;

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

        public TUIElement Create<TUIElement>(UILayer uiLayer = UILayer.Foreground, UIIndex uiIndex = UIIndex.Start, Transform parent = null)
            where TUIElement : UIElement
        {
            var name = typeof(TUIElement).Name;
            var path = string.Format(UIConstants.UIElementsPath, name);
            var uiElement = Utils.LoadAndCreate<TUIElement>(path);
            uiElement.transform.SetParent(parent ?? GetUILayer(uiLayer), false);

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

        private Transform GetUILayer(UILayer uiLayer)
        {
            if (uiLayers == null)
            {
                var background =
                    Utils.CreateCanvas("UI Background", (int)UILayer.Background);
                var foreground =
                    Utils.CreateCanvas("UI Foreground", (int)UILayer.Foreground);

                uiLayers = new UILayers(background, foreground);
            }

            return uiLayers?.GetLayer(uiLayer);
        }
    }
}
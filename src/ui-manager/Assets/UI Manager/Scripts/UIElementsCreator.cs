using UnityEngine;
using UnityEngine.EventSystems;

namespace UI.Manager
{
    public class UIElementsCreator : Singleton<UIElementsCreator>
    {
        private const string UiElementsPath = "UI/{0}";
        private const string UiBackground = "UI Background";
        private const string UiForeground = "UI Foreground";

        private UILayers uiLayers;

        private void Awake()
        {
            var background = FindOrCreateUILayer(UiBackground);
            var foreground = FindOrCreateUILayer(UiForeground);

            uiLayers = 
                new UILayers(background.transform, foreground.transform);
            
            UIUtils.CreateEventSystem<EventSystem, StandaloneInputModule>();
        }

        public TUIElement Create<TUIElement>(UILayer uiLayer = UILayer.Foreground, UIIndex uiIndex = UIIndex.Start, Transform parent = null)
            where TUIElement : UIElement
        {
            parent = parent ?? uiLayers.GetLayer(uiLayer);

            var uiElement = UIUtils.LoadAndCreateUIElement<TUIElement>();
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

        private Transform FindOrCreateUILayer(string uiLayerName)
        {
            var uiLayer = GameObject.Find(uiLayerName);
            if (uiLayer == null)
            {
                uiLayer = Utils.LoadAndCreateGameObject(
                    string.Format(UiElementsPath, uiLayerName));
            }

            return uiLayer.transform;
        }
    }
}
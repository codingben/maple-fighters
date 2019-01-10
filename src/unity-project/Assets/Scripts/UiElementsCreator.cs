using UnityEngine;
using UnityEngine.EventSystems;

namespace UserInterface
{
    public class UiElementsCreator : Singleton<UiElementsCreator>
    {
        private const string UiElementsPath = "Ui/{0}";
        private const string UiBackground = "Ui Background";
        private const string UiForeground = "Ui Foreground";

        private UiLayers uiLayers;

        private void Awake()
        {
            var background = FindOrCreateUiLayer(UiBackground);
            var foreground = FindOrCreateUiLayer(UiForeground);

            uiLayers = 
                new UiLayers(background.transform, foreground.transform);
            
            UiEventSystemCreator.GetInstance()
                .Create<EventSystem, StandaloneInputModule>();
        }

        public TUiElement Create<TUiElement>(UiLayer uiLayer = UiLayer.Foreground, UiIndex uiIndex = UiIndex.Start, Transform parent = null)
            where TUiElement : UiElement
        {
            parent = parent ?? uiLayers.GetLayer(uiLayer);

            var uiElement = UiUtils.LoadAndCreateUiElement<TUiElement>();
            uiElement.transform.SetParent(parent, false);

            if (uiIndex == UiIndex.Start)
            {
                uiElement.transform.SetAsFirstSibling();
            }
            else
            {
                uiElement.transform.SetAsLastSibling();
            }

            return uiElement;
        }

        private Transform FindOrCreateUiLayer(string uiLayerName)
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
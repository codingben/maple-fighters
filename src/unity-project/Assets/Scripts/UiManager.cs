using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace UserInterface
{
    public class UiManager : UiSingleton<UiManager>
    {
        private const string UiElementsPath = "UI/{0}";

        [SerializeField]
        private Transform backgroundLayer;

        [SerializeField]
        private Transform foregroundLayer;

        private List<UiElement> uiElements;

        private void Awake()
        {
            uiElements = new List<UiElement>();
        }
        
        private void OnDestroy()
        {
            uiElements.Clear();
        }

        public TUiElement Add<TUiElement>(
            UiLayer layer = UiLayer.Foreground,
            UiIndex index = UiIndex.Start,
            Transform parent = null)
            where TUiElement : UiElement
        {
            var uiElement = UiUtils.CreateUiElement<TUiElement>();

            uiElements.Add(uiElement);

            parent = 
                parent ?? layer == UiLayer.Background
                        ? backgroundLayer
                        : foregroundLayer;

            var rectTransform = uiElement.GetComponent<RectTransform>();
            rectTransform.SetParent(parent, true);

            if (index == UiIndex.Start)
            {
                rectTransform.SetAsFirstSibling();
            }
            else
            {
                rectTransform.SetAsLastSibling();
            }

            rectTransform.anchoredPosition = uiElement.transform.position;

            return uiElement;
        }

        public void Remove<TUiElement>(TUiElement uiElement)
            where TUiElement : UiElement
        {
            if (uiElements.Contains(uiElement))
            {
                uiElements.Remove(uiElement);

                Destroy(uiElement.gameObject);
            }
        }

        public TUiElement Get<TUiElement>()
            where TUiElement : UiElement
        {
            return uiElements.OfType<TUiElement>().FirstOrDefault();
        }
    }
}
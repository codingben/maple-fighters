using System;
using UnityEngine.EventSystems;
using UserInterface;

namespace Sample.Scripts
{
    public class BackgroundImage : UiElement, IPointerClickHandler
    {
        public event Action<PointerEventData> PointerClicked;

        public void OnPointerClick(PointerEventData eventData)
        {
            PointerClicked?.Invoke(eventData);
        }
    }
}
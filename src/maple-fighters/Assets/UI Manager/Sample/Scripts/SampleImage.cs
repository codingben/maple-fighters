using System;
using UI.Manager;
using UnityEngine.EventSystems;

namespace Sample.Scripts
{
    public class SampleImage : UIElement, IPointerClickHandler
    {
        public event Action<PointerEventData> PointerClicked;

        public void OnPointerClick(PointerEventData eventData)
        {
            PointerClicked?.Invoke(eventData);
        }
    }
}
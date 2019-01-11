using System;
using UI.Manager;
using UnityEngine.EventSystems;

namespace Scripts.UI
{
    public class FullScreenBackgroundImage : UIElement, IPointerClickHandler
    {
        public event Action PointerClick;
        
        public void OnPointerClick(PointerEventData eventData)
        {
            PointerClick?.Invoke();
        }
    }
}
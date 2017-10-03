using System;
using UnityEngine.EventSystems;

namespace Scripts.UI
{
    public class MouseDetectionBackground : UniqueUserInterfaceBase, IPointerClickHandler
    {
        public event Action MouseClicked;

        protected void Awake()
        {
            UserInterfaceContainer.Instance.AddOnly(this);
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            MouseClicked?.Invoke();
        }
    }
}
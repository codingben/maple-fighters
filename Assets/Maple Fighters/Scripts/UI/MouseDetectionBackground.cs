using System;
using Scripts.UI.Core;
using UnityEngine.EventSystems;

namespace Scripts.UI
{
    public class MouseDetectionBackground : UniqueUserInterfaceBase, IPointerClickHandler
    {
        public event Action MouseClicked;

        protected void Awake()
        {
            if (UserInterfaceContainer.Instance.Get<MouseDetectionBackground>() == null)
            {
                UserInterfaceContainer.Instance.AddOnly(this);
            }
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            MouseClicked?.Invoke();
        }
    }
}
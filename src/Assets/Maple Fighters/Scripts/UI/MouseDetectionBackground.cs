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
            if (UserInterfaceContainer.GetInstance().Get<MouseDetectionBackground>() == null)
            {
                UserInterfaceContainer.GetInstance().AddOnly(this);
            }
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            MouseClicked?.Invoke();
        }
    }
}
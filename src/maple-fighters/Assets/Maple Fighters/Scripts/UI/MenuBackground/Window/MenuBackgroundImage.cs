﻿using System;
using UI;
using UnityEngine.EventSystems;

namespace Scripts.UI.MenuBackground
{
    public class MenuBackgroundImage : UIElement,
                                       IPointerClickHandler,
                                       IMenuBackgroundView
    {
        public event Action PointerClicked;

        public void OnPointerClick(PointerEventData eventData)
        {
            PointerClicked?.Invoke();
        }
    }
}
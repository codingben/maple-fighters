using System;
using UI;
using UnityEngine;

namespace Scripts.UI.MenuBackground
{
    public class MenuBackgroundController : MonoBehaviour
    {
        public event Action BackgroundClicked;

        private IMenuBackgroundView backgroundView;

        private void Awake()
        {
            backgroundView = UICreator
                .GetInstance()
                .Create<MenuBackgroundImage>(UICanvasLayer.Background);

            if (backgroundView != null)
            {
                backgroundView.PointerClicked += OnPointerClicked;
            }
        }

        private void OnDestroy()
        {
            if (backgroundView != null)
            {
                backgroundView.PointerClicked -= OnPointerClicked;
            }
        }

        private void OnPointerClicked()
        {
            BackgroundClicked?.Invoke();
        }
    }
}
using System;
using UI.Manager;
using UnityEngine;

namespace Scripts.UI.MenuBackground
{
    public class MenuBackgroundController : MonoBehaviour
    {
        public event Action BackgroundClicked;

        private IMenuBackgroundView backgroundView;

        private void Awake()
        {
            backgroundView = UIElementsCreator.GetInstance()
                .Create<MenuBackgroundImage>(UILayer.Background);
            backgroundView.PointerClicked += OnPointerClicked;
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
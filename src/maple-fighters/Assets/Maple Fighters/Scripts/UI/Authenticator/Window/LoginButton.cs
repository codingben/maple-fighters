using System;
using UI;
using UnityEngine;
using UnityEngine.UI;

namespace Scripts.UI.Authenticator
{
    public class LoginButton : UIElement, ILoginButtonView
    {
        public event Action ButtonClicked;

        private void Start()
        {
            var button = GetComponent<Button>();
            button?.onClick.AddListener(OnButtonClicked);
        }

        private void OnDestroy()
        {
            var button = GetComponent<Button>();
            button?.onClick.RemoveListener(OnButtonClicked);
        }

        private void OnButtonClicked()
        {
            ButtonClicked?.Invoke();
        }
    }
}
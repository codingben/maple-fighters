using System;
using TMPro;
using UI.Manager;
using UnityEngine;
using UnityEngine.UI;

namespace Scripts.UI.Windows
{
    [RequireComponent(typeof(UIFadeAnimation))]
    public class LoginWindow : UIElement, ILoginView
    {
        public event Action<UIAuthenticationDetails> LoginButtonClicked;

        public event Action CreateAccountButtonClicked;

        public string Email
        {
            set
            {
                if (emailInputField != null)
                {
                    emailInputField.text = value;
                }
            }
        }

        public string Password
        {
            set
            {
                if (passwordInputField != null)
                {
                    passwordInputField.text = value;
                }
            }
        }

        [Header("Input Fields")]
        [SerializeField]
        private TMP_InputField emailInputField;

        [SerializeField]
        private TMP_InputField passwordInputField;

        [Header("Buttons")]
        [SerializeField]
        private Button loginButton;

        [SerializeField]
        private Button createAccountButton;

        private void Start()
        {
            if (loginButton != null)
            {
                loginButton.onClick.AddListener(OnLoginButtonClicked);
            }

            if (createAccountButton != null)
            {
                createAccountButton.onClick.AddListener(
                    OnCreateAccountButtonClicked);
            }
        }

        private void OnDestroy()
        {
            if (loginButton != null)
            {
                loginButton.onClick.RemoveListener(OnLoginButtonClicked);
            }

            if (createAccountButton != null)
            {
                createAccountButton.onClick.RemoveListener(
                    OnCreateAccountButtonClicked);
            }
        }

        private void OnLoginButtonClicked()
        {
            var email = emailInputField?.text;
            var password = passwordInputField?.text;

            LoginButtonClicked?.Invoke(
                new UIAuthenticationDetails(email, password));
        }

        private void OnCreateAccountButtonClicked()
        {
            CreateAccountButtonClicked?.Invoke();
        }
    }
}
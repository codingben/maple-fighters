using System;
using TMPro;
using UI.Manager;
using UnityEngine;
using UnityEngine.UI;

namespace Scripts.UI.Authenticator
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
            loginButton?.onClick.AddListener(OnLoginButtonClicked);
            createAccountButton?.onClick.AddListener(OnCreateAccountButtonClicked);
        }

        private void OnDestroy()
        {
            loginButton?.onClick.RemoveListener(OnLoginButtonClicked);
            createAccountButton?.onClick.RemoveListener(OnCreateAccountButtonClicked);
        }

        private void OnLoginButtonClicked()
        {
            var email = emailInputField?.text;
            var password = passwordInputField?.text;
            var authenticationDetails =
                new UIAuthenticationDetails(email, password);

            LoginButtonClicked?.Invoke(authenticationDetails);
        }

        private void OnCreateAccountButtonClicked()
        {
            CreateAccountButtonClicked?.Invoke();
        }
    }
}
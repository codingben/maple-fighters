using System;
using UI;
using UnityEngine;
using UnityEngine.UI;

namespace Scripts.UI.Authenticator
{
    [RequireComponent(typeof(UIFadeAnimation))]
    public class LoginWindow : UIElement, ILoginView
    {
        public event Action<UIAuthenticationDetails> LoginButtonClicked;

        public event Action CreateAccountButtonClicked;

        public event Action LoginAsGuestButtonClicked;

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
        private InputField emailInputField;

        [SerializeField]
        private InputField passwordInputField;

        [Header("Buttons")]
        [SerializeField]
        private Button loginButton;

        [SerializeField]
        private Button createAccountButton;

        [SerializeField]
        private Button loginAsGuestButton;

        private void Start()
        {
            loginButton?.onClick.AddListener(OnLoginButtonClicked);
            createAccountButton?.onClick.AddListener(OnCreateAccountButtonClicked);
            loginAsGuestButton?.onClick.AddListener(OnLoginAsgGuestButtonClicked);
        }

        private void OnDestroy()
        {
            loginButton?.onClick.RemoveListener(OnLoginButtonClicked);
            createAccountButton?.onClick.RemoveListener(OnCreateAccountButtonClicked);
            loginAsGuestButton?.onClick.RemoveListener(OnLoginAsgGuestButtonClicked);
        }

        public void EnableInteraction()
        {
            if (emailInputField != null)
            {
                emailInputField.interactable = true;
            }

            if (passwordInputField != null)
            {
                passwordInputField.interactable = true;
            }

            if (loginButton != null)
            {
                loginButton.interactable = true;
            }

            if (createAccountButton != null)
            {
                createAccountButton.interactable = true;
            }
        }

        public void DisableInteraction()
        {
            if (emailInputField != null)
            {
                emailInputField.interactable = false;
            }

            if (passwordInputField != null)
            {
                passwordInputField.interactable = false;
            }

            if (loginButton != null)
            {
                loginButton.interactable = false;
            }

            if (createAccountButton != null)
            {
                createAccountButton.interactable = false;
            }
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

        private void OnLoginAsgGuestButtonClicked()
        {
            LoginAsGuestButtonClicked?.Invoke();
        }
    }
}
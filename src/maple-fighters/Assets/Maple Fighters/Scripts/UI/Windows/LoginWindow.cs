using System;
using TMPro;
using UI.Manager;
using UnityEngine;
using UnityEngine.UI;

namespace Scripts.UI.Windows
{
    [RequireComponent(typeof(UIFadeAnimation))]
    public class LoginWindow : UIElement
    {
        public event Action<string, string> LoginButtonClicked;

        public event Action CreateAccountButtonClicked;

        public event Action<string> ShowNotice;

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

        [Header("Configuration")]
        [SerializeField]
        private int passwordCharactersLength;

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
            string message;

            if (IsEmptyEmailAddress(out message)
                || IsInvalidEmailAddress(out message)
                || IsEmptyPassword(out message)
                || IsPasswordTooShort(out message))
            {
                ShowNotice?.Invoke(message);
            }
            else
            {
                Login();
            }
        }

        private void OnCreateAccountButtonClicked()
        {
            CreateAccountButtonClicked?.Invoke();
        }

        private void Login()
        {
            if (emailInputField != null && passwordInputField != null)
            {
                var email = emailInputField.text;
                var password = passwordInputField.text;

                LoginButtonClicked?.Invoke(email, password);
            }
        }

        private bool IsEmptyEmailAddress(out string message)
        {
            message = WindowMessages.EmptyEmailAddress;

            if (emailInputField != null)
            {
                var email = emailInputField.text;

                if (string.IsNullOrWhiteSpace(email))
                {
                    return true;
                }
            }

            return false;
        }

        private bool IsInvalidEmailAddress(out string message)
        {
            message = WindowMessages.InvalidEmailAddress;

            if (emailInputField != null)
            {
                var email = emailInputField.text;

                if (WindowUtils.IsEmailAddressValid(email) == false)
                {
                    return true;
                }
            }

            return false;
        }

        private bool IsEmptyPassword(out string message)
        {
            message = WindowMessages.EmptyPassword;

            if (passwordInputField != null)
            {
                var password = passwordInputField.text;

                if (string.IsNullOrWhiteSpace(password))
                {
                    return true;
                }
            }

            return false;
        }

        private bool IsPasswordTooShort(out string message)
        {
            message = WindowMessages.ShortPassword;

            if (passwordInputField != null)
            {
                var password = passwordInputField.text;

                if (password.Length <= passwordCharactersLength)
                {
                    return true;
                }
            }

            return false;
        }
    }
}
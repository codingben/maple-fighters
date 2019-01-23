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

        public event Action RegisterButtonClicked;

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
        private int passwordCharacters;

        [Header("Input Fields")]
        [SerializeField]
        private TMP_InputField emailInputField;

        [SerializeField]
        private TMP_InputField passwordInputField;

        [Header("Buttons")]
        [SerializeField]
        private Button loginButton;

        [SerializeField]
        private Button registerButton;

        private void Start()
        {
            if (loginButton != null)
            {
                loginButton.onClick.AddListener(OnLoginButtonClicked);
            }

            if (registerButton != null)
            {
                registerButton.onClick.AddListener(OnRegisterButtonClicked);
            }
        }

        private void OnDestroy()
        {
            if (loginButton != null)
            {
                loginButton.onClick.RemoveListener(OnLoginButtonClicked);
            }

            if (registerButton != null)
            {
                registerButton.onClick.RemoveListener(OnRegisterButtonClicked);
            }
        }

        private void OnLoginButtonClicked()
        {
            string message;

            if (IsEmailInputFieldValid(out message)
                && IsPasswordInputFieldValid(out message))
            {
                Login();
            }
            else
            {
                ShowNotice?.Invoke(message);
            }
        }

        private void OnRegisterButtonClicked()
        {
            RegisterButtonClicked?.Invoke();
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

        public void ResetInputFields()
        {
            if (emailInputField != null)
            {
                emailInputField.text = string.Empty;
            }

            if (passwordInputField != null)
            {
                passwordInputField.text = string.Empty;
            }
        }

        private bool IsEmailInputFieldValid(out string message)
        {
            message = string.Empty;

            if (emailInputField != null)
            {
                if (string.IsNullOrWhiteSpace(emailInputField.text))
                {
                    message = WindowMessages.EmailAddressEmpty;
                    return false;
                }

                if (!WindowUtils.IsEmailAddressValid(emailInputField.text))
                {
                    message = WindowMessages.EmailAddressInvalid;
                    return false;
                }
            }

            return true;
        }

        private bool IsPasswordInputFieldValid(out string message)
        {
            message = string.Empty;

            if (passwordInputField != null)
            {
                if (string.IsNullOrWhiteSpace(passwordInputField.text))
                {
                    message = WindowMessages.PasswordEmpty;
                    return false;
                }

                if (passwordInputField.text.Length <= passwordCharacters)
                {
                    message = WindowMessages.PasswordNotMatch;
                    return false;
                }
            }

            return true;
        }
    }
}
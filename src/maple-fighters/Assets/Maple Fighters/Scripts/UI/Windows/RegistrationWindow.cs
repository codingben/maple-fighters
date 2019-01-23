using System;
using TMPro;
using UI.Manager;
using UnityEngine;
using UnityEngine.UI;

namespace Scripts.UI.Windows
{
    [RequireComponent(typeof(UIFadeAnimation))]
    public class RegistrationWindow : UIElement
    {
        public event Action<UIRegistrationDetails> RegisterButtonClicked;

        public event Action BackButtonClicked;

        public event Action<string> ShowNotice;

        [Header("Configuration")]
        [SerializeField]
        private int passwordCharacters;

        [SerializeField]
        private int firstLastNameCharacters;

        [Header("Input Fields")]
        [SerializeField]
        private TMP_InputField emailInputField;

        [SerializeField]
        private TMP_InputField passwordInputField;

        [SerializeField]
        private TMP_InputField confirmPasswordInputField;

        [SerializeField]
        private TMP_InputField firstNameInputField;

        [SerializeField]
        private TMP_InputField lastNameInputField;

        [Header("Buttons")]
        [SerializeField]
        private Button backButton;

        [SerializeField]
        private Button registerButton;

        private void Start()
        {
            if (backButton != null)
            {
                backButton.onClick.AddListener(OnBackButtonClicked);
            }

            if (registerButton != null)
            {
                registerButton.onClick.AddListener(OnRegisterButtonClicked);
            }
        }

        private void OnDestroy()
        {
            if (backButton != null)
            {
                backButton.onClick.RemoveListener(OnBackButtonClicked);
            }

            if (registerButton != null)
            {
                registerButton.onClick.RemoveListener(OnRegisterButtonClicked);
            }
        }

        private void OnBackButtonClicked()
        {
            BackButtonClicked?.Invoke();
        }

        private void OnRegisterButtonClicked()
        {
            string message;

            if (AreInputFieldsValid(out message))
            {
                Register();
            }
            else
            {
                ShowNotice?.Invoke(message);
            }
        }

        private void Register()
        {
            if (emailInputField != null && passwordInputField != null 
                && firstNameInputField != null && lastNameInputField != null)
            {
                var email = emailInputField.text;
                var password = passwordInputField.text;
                var firstName = firstNameInputField.text;
                var lastName = lastNameInputField.text;

                RegisterButtonClicked?.Invoke(
                    new UIRegistrationDetails(
                        email,
                        password,
                        firstName,
                        lastName));
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

            if (confirmPasswordInputField != null)
            {
                confirmPasswordInputField.text = string.Empty;
            }

            if (firstNameInputField != null)
            {
                firstNameInputField.text = string.Empty;
            }

            if (lastNameInputField != null)
            {
                lastNameInputField.text = string.Empty;
            }
        }

        private bool AreInputFieldsValid(out string message)
        {
            message = string.Empty;

            if (emailInputField != null)
            {
                if (string.IsNullOrWhiteSpace(emailInputField.text))
                {
                    message = WindowMessages.EmailAddressEmpty;
                }

                if (!WindowUtils.IsEmailAddressValid(emailInputField.text))
                {
                    message = WindowMessages.EmailAddressInvalid;
                }
            }

            if (passwordInputField != null && confirmPasswordInputField != null)
            {
                if (string.IsNullOrWhiteSpace(passwordInputField.text)
                    || string.IsNullOrWhiteSpace(confirmPasswordInputField.text))
                {
                    message = WindowMessages.PasswordsEmpty;
                }
            }

            if (passwordInputField != null)
            {
                if (passwordInputField.text.Length <= passwordCharacters)
                {
                    message = WindowMessages.ShortPassword;
                }
            }

            if (passwordInputField != null && confirmPasswordInputField != null)
            {
                if (passwordInputField.text != confirmPasswordInputField.text)
                {
                    message = WindowMessages.PasswordsNotMatch;
                }
            }

            if (firstNameInputField != null && lastNameInputField != null)
            {
                if (firstNameInputField.text.Length < firstLastNameCharacters
                    || lastNameInputField.text.Length < firstLastNameCharacters)
                {
                    message = WindowMessages.ShortFirstOrLastName;
                }
            }

            return message == string.Empty;
        }
    }
}
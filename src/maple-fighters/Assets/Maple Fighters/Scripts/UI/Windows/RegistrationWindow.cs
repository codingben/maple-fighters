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
        private TMP_InputField rePasswordInputField;

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
            if (AreInputFieldsValid())
            {
                Register();
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

            if (rePasswordInputField != null)
            {
                rePasswordInputField.text = string.Empty;
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

        private bool AreInputFieldsValid()
        {
            var isValid = true;

            if (emailInputField != null)
            {
                if (string.IsNullOrWhiteSpace(emailInputField.text))
                {
                    isValid = false;

                    ShowNotice?.Invoke("Email address can not be empty.");
                }

                if (!WindowUtils.IsEmailAddressValid(emailInputField.text))
                {
                    isValid = false;

                    ShowNotice?.Invoke("Email address is not valid.");
                }
            }

            if (passwordInputField != null && rePasswordInputField != null)
            {
                if (string.IsNullOrWhiteSpace(passwordInputField.text)
                    || string.IsNullOrWhiteSpace(rePasswordInputField.text))
                {
                    isValid = false;

                    ShowNotice?.Invoke("Passwords can not be empty.");
                }
            }

            if (passwordInputField != null)
            {
                if (passwordInputField.text.Length <= passwordCharacters)
                {
                    isValid = false;

                    ShowNotice?.Invoke("Please enter a longer password.");
                }
            }

            if (passwordInputField != null && rePasswordInputField != null)
            {
                if (passwordInputField.text != rePasswordInputField.text)
                {
                    isValid = false;

                    ShowNotice?.Invoke("Passwords are not match.");
                }
            }

            if (firstNameInputField != null && lastNameInputField != null)
            {
                if (firstNameInputField.text.Length < firstLastNameCharacters
                    || lastNameInputField.text.Length < firstLastNameCharacters)
                {
                    isValid = false;

                    ShowNotice?.Invoke("First or last name is too short.");
                }
            }

            return isValid;
        }
    }
}
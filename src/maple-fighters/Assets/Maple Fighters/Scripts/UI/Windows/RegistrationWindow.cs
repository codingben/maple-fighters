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

        public string ConfirmPassword
        {
            set
            {
                if (confirmPasswordInputField != null)
                {
                    confirmPasswordInputField.text = value;
                }
            }
        }

        public string FirstName
        {
            set
            {
                if (firstNameInputField != null)
                {
                    firstNameInputField.text = value;
                }
            }
        }

        public string LastName
        {
            set
            {
                if (lastNameInputField != null)
                {
                    lastNameInputField.text = value;
                }
            }
        }

        [Header("Configuration")]
        [SerializeField]
        private int passwordCharactersLength;

        [SerializeField]
        private int firstNameCharactersLength;

        [SerializeField]
        private int lastNameCharactersLength;

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

            if (IsEmptyEmailAddress(out message)
                || IsInvalidEmailAddress(out message)
                || IsEmptyPassword(out message)
                || IsEmptyConfirmPassword(out message)
                || IsPasswordTooShort(out message)
                || IsConfirmPasswordTooShort(out message)
                || ArePasswordsDoNotMatch(out message)
                || IsFirstNameEmpty(out message)
                || IsLastNameEmpty(out message)
                || IsFirstNameTooShort(out message)
                || IsLastNameTooShort(out message))
            {
                ShowNotice?.Invoke(message);
            }
            else
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

        private bool IsEmptyConfirmPassword(out string message)
        {
            message = WindowMessages.EmptyConfirmPassword;

            if (confirmPasswordInputField != null)
            {
                var confirmPassword = confirmPasswordInputField.text;

                if (string.IsNullOrWhiteSpace(confirmPassword))
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

        private bool IsConfirmPasswordTooShort(out string message)
        {
            message = WindowMessages.ShortPassword;

            if (confirmPasswordInputField != null)
            {
                var confirmPassword = confirmPasswordInputField.text;

                if (confirmPassword.Length <= passwordCharactersLength)
                {
                    return true;
                }
            }

            return false;
        }

        private bool ArePasswordsDoNotMatch(out string message)
        {
            message = WindowMessages.PasswordsDoNotMatch;

            if (passwordInputField != null && confirmPasswordInputField != null)
            {
                var password = passwordInputField.text;
                var confirmPassword = confirmPasswordInputField.text;

                if (password != confirmPassword)
                {
                    return true;
                }
            }

            return false;
        }

        private bool IsFirstNameEmpty(out string message)
        {
            message = WindowMessages.EmptyFirstName;

            if (firstNameInputField != null)
            {
                var firstName = firstNameInputField.text;

                if (string.IsNullOrWhiteSpace(firstName))
                {
                    return true;
                }
            }

            return false;
        }

        private bool IsLastNameEmpty(out string message)
        {
            message = WindowMessages.EmptyLastName;

            if (lastNameInputField != null)
            {
                var lastName = lastNameInputField.text;

                if (string.IsNullOrWhiteSpace(lastName))
                {
                    return true;
                }
            }

            return false;
        }

        private bool IsFirstNameTooShort(out string message)
        {
            message = WindowMessages.ShortFirstName;

            if (firstNameInputField != null)
            {
                var firstName = firstNameInputField.text;

                if (firstName.Length < firstNameCharactersLength)
                {
                    return true;
                }
            }

            return false;
        }

        private bool IsLastNameTooShort(out string message)
        {
            message = WindowMessages.ShortLastName;

            if (lastNameInputField != null)
            {
                var lastName = lastNameInputField.text;

                if (lastName.Length < lastNameCharactersLength)
                {
                    return true;
                }
            }

            return false;
        }
    }
}
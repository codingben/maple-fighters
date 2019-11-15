using System;
using TMPro;
using UI.Manager;
using UnityEngine;
using UnityEngine.UI;

namespace Scripts.UI.Authenticator
{
    [RequireComponent(typeof(UIFadeAnimation))]
    public class RegistrationWindow : UIElement, IRegistrationView
    {
        public event Action<UIRegistrationDetails> RegisterButtonClicked;

        public event Action BackButtonClicked;

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
            backButton?.onClick.AddListener(OnBackButtonClicked);
            registerButton?.onClick.AddListener(OnRegisterButtonClicked);
        }

        private void OnDestroy()
        {
            backButton?.onClick.RemoveListener(OnBackButtonClicked);
            registerButton?.onClick.RemoveListener(OnRegisterButtonClicked);
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

            if (confirmPasswordInputField != null)
            {
                confirmPasswordInputField.interactable = true;
            }

            if (firstNameInputField != null)
            {
                firstNameInputField.interactable = true;
            }

            if (lastNameInputField != null)
            {
                lastNameInputField.interactable = true;
            }

            if (backButton != null)
            {
                backButton.interactable = true;
            }

            if (registerButton != null)
            {
                registerButton.interactable = true;
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

            if (confirmPasswordInputField != null)
            {
                confirmPasswordInputField.interactable = false;
            }

            if (firstNameInputField != null)
            {
                firstNameInputField.interactable = false;
            }

            if (lastNameInputField != null)
            {
                lastNameInputField.interactable = false;
            }

            if (backButton != null)
            {
                backButton.interactable = false;
            }

            if (registerButton != null)
            {
                registerButton.interactable = false;
            }
        }

        private void OnBackButtonClicked()
        {
            BackButtonClicked?.Invoke();
        }

        private void OnRegisterButtonClicked()
        {
            var email = emailInputField?.text;
            var password = passwordInputField?.text;
            var confirmPassword = confirmPasswordInputField?.text;
            var firstName = firstNameInputField?.text;
            var lastName = lastNameInputField?.text;
            var registrationDetails = new UIRegistrationDetails(
                email,
                password,
                confirmPassword,
                firstName,
                lastName);

            RegisterButtonClicked?.Invoke(registrationDetails);
        }
    }
}
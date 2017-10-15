using System;
using Scripts.UI.Core;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Scripts.UI.Windows
{
    public class RegistrationWindow : UserInterfaceWindowFadeEffect
    {
        public Action ResetInputFields;

        public event Action<string, string, string, string> RegisterButtonClicked;
        public event Action BackButtonClicked;
        public event Action<string> ShowNotice;

        [Header("Configuration")]
        [SerializeField] private int passwordCharacters;
        [SerializeField] private int firstLastNameCharacters;
        [Header("Input Fields")]
        [SerializeField] private TMP_InputField emailInputField;
        [SerializeField] private TMP_InputField passwordInputField;
        [SerializeField] private TMP_InputField rePasswordInputField;
        [SerializeField] private TMP_InputField firstNameInputField;
        [SerializeField] private TMP_InputField lastNameInputField;
        [Header("Buttons")]
        [SerializeField] private Button backButton;
        [SerializeField] private Button registerButton;

        private void Start()
        {
            ResetInputFields = OnResetInputFields;

            backButton.onClick.AddListener(OnBackButtonClicked);
            registerButton.onClick.AddListener(OnRegisterButtonClicked);
        }

        private void OnDestroy()
        {
            backButton.onClick.RemoveListener(OnBackButtonClicked);
            registerButton.onClick.RemoveListener(OnRegisterButtonClicked);

            ShowNotice = null;
        }

        private void OnBackButtonClicked()
        {
            Hide();

            BackButtonClicked?.Invoke();
        }

        private void OnRegisterButtonClicked()
        {
            Hide();

            if (!AcceptInputFieldsContent())
            {
                return;
            }

            Register();
        }

        private void Register()
        {
            var email = emailInputField.text;
            var password = passwordInputField.text;
            var firstName = firstNameInputField.text;
            var lastName = lastNameInputField.text;

            RegisterButtonClicked?.Invoke(email, password, firstName, lastName);
        }

        private void OnResetInputFields()
        {
            emailInputField.text = string.Empty;
            passwordInputField.text = string.Empty;
            rePasswordInputField.text = string.Empty;
            firstNameInputField.text = string.Empty;
            lastNameInputField.text = string.Empty;
        }

        private bool AcceptInputFieldsContent()
        {
            if (emailInputField.text == string.Empty)
            {
                ShowNotice?.Invoke("Email address can not be empty.");
                return false;
            }

            if (!emailInputField.text.IsValidEmailAddress())
            {
                ShowNotice?.Invoke("Email address is not valid.");
                return false;
            }

            if (passwordInputField.text == string.Empty || rePasswordInputField.text == string.Empty)
            {
                ShowNotice?.Invoke("Passwords can not be empty.");
                return false;
            }

            if (passwordInputField.text.Length <= passwordCharacters)
            {
                ShowNotice?.Invoke("Please enter a longer password.");
                return false;
            }

            if (firstNameInputField.text.Length < firstLastNameCharacters ||
                lastNameInputField.text.Length < firstLastNameCharacters)
            {
                ShowNotice?.Invoke("First or last name is too short.");
                return false;
            }

            if (passwordInputField.text != rePasswordInputField.text)
            {
                ShowNotice?.Invoke("Passwords are not match.");
                return false;
            }

            return true;
        }
    }
}
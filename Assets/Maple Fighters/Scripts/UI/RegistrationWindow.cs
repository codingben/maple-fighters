using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Scripts.UI
{
    public class RegistrationWindow : UserInterfaceWindowFadeEffect
    {
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
            backButton.onClick.AddListener(OnBackButtonClicked);
            registerButton.onClick.AddListener(OnRegisterButtonClicked);
        }

        private void OnBackButtonClicked()
        {
            Hide();
            ResetInputFields();

            BackButtonClicked?.Invoke();
        }

        private void OnRegisterButtonClicked()
        {
            if (emailInputField.text == string.Empty)
            {
                ShowNotice?.Invoke("Email address can not be empty.");
                return;
            }

            if (!emailInputField.text.IsValidEmailAddress())
            {
                ShowNotice?.Invoke("Email address is not valid.");
                return;
            }

            if (passwordInputField.text == string.Empty || rePasswordInputField.text == string.Empty)
            {
                ShowNotice?.Invoke("Passwords can not be empty.");
                return;
            }

            if (passwordInputField.text.Length <= passwordCharacters)
            {
                ShowNotice?.Invoke("Please enter a longer password.");
                return;
            }

            if (firstNameInputField.text.Length < firstLastNameCharacters || 
                lastNameInputField.text.Length < firstLastNameCharacters)
            {
                ShowNotice?.Invoke("First or last name is too short.");
                return;
            }

            if (passwordInputField.text != rePasswordInputField.text)
            {
                ShowNotice?.Invoke("Passwords are not match.");
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

        private void ResetInputFields()
        {
            emailInputField.text = string.Empty;
            passwordInputField.text = string.Empty;
            rePasswordInputField.text = string.Empty;
            firstNameInputField.text = string.Empty;
            lastNameInputField.text = string.Empty;
        }
    }
}
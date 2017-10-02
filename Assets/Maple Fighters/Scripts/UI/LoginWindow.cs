using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Scripts.UI
{
    public class LoginWindow : UserInterfaceWindowFadeEffect
    {
        public event Action<string, string> LoginButtonClicked;
        public event Action RegisterButtonClicked;
        public event Action<string> ShowNotice;

        [Header("Configuration")]
        [SerializeField] private int passwordCharacters;
        [Header("Input Fields")]
        [SerializeField] private TMP_InputField emailInputField;
        [SerializeField] private TMP_InputField passwordInputField;
        [Header("Buttons")]
        [SerializeField] private Button loginButton;
        [SerializeField] private Button registerButton;

        private void Start()
        {
            loginButton.onClick.AddListener(OnLoginButtonClicked);
            registerButton.onClick.AddListener(OnRegisterButtonClicked);
        }

        private void OnLoginButtonClicked()
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

            if (passwordInputField.text == string.Empty)
            {
                ShowNotice?.Invoke("Password can not be empty.");
                return;
            }

            if (passwordInputField.text.Length <= passwordCharacters)
            {
                ShowNotice?.Invoke("Password is not match.");
                return;
            }

            Login();
        }

        private void OnRegisterButtonClicked()
        {
            Hide();
            ResetInputFields();

            RegisterButtonClicked?.Invoke();
        }

        private void Login()
        {
            var email = emailInputField.text;
            var password = passwordInputField.text;

            LoginButtonClicked?.Invoke(email, password);
        }

        private void ResetInputFields()
        {
            emailInputField.text = string.Empty;
            passwordInputField.text = string.Empty;
        }
    }
}
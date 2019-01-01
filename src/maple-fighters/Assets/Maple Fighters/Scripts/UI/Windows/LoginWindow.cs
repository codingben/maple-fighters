using System;
using Scripts.UI.Core;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Scripts.UI.Windows
{
    public class LoginWindow : UserInterfaceWindowFadeEffect
    {
        public string Email
        {
            set
            {
                emailInputField.text = value;
            }
        }

        public string Password
        {
            set
            {
                passwordInputField.text = value;
            }
        }

        public event Action<string> ShowNotice;
        public event Action<string, string> LoginButtonClicked;
        public event Action RegisterButtonClicked;

        [Header("Configuration")]
        [SerializeField] private int passwordCharacters;
        [Header("Input Fields")]
        [SerializeField] private TMP_InputField emailInputField;
        [SerializeField] private TMP_InputField passwordInputField;
        [Header("Buttons")]
        [SerializeField] private Button loginButton;
        [SerializeField] private Button registerButton;

        private bool isInitialized;

        public override void Show()
        {
            if (!isInitialized)
            {
                isInitialized = true;

                var oldShowSpeed = showSpeed;
                showSpeed /= 5;

                Show(() => { showSpeed = oldShowSpeed; });
            }
            else
            {
                base.Show();
            }
        }
        
        private void Start()
        {
            loginButton.onClick.AddListener(OnLoginButtonClicked);
            registerButton.onClick.AddListener(OnRegisterButtonClicked);
        }

        private void OnDestroy()
        {
            loginButton.onClick.RemoveListener(OnLoginButtonClicked);
            registerButton.onClick.RemoveListener(OnRegisterButtonClicked);

            ShowNotice = null;
        }

        private void OnLoginButtonClicked()
        {
            if (AcceptInputFieldsContent())
            {
                Login();
            }
            else
            {
                Hide();
            }
        }

        private void OnRegisterButtonClicked()
        {
            RegisterButtonClicked?.Invoke();
        }

        private void Login()
        {
            var email = emailInputField.text;
            var password = passwordInputField.text;

            LoginButtonClicked?.Invoke(email, password);
        }

        public void ResetInputFields()
        {
            emailInputField.text = string.Empty;
            passwordInputField.text = string.Empty;
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

            if (passwordInputField.text == string.Empty)
            {
                ShowNotice?.Invoke("Password can not be empty.");
                return false;
            }

            if (passwordInputField.text.Length <= passwordCharacters)
            {
                ShowNotice?.Invoke("Password is not match.");
                return false;
            }

            return true;
        }
    }
}
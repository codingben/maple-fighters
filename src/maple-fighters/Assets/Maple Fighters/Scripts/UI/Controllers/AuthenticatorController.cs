using System;
using Scripts.UI.Windows;
using UI.Manager;
using UnityEngine;

namespace Scripts.UI.Controllers
{
    public class AuthenticatorController : MonoBehaviour
    {
        public event Action<UIAuthenticationDetails> Login;

        public event Action<UIRegistrationDetails> Register;

        private LoginWindow loginWindow;
        private RegistrationWindow registrationWindow;

        private void Awake()
        {
            CreateLoginWindow();
            CreateRegistrationWindow();
        }

        private void Start()
        {
            ShowLoginWindow();
        }

        private void CreateLoginWindow()
        {
            loginWindow = UIElementsCreator.GetInstance().Create<LoginWindow>();
            loginWindow.LoginButtonClicked += OnLoginButtonClicked;
            loginWindow.CreateAccountButtonClicked +=
                OnCreateAccountButtonClicked;
            loginWindow.ShowNotice += OnShowNotice;
        }

        private void CreateRegistrationWindow()
        {
            registrationWindow = UIElementsCreator.GetInstance()
                .Create<RegistrationWindow>();
            registrationWindow.RegisterButtonClicked +=
                OnRegisterButtonClicked;
            registrationWindow.BackButtonClicked += OnBackButtonClicked;
            registrationWindow.ShowNotice += OnShowNotice;
        }

        private void OnDestroy()
        {
            DestroyLoginWindow();
            DestroyRegistrationWindow();
        }

        private void DestroyLoginWindow()
        {
            if (loginWindow != null)
            {
                loginWindow.LoginButtonClicked -= OnLoginButtonClicked;
                loginWindow.CreateAccountButtonClicked -=
                    OnCreateAccountButtonClicked;

                Destroy(loginWindow.gameObject);
            }
        }

        private void DestroyRegistrationWindow()
        {
            if (registrationWindow != null)
            {
                registrationWindow.RegisterButtonClicked -=
                    OnRegisterButtonClicked;
                registrationWindow.BackButtonClicked -= OnBackButtonClicked;

                Destroy(registrationWindow.gameObject);
            }
        }

        private void OnShowNotice(string message)
        {
            // TODO: Use event bus system
            var noticeController = FindObjectOfType<NoticeController>();
            if (noticeController != null)
            {
                noticeController.Show(message);
            }
        }

        private void OnLoginButtonClicked(
            UIAuthenticationDetails authenticationDetails)
        {
            Login?.Invoke(authenticationDetails);
        }

        private void OnCreateAccountButtonClicked()
        {
            HideLoginWindow();
            ShowRegistrationWindow();
        }

        private void OnRegisterButtonClicked(
            UIRegistrationDetails uiRegistrationDetails)
        {
            Register?.Invoke(uiRegistrationDetails);
        }

        private void OnBackButtonClicked()
        {
            HideRegistrationWindow();
            ShowLoginWindow();
        }

        private void ShowLoginWindow()
        {
            if (loginWindow != null)
            {
                loginWindow.Show();
            }
        }

        private void HideLoginWindow()
        {
            if (loginWindow != null)
            {
                loginWindow.Email = string.Empty;
                loginWindow.Password = string.Empty;
                loginWindow.Hide();
            }
        }

        private void ShowRegistrationWindow()
        {
            if (registrationWindow != null)
            {
                registrationWindow.Show();
            }
        }

        private void HideRegistrationWindow()
        {
            if (registrationWindow != null)
            {
                registrationWindow.Email = string.Empty;
                registrationWindow.Password = string.Empty;
                registrationWindow.ConfirmPassword = string.Empty;
                registrationWindow.FirstName = string.Empty;
                registrationWindow.LastName = string.Empty;
                registrationWindow.Hide();
            }
        }
    }
}
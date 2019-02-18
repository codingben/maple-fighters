using System;
using Scripts.UI.Windows;
using UI.Manager;
using UnityEngine;

namespace Scripts.UI.Controllers
{
    public class AuthenticatorController : MonoBehaviour
    {
        public event Action<string, string> Login;

        public event Action<UIRegistrationDetails> Register;

        private LoginWindow loginWindow;
        private RegistrationWindow registrationWindow;

        private void Awake()
        {
            loginWindow = UIElementsCreator.GetInstance().Create<LoginWindow>();
            loginWindow.LoginButtonClicked += OnLoginButtonClicked;
            loginWindow.CreateAccountButtonClicked +=
                OnCreateAccountButtonClicked;
            loginWindow.ShowNotice += OnShowNotice;
            loginWindow.Show();
        }

        private void OnDestroy()
        {
            if (loginWindow != null)
            {
                loginWindow.LoginButtonClicked -= OnLoginButtonClicked;
                loginWindow.CreateAccountButtonClicked -= 
                    OnCreateAccountButtonClicked;

                Destroy(loginWindow.gameObject);
            }

            if (registrationWindow != null)
            {
                registrationWindow.RegisterButtonClicked -= OnRegisterButtonClicked;
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

        private void OnLoginButtonClicked(string email, string password)
        {
            Login?.Invoke(email, password);
        }

        private void OnCreateAccountButtonClicked()
        {
            if (loginWindow != null)
            {
                loginWindow.Hide();
                loginWindow.ResetInputFields();
            }

            if (registrationWindow == null)
            {
                registrationWindow = UIElementsCreator.GetInstance()
                    .Create<RegistrationWindow>();
                registrationWindow.RegisterButtonClicked +=
                    OnRegisterButtonClicked;
                registrationWindow.BackButtonClicked += OnBackButtonClicked;
                registrationWindow.ShowNotice += OnShowNotice;
                registrationWindow.Show();
            }
            else
            {
                registrationWindow.Show();
            }
        }

        private void OnRegisterButtonClicked(
            UIRegistrationDetails uiRegistrationDetails)
        {
            Register?.Invoke(uiRegistrationDetails);
        }

        private void OnBackButtonClicked()
        {
            if (registrationWindow != null)
            {
                registrationWindow.Hide();
                registrationWindow.ResetInputFields();
            }

            if (loginWindow != null)
            {
                loginWindow.Show();
            }
        }
    }
}
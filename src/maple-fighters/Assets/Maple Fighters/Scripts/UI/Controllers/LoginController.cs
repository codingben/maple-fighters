using System;
using Scripts.UI.Windows;
using UI.Manager;
using UnityEngine;

namespace Scripts.UI.Controllers
{
    public class LoginController : MonoBehaviour
    {
        public event Action<string, string> Login;

        public event Action Register;

        private LoginWindow loginWindow;

        private void Awake()
        {
            loginWindow = UIElementsCreator.GetInstance().Create<LoginWindow>();
            loginWindow.LoginButtonClicked += OnLoginButtonClicked;
            loginWindow.RegisterButtonClicked += OnRegisterButtonClicked;
            loginWindow.ShowNotice += OnShowNotice;
            loginWindow.Show();
        }

        private void Start()
        {
            // TODO: Use event bus system
            var registrationController =
                FindObjectOfType<RegistrationController>();
            if (registrationController != null)
            {
                registrationController.Back += OnBack;
            }
        }

        private void OnDestroy()
        {
            if (loginWindow != null)
            {
                loginWindow.LoginButtonClicked -= OnLoginButtonClicked;
                loginWindow.RegisterButtonClicked -= OnRegisterButtonClicked;

                Destroy(loginWindow.gameObject);
            }

            // TODO: Use event bus system
            var registrationController =
                FindObjectOfType<RegistrationController>();
            if (registrationController != null)
            {
                registrationController.Back -= OnBack;
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

        private void OnBack()
        {
            loginWindow.Show();
        }

        private void OnLoginButtonClicked(string email, string password)
        {
            if (LoginConnectionProvider.GetInstance().IsConnected())
            {
                Login?.Invoke(email, password);
            }
            else
            {
                LoginConnectionProvider.GetInstance()
                    .Connect(() => Login?.Invoke(email, password));
            }
        }

        private void OnRegisterButtonClicked()
        {
            if (loginWindow != null)
            {
                loginWindow.Hide();
                loginWindow.ResetInputFields();
            }

            Register?.Invoke();
        }
    }
}
using System;
using Scripts.UI.Windows;
using Scripts.Utils;
using UI.Manager;

namespace Scripts.UI.Controllers
{
    public class LoginController : MonoSingleton<LoginController>
    {
        public event Action<string, string> Login;

        public event Action Register;

        private LoginWindow loginWindow;

        protected override void OnAwake()
        {
            base.OnAwake();

            loginWindow = UIElementsCreator.GetInstance().Create<LoginWindow>();
            loginWindow.LoginButtonClicked += OnLoginButtonClicked;
            loginWindow.RegisterButtonClicked += OnRegisterButtonClicked;
            loginWindow.ShowNotice += OnShowNotice;
            loginWindow.Show();
        }

        private void Start()
        {
            RegistrationController.GetInstance().Back += OnBack;
        }

        protected override void OnDestroying()
        {
            base.OnDestroying();

            if (loginWindow != null)
            {
                RegistrationController.GetInstance().Back -= OnBack;

                loginWindow.LoginButtonClicked -= OnLoginButtonClicked;
                loginWindow.RegisterButtonClicked -= OnRegisterButtonClicked;

                Destroy(loginWindow.gameObject);
            }
        }

        private void OnShowNotice(string message)
        {
            NoticeController.GetInstance().Show(message);
        }

        private void OnBack()
        {
            loginWindow.Show();
        }

        private void OnLoginButtonClicked(string email, string password)
        {
            if (loginWindow != null)
            {
                loginWindow.Hide();
            }

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
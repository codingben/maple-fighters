using System;
using Scripts.UI.Windows;
using Scripts.Utils;
using UI.Manager;

namespace Scripts.UI.Controllers
{
    public class LoginController : MonoSingleton<LoginController>
    {
        public event Action<string, string> LoginButtonClicked;

        public event Action RegisterButtonClicked;

        private LoginWindow loginWindow;

        protected override void OnAwake()
        {
            base.OnAwake();

            loginWindow = UIElementsCreator.GetInstance().Create<LoginWindow>();
            loginWindow.LoginButtonClicked += OnLoginButtonClicked;
            loginWindow.RegisterButtonClicked += OnRegisterButtonClicked;
            loginWindow.Show();
        }

        protected override void OnDestroying()
        {
            base.OnDestroying();

            if (loginWindow != null)
            {
                loginWindow.LoginButtonClicked -= OnLoginButtonClicked;
                loginWindow.RegisterButtonClicked -= OnRegisterButtonClicked;

                Destroy(loginWindow.gameObject);
            }
        }

        private void OnLoginButtonClicked(string email, string password)
        {
            if (loginWindow != null)
            {
                loginWindow.Hide();
            }

            if (LoginConnectionProvider.GetInstance().IsConnected())
            {
                LoginButtonClicked?.Invoke(email, password);
            }
            else
            {
                LoginConnectionProvider.GetInstance()
                    .Connect(() => LoginButtonClicked?.Invoke(email, password));
            }
        }

        private void OnRegisterButtonClicked()
        {
            if (loginWindow != null)
            {
                loginWindow.Hide();
                loginWindow.ResetInputFields();
            }

            RegisterButtonClicked?.Invoke();
        }
    }
}
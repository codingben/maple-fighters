using System;
using Scripts.UI.Windows;
using Scripts.Utils;
using UI.Manager;
using UnityEngine;

namespace Scripts.UI.Controllers
{
    public class LoginController : MonoSingleton<LoginController>
    {
        public event Action<string, string> LoginButtonClicked;

        public event Action RegisterButtonClicked;

        [SerializeField]
        private int loadSceneIndex;

        private LoginWindow loginWindow;

        protected override void OnAwake()
        {
            base.OnAwake();

            loginWindow = UIElementsCreator.GetInstance().Create<LoginWindow>();
            loginWindow.LoginButtonClicked += OnLoginButtonClicked;
            loginWindow.RegisterButtonClicked += OnRegisterButtonClicked;
            // loginWindow.ShowNotice += (message) => Utils.ShowNotice(message, okButtonClicked: () => loginWindow.Show());
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

            /*var noticeWindow = Utils.ShowNotice(
                message: "Logging in... Please wait.", 
                okButtonClicked: () =>
                {
                    loginWindow.Show();
                });

            noticeWindow.OkButton.interactable = false;*/

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
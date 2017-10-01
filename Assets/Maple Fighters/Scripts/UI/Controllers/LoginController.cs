using CommonTools.Coroutines;
using CommonTools.Log;
using Login.Common;
using Scripts.Containers.Service;
using UnityEngine;

namespace Scripts.UI.Controllers
{
    public class LoginController : MonoBehaviour
    {
        private RegistrationWindow registrationWindow;
        private LoginWindow loginWindow;

        private ExternalCoroutinesExecutor coroutinesExecutor;

        private void Start()
        {
            coroutinesExecutor = new ExternalCoroutinesExecutor();
            loginWindow = UserInterfaceContainer.Instance.Add<LoginWindow>();

            SubscribeToLoginWindowEvents();
        }

        private void Update()
        {
            coroutinesExecutor?.Update();
        }

        private void OnDestroy()
        {
            coroutinesExecutor.Dispose();

            UnsubscribeFromRegistrationWindowEvents();

            UserInterfaceContainer.Instance.Remove(loginWindow);
        }

        private void SubscribeToLoginWindowEvents()
        {
            loginWindow.LoginButtonClicked += OnLoginButtonClicked;
            loginWindow.RegisterButtonClicked += OnRegisterButtonClicked;
        }

        private void UnsubscribeFromRegistrationWindowEvents()
        {
            loginWindow.LoginButtonClicked -= OnLoginButtonClicked;
            loginWindow.RegisterButtonClicked -= OnRegisterButtonClicked;
        }

        private void OnLoginButtonClicked(string email, string password)
        {
            var parameters = new LoginRequestParameters(email, password);
            coroutinesExecutor.StartTask((y) => ServiceContainer.LoginService.Login(y, parameters));
        }

        private void OnRegisterButtonClicked()
        {
            if (registrationWindow == null)
            {
                registrationWindow = UserInterfaceContainer.Instance.Get<RegistrationWindow>().AssertNotNull();
            }

            registrationWindow.Show();
        }
    }
}
using CommonTools.Coroutines;
using CommonTools.Log;
using Registration.Common;
using Scripts.Containers.Service;
using UnityEngine;

namespace Scripts.UI.Controllers
{
    public class RegistrationController : MonoBehaviour
    {
        private RegistrationWindow registrationWindow;
        private LoginWindow loginWindow;

        private ExternalCoroutinesExecutor coroutinesExecutor;

        private void Start()
        {
            coroutinesExecutor = new ExternalCoroutinesExecutor();
            registrationWindow = UserInterfaceContainer.Instance.Add<RegistrationWindow>();

            SubscribeToRegistrationWindowEvents();
        }

        private void Update()
        {
            coroutinesExecutor?.Update();
        }

        private void OnDestroy()
        {
            coroutinesExecutor.Dispose();

            UnsubscribeFromRegistrationWindowEvents();

            UserInterfaceContainer.Instance.Remove(registrationWindow);
        }

        private void SubscribeToRegistrationWindowEvents()
        {
            registrationWindow.RegisterButtonClicked += OnRegisterButtonClicked;
            registrationWindow.BackButtonClicked += OnBackButtonClicked;
            registrationWindow.ShowNotice += OnShowNotice;
        }

        private void UnsubscribeFromRegistrationWindowEvents()
        {
            registrationWindow.RegisterButtonClicked -= OnRegisterButtonClicked;
            registrationWindow.BackButtonClicked -= OnBackButtonClicked;
            registrationWindow.ShowNotice -= OnShowNotice;
        }

        private void OnRegisterButtonClicked(string email, string password, string firstName, string lastName)
        {
            var parameters = new RegisterRequestParameters(email, password, firstName, lastName);
            coroutinesExecutor.StartTask((y) => ServiceContainer.RegistrationService.Register(y, parameters));
        }

        private void OnBackButtonClicked()
        {
            if (loginWindow == null)
            {
                loginWindow = UserInterfaceContainer.Instance.Get<LoginWindow>().AssertNotNull();
            }

            loginWindow.Show();
        }

        protected void OnShowNotice(string message)
        {
            registrationWindow.Hide();

            var noticeWindow = UserInterfaceContainer.Instance.Add<NoticeWindow>();
            noticeWindow.Initialize(message, delegate { registrationWindow.Show(); });
            noticeWindow.Show();
        }
    }
}
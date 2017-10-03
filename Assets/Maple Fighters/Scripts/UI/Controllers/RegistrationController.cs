using System;
using System.Threading.Tasks;
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
            registrationWindow.ShowNotice += ShowNotice;
        }

        private void UnsubscribeFromRegistrationWindowEvents()
        {
            registrationWindow.RegisterButtonClicked -= OnRegisterButtonClicked;
            registrationWindow.BackButtonClicked -= OnBackButtonClicked;
            registrationWindow.ShowNotice -= ShowNotice;
        }

        private void OnRegisterButtonClicked(string email, string password, string firstName, string lastName)
        {
            if (!ServiceContainer.RegistrationService.IsConnected())
            {
                ShowNotice("Could not connect to a server.");
                return;
            }

            var parameters = new RegisterRequestParameters(email, password, firstName, lastName);
            coroutinesExecutor.StartTask((y) => Register(y, parameters));
        }

        private async Task Register(IYield yield, RegisterRequestParameters paramters)
        {
            var noticeWindow = ShowCustomNotice("Registration is in a process, please wait.", delegate { registrationWindow.Show(); });
            noticeWindow.OkButton.interactable = false;

            var responseParameters = await ServiceContainer.RegistrationService.Register(yield, paramters);

            switch (responseParameters.Status)
            {
                case RegisterStatus.Succeed:
                {
                    noticeWindow.Message.text = "Registration is completed successfully.";
                    noticeWindow.OkButtonClicked = OnBackButtonClicked;
                    noticeWindow.OkButton.interactable = true;
                    break;
                }
                case RegisterStatus.EmailExists:
                {
                    noticeWindow.Message.text = "Email address already exists.";
                    noticeWindow.OkButton.interactable = true;
                    break;
                }
                default:
                {
                    noticeWindow.Message.text = "Something went wrong, please try again.";
                    noticeWindow.OkButton.interactable = true;
                    break;
                }
            }
        }

        private void OnBackButtonClicked()
        {
            if (loginWindow == null)
            {
                loginWindow = UserInterfaceContainer.Instance.Get<LoginWindow>().AssertNotNull();
            }

            loginWindow.Show();
        }

        private void ShowNotice(string message)
        {
            registrationWindow.Hide();

            var noticeWindow = UserInterfaceContainer.Instance.Add<NoticeWindow>();
            noticeWindow.Initialize(message, delegate { registrationWindow.Show(); });
            noticeWindow.Show();
        }

        private NoticeWindow ShowCustomNotice(string message, Action okButtonClicked)
        {
            registrationWindow.Hide();

            var noticeWindow = UserInterfaceContainer.Instance.Add<NoticeWindow>();
            noticeWindow.Initialize(message, okButtonClicked);
            noticeWindow.Show();

            return noticeWindow;
        }
    }
}
using System.Collections.Generic;
using System.Threading.Tasks;
using CommonTools.Coroutines;
using CommonTools.Log;
using Registration.Common;
using Scripts.Containers;
using Scripts.Services;
using Scripts.UI.Core;
using Scripts.UI.Windows;
using UnityEngine;
using WaitForSeconds = CommonTools.Coroutines.WaitForSeconds;

namespace Scripts.UI.Controllers
{
    public class RegistrationController : MonoBehaviour
    {
        private const int AUTO_TIME_FOR_DISCONNECT = 60;

        private RegistrationWindow registrationWindow;
        private LoginWindow loginWindow;

        private ICoroutine disconnectAutomatically;
        private readonly ExternalCoroutinesExecutor coroutinesExecutor = new ExternalCoroutinesExecutor();

        private void Start()
        {
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
            registrationWindow.ShowNotice += (message) => Utils.ShowNotice(message, () => registrationWindow.Show());
        }

        private void UnsubscribeFromRegistrationWindowEvents()
        {
            registrationWindow.RegisterButtonClicked -= OnRegisterButtonClicked;
            registrationWindow.BackButtonClicked -= OnBackButtonClicked;
        }

        private void OnRegisterButtonClicked(string email, string password, string firstName, string lastName)
        {
            var parameters = new RegisterRequestParameters(email, password, firstName, lastName);
            coroutinesExecutor.StartTask((y) => Connect(y, parameters));
        }

        private async Task Connect(IYield yield, RegisterRequestParameters parameters)
        {
            var noticeWindow = Utils.ShowNotice("Registration is in a process.. Please wait.", () => registrationWindow.Show());
            noticeWindow.OkButton.interactable = false;

            if (!ServiceContainer.RegistrationService.IsConnected())
            {
                var connectionStatus = await ServiceContainer.RegistrationService.Connect(yield);
                if (connectionStatus == ConnectionStatus.Failed)
                {
                    noticeWindow.Message.text = "Could not connect to a registration server.";
                    noticeWindow.OkButton.interactable = true;
                    return;
                }
            }

            coroutinesExecutor.StartTask((y) => Register(y, parameters));
        }

        private async Task Register(IYield yield, RegisterRequestParameters paramters)
        {
            var noticeWindow = UserInterfaceContainer.Instance.Get<NoticeWindow>().AssertNotNull();
            var responseParameters = await ServiceContainer.RegistrationService.Register(yield, paramters);

            switch (responseParameters.Status)
            {
                case RegisterStatus.Succeed:
                {
                    noticeWindow.Message.text = "Registration is completed successfully.";
                    noticeWindow.OkButtonClickedAction = OnBackButtonClicked;
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

            if (responseParameters.Status != RegisterStatus.Succeed)
            {
                if (disconnectAutomatically == null)
                {
                    disconnectAutomatically = coroutinesExecutor.StartCoroutine(DisconnectAutomatically());
                }
            }
            else
            {
                Disconnect();
            }
        }

        private void OnBackButtonClicked()
        {
            registrationWindow.ResetInputFields.Invoke();

            if (loginWindow == null)
            {
                loginWindow = UserInterfaceContainer.Instance.Get<LoginWindow>().AssertNotNull();
            }

            loginWindow.Show();
        }

        private IEnumerator<IYieldInstruction> DisconnectAutomatically()
        {
            const int MINIMUM_TASKS = 1;

            while (true)
            {
                yield return new WaitForSeconds(AUTO_TIME_FOR_DISCONNECT);

                if (coroutinesExecutor.Count > MINIMUM_TASKS)
                {
                    continue;
                }

                Disconnect();
                yield break;
            }
        }

        private void Disconnect()
        {
            disconnectAutomatically?.Dispose();
            disconnectAutomatically = null;

            ServiceContainer.RegistrationService.Disconnect();
        }
    }
}
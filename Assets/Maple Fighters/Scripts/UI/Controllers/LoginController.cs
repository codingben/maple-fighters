using System.Collections.Generic;
using System.Threading.Tasks;
using CommonTools.Coroutines;
using CommonTools.Log;
using Login.Common;
using Scripts.Containers;
using Scripts.Services;
using Scripts.UI.Core;
using Scripts.UI.Windows;
using UnityEngine;
using WaitForSeconds = CommonTools.Coroutines.WaitForSeconds;

namespace Scripts.UI.Controllers
{
    public class LoginController : MonoBehaviour
    {
        private const int AUTO_TIME_FOR_DISCONNECT = 60;

        [SerializeField] private int loadSceneIndex;

        private RegistrationWindow registrationWindow;
        private LoginWindow loginWindow;

        private ICoroutine disconnectAutomatically;
        private readonly ExternalCoroutinesExecutor coroutinesExecutor = new ExternalCoroutinesExecutor();

        private void Start()
        {
            loginWindow = UserInterfaceContainer.Instance.Add<LoginWindow>();
            loginWindow.Show();

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
            loginWindow.ShowNotice += (message) => Utils.ShowNotice(message, () => loginWindow.Show());
        }

        private void UnsubscribeFromRegistrationWindowEvents()
        {
            loginWindow.LoginButtonClicked -= OnLoginButtonClicked;
            loginWindow.RegisterButtonClicked -= OnRegisterButtonClicked;
        }

        private void OnLoginButtonClicked(string email, string password)
        {
            var parameters = new LoginRequestParameters(email, password);
            coroutinesExecutor.StartTask((y) => Connect(y, parameters));
        }

        private async Task Connect(IYield yield, LoginRequestParameters parameters)
        {
            var noticeWindow = Utils.ShowNotice("Logging in... Please wait.", () => loginWindow.Show());
            noticeWindow.OkButton.interactable = false;

            if (!ServiceContainer.LoginService.IsConnected())
            {
                var connectionStatus = await ServiceContainer.LoginService.Connect(yield);
                if (connectionStatus == ConnectionStatus.Failed)
                {
                    noticeWindow.Message.text = "Could not connect to a login server.";
                    noticeWindow.OkButton.interactable = true;
                    return;
                }
            }

            coroutinesExecutor.StartTask((y) => Login(y, parameters));
        }

        private async Task Login(IYield yield, LoginRequestParameters parameters)
        {
            var noticeWindow = UserInterfaceContainer.Instance.Get<NoticeWindow>().AssertNotNull();
            var responseParameters = await ServiceContainer.LoginService.Login(yield, parameters);

            switch (responseParameters.Status)
            {
                case LoginStatus.Succeed:
                {
                    noticeWindow.Message.text = "You have logged in successfully. Please wait.";
                    LoginSucceed();
                    break;
                }
                case LoginStatus.UserNotExist:
                {
                    noticeWindow.Message.text = "The user does not exist. Please check your typed email.";
                    noticeWindow.OkButton.interactable = true;
                    break;
                }
                case LoginStatus.PasswordIncorrect:
                {
                    noticeWindow.Message.text = "The password is incorrect, please type it again.";
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

            if (responseParameters.Status != LoginStatus.Succeed)
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

        private void LoginSucceed()
        {
            GameConnector.Instance.Connect();
        }

        private void OnRegisterButtonClicked()
        {
            if (registrationWindow == null)
            {
                registrationWindow = UserInterfaceContainer.Instance.Get<RegistrationWindow>().AssertNotNull();
            }

            registrationWindow.Show();
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

            ServiceContainer.LoginService.Disconnect();
        }
    }
}
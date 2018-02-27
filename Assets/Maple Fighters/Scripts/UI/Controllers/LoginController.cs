using System.Threading.Tasks;
using CommonTools.Coroutines;
using CommonTools.Log;
using Login.Common;
using Scripts.Containers;
using Scripts.ScriptableObjects;
using Scripts.Services;
using Scripts.UI.Core;
using Scripts.UI.Windows;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Scripts.UI.Controllers
{
    public class LoginController : ServiceConnector<LoginController>
    {
        [SerializeField] private int loadSceneIndex;

        private RegistrationWindow registrationWindow;
        private LoginWindow loginWindow;

        private void Start()
        {
            loginWindow = UserInterfaceContainer.Instance.Add<LoginWindow>();
            loginWindow.Show();

            SubscribeToLoginWindowEvents();
        }

        protected override void OnDestroyed()
        {
            base.OnDestroyed();

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
            var parameters = new AuthenticateRequestParameters(email, password.CreateSha512());
            CoroutinesExecutor.StartTask((y) => Connect(y, parameters));
        }

        private async Task Connect(IYield yield, AuthenticateRequestParameters parameters)
        {
            var noticeWindow = Utils.ShowNotice("Logging in... Please wait.", () => loginWindow.Show());
            noticeWindow.OkButton.interactable = false;

            if (!IsConnected())
            {
                var connectionInformation = ServicesConfiguration.GetInstance().GetConnectionInformation(ServersType.Login);
                var connectionStatus = await Connect(yield, ServiceContainer.LoginService, connectionInformation);
                if (connectionStatus == ConnectionStatus.Failed)
                {
                    noticeWindow.Message.text = "Could not connect to a login server.";
                    noticeWindow.OkButton.interactable = true;
                    return;
                }
            }

            CoroutinesExecutor.StartTask((y) => Login(y, parameters));
        }

        private async Task Login(IYield yield, AuthenticateRequestParameters parameters)
        {
            var noticeWindow = UserInterfaceContainer.Instance.Get<NoticeWindow>().AssertNotNull();
            var responseParameters = await ServiceContainer.LoginService.Login(yield, parameters);
            switch (responseParameters.Status)
            {
                case LoginStatus.Succeed:
                {
                    noticeWindow.Message.text = "You have logged in successfully. Please wait.";
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
                DisconnectAutomatically();
            }
            else
            {
                Disconnect();
            }

            if (responseParameters.Status == LoginStatus.Succeed)
            {
                OnLoginSucceed();
            }
        }

        private void OnLoginSucceed()
        {
            CharacterConnector.Instance.Connect(onAuthorized: () => 
            {
                SceneManager.LoadScene(loadSceneIndex, LoadSceneMode.Single);
            });
        }

        private void OnRegisterButtonClicked()
        {
            // TODO: It should access a registration controller (?)

            if (registrationWindow == null)
            {
                registrationWindow = UserInterfaceContainer.Instance.Get<RegistrationWindow>().AssertNotNull();
            }

            registrationWindow.Show();
        }
    }
}
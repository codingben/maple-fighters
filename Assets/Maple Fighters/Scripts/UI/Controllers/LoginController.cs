using System.Threading.Tasks;
using CommonTools.Coroutines;
using CommonTools.Log;
using Login.Common;
using Scripts.Containers;
using Scripts.UI.Core;
using Scripts.UI.Windows;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Scripts.UI.Controllers
{
    public class LoginController : MonoBehaviour
    {
        [SerializeField] private int loadSceneIndex;

        private RegistrationWindow registrationWindow;
        private LoginWindow loginWindow;

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
            if (!ServiceContainer.LoginService.IsConnected())
            {
                Utils.ShowNotice("Could not connect to a server.", () => loginWindow.Show());
                return;
            }

            var parameters = new LoginRequestParameters(email, password);
            coroutinesExecutor.StartTask((y) => Login(y, parameters));
        }

        private async Task Login(IYield yield, LoginRequestParameters parameters)
        {
            var noticeWindow = Utils.ShowNotice("Login is in a process, please wait.", () => loginWindow.Show());
            noticeWindow.OkButton.interactable = false;

            var responseParameters = await ServiceContainer.LoginService.Login(yield, parameters);

            switch (responseParameters.Status)
            {
                case LoginStatus.Succeed:
                {
                    noticeWindow.Message.text = "You have logged in successfully. Loading the world...";
                    LoginSucceed(noticeWindow);
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
        }

        private void LoginSucceed(NoticeWindow noticeWindow)
        {
            var screenFade = UserInterfaceContainer.Instance.Get<ScreenFade>();
            screenFade.Show(() => OnLoginSucceed(noticeWindow));
        }

        private void OnLoginSucceed(NoticeWindow noticeWindow)
        {
            UserInterfaceContainer.Instance.Remove(noticeWindow);

            SceneManager.LoadScene(loadSceneIndex, LoadSceneMode.Single);
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
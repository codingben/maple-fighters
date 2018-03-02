using System;
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
    public class LoginController : MonoSingleton<LoginController>
    {
        [SerializeField] private int loadSceneIndex;
        private readonly ExternalCoroutinesExecutor coroutinesExecutor = new ExternalCoroutinesExecutor();

        private void Start()
        {
            CreateLoginWindow();
        }

        private void Update()
        {
            coroutinesExecutor.Update();
        }

        private void OnDestroyed()
        {
            coroutinesExecutor.Dispose();

            RemoveLoginWindow();
        }

        private void CreateLoginWindow()
        {
            var loginWindow = UserInterfaceContainer.Instance.Add<LoginWindow>();
            loginWindow.LoginButtonClicked += OnLoginButtonClicked;
            loginWindow.RegisterButtonClicked += OnRegisterButtonClicked;
            loginWindow.ShowNotice += (message) => Utils.ShowNotice(message, okButtonClicked: () => loginWindow.Show());
            loginWindow.Show();
        }

        private void RemoveLoginWindow()
        {
            var loginWindow = UserInterfaceContainer.Instance.Get<LoginWindow>().AssertNotNull();
            loginWindow.LoginButtonClicked -= OnLoginButtonClicked;
            loginWindow.RegisterButtonClicked -= OnRegisterButtonClicked;

            UserInterfaceContainer.Instance.Remove(loginWindow);
        }

        private void OnLoginButtonClicked(string email, string password)
        {
            var loginWindow = UserInterfaceContainer.Instance.Get<LoginWindow>().AssertNotNull();
            loginWindow.Hide(onFinished: () => Login(email, password));
        }

        private void Login(string email, string password)
        {
            var noticeWindow = Utils.ShowNotice("Logging in... Please wait.", okButtonClicked: () =>
            {
                var loginWindow = UserInterfaceContainer.Instance.Get<LoginWindow>().AssertNotNull();
                loginWindow.Show();
            });
            noticeWindow.OkButton.interactable = false;

            Action loginAction = () =>
            {
                var parameters = new AuthenticateRequestParameters(email, password.CreateSha512());
                coroutinesExecutor.StartTask((yield) => Login(yield, parameters));
            };

            if (LoginConnectionProvider.Instance.IsConnected())
            {
                loginAction.Invoke();
            }
            else
            {
                LoginConnectionProvider.Instance.Connect(onConnected: loginAction);
            }
        }

        private async Task Login(IYield yield, AuthenticateRequestParameters parameters)
        {
            var responseParameters = await ServiceContainer.LoginService.Login(yield, parameters);
            switch (responseParameters.Status)
            {
                case LoginStatus.Succeed:
                {
                    var noticeWindow = UserInterfaceContainer.Instance.Get<NoticeWindow>().AssertNotNull();
                    noticeWindow.Message.text = "You have logged in successfully. Please wait.";
                    break;
                }
                case LoginStatus.UserNotExist:
                {
                    var noticeWindow = UserInterfaceContainer.Instance.Get<NoticeWindow>().AssertNotNull();
                    noticeWindow.Message.text = "The user does not exist. Please check your typed email.";
                    noticeWindow.OkButton.interactable = true;
                    break;
                }
                case LoginStatus.PasswordIncorrect:
                {
                    var noticeWindow = UserInterfaceContainer.Instance.Get<NoticeWindow>().AssertNotNull();
                    noticeWindow.Message.text = "The password is incorrect, please type it again.";
                    noticeWindow.OkButton.interactable = true;
                    break;
                }
                case LoginStatus.NonAuthorized:
                {
                    var noticeWindow = UserInterfaceContainer.Instance.Get<NoticeWindow>().AssertNotNull();
                    noticeWindow.Message.text = "Authentication with login server failed.";
                    noticeWindow.OkButton.interactable = true;
                    break;
                }
                default:
                {
                    var noticeWindow = UserInterfaceContainer.Instance.Get<NoticeWindow>().AssertNotNull();
                    noticeWindow.Message.text = "Something went wrong, please try again.";
                    noticeWindow.OkButton.interactable = true;
                    break;
                }
            }

            if (responseParameters.Status == LoginStatus.Succeed)
            {
                OnLoginSucceed();
            }
        }

        private void OnLoginSucceed()
        {
            LoginConnectionProvider.Instance.Dispose();
            SceneManager.LoadScene(loadSceneIndex, LoadSceneMode.Single);
        }

        private void OnRegisterButtonClicked()
        {
            var loginWindow = UserInterfaceContainer.Instance.Get<LoginWindow>().AssertNotNull();
            loginWindow.Hide(onFinished: () => 
            {
                loginWindow.ResetInputFields();

                var registrationWindow = UserInterfaceContainer.Instance.Get<RegistrationWindow>().AssertNotNull();
                registrationWindow.Show();
            });
        }
    }
}
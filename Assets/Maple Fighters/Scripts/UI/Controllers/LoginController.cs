using System;
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
    public class LoginController : MonoSingleton<LoginController>
    {
        [SerializeField] private int loadSceneIndex;

        private LoginWindow loginWindow;
        private readonly ExternalCoroutinesExecutor coroutinesExecutor = new ExternalCoroutinesExecutor();

        private void Start()
        {
            CreateLoginWindow();
        }

        private void Update()
        {
            coroutinesExecutor.Update();
        }

        protected override void OnDestroyed()
        {
            base.OnDestroyed();

            coroutinesExecutor.Dispose();

            RemoveLoginWindow();
        }

        private void CreateLoginWindow()
        {
            loginWindow = UserInterfaceContainer.Instance.Add<LoginWindow>();
            loginWindow.LoginButtonClicked += OnLoginButtonClicked;
            loginWindow.RegisterButtonClicked += OnRegisterButtonClicked;
            loginWindow.ShowNotice += (message) => Utils.ShowNotice(message, okButtonClicked: () => loginWindow.Show());
            loginWindow.Show();
        }

        private void RemoveLoginWindow()
        {
            loginWindow.LoginButtonClicked -= OnLoginButtonClicked;
            loginWindow.RegisterButtonClicked -= OnRegisterButtonClicked;

            UserInterfaceContainer.Instance?.Remove(loginWindow);
        }

        private void OnLoginButtonClicked(string email, string password)
        {
            loginWindow.Hide(onFinished: () => Login(email, password));
        }

        private void Login(string email, string password)
        {
            var noticeWindow = Utils.ShowNotice("Logging in... Please wait.", okButtonClicked: () =>
            {
                loginWindow.Show();
            });
            noticeWindow.OkButton.interactable = false;

            Action authenticateAction = () =>
            {
                var parameters = new AuthenticateRequestParameters(email, password.CreateSha512());
                coroutinesExecutor.StartTask((yield) => Authenticate(yield, parameters));
            };

            if (LoginConnectionProvider.Instance.IsConnected())
            {
                authenticateAction.Invoke();
            }
            else
            {
                LoginConnectionProvider.Instance.Connect(onConnected: authenticateAction);
            }
        }

        private async Task Authenticate(IYield yield, AuthenticateRequestParameters parameters)
        {
            try
            {
                var loginService = ServiceContainer.LoginService.GetPeerLogic<ILoginPeerLogicAPI>().AssertNotNull();
                var responseParameters = await loginService.Authenticate(yield, parameters);
                switch (responseParameters.Status)
                {
                    case LoginStatus.Succeed:
                    {
                        var noticeWindow = UserInterfaceContainer.Instance.Get<NoticeWindow>().AssertNotNull();
                        noticeWindow.Message.text = "You have logged in successfully!";
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
            catch (Exception)
            {
                Utils.ShowExceptionNotice();
            }
        }

        private void OnLoginSucceed()
        {
            coroutinesExecutor.StartCoroutine(ConnectToMasterServerAfterDelay());
        }

        private IEnumerator<IYieldInstruction> ConnectToMasterServerAfterDelay()
        {
            yield return new WaitForSeconds(0.25f);

            GameServerSelectorConnectionProvider.Instance.Connect();
        }

        private void OnRegisterButtonClicked()
        {
            loginWindow.Hide(onFinished: () => 
            {
                loginWindow.ResetInputFields();

                var registrationWindow = UserInterfaceContainer.Instance.Get<RegistrationWindow>().AssertNotNull();
                registrationWindow.Show();
            });
        }
    }
}
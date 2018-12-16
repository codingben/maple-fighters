using System;
using System.Threading.Tasks;
using CommonTools.Coroutines;
using CommonTools.Log;
using Registration.Common;
using Scripts.Containers;
using Scripts.Services;
using Scripts.UI.Core;
using Scripts.UI.Windows;
using Scripts.Utils;

namespace Scripts.UI.Controllers
{
    public class RegistrationController : MonoSingleton<RegistrationController>
    {
        private ExternalCoroutinesExecutor coroutinesExecutor;
        private RegistrationWindow registrationWindow;

        protected override void OnAwake()
        {
            base.OnAwake();

            coroutinesExecutor = new ExternalCoroutinesExecutor();
        }

        private void Start()
        {
            CreateRegistrationWindow();
        }

        private void Update()
        {
            coroutinesExecutor?.Update();
        }

        protected override void OnDestroying()
        {
            base.OnDestroying();

            coroutinesExecutor?.Dispose();

            RemoveRegistrationWindow();
        }

        private void CreateRegistrationWindow()
        {
            registrationWindow = UserInterfaceContainer.GetInstance().Add<RegistrationWindow>().AssertNotNull();
            registrationWindow.RegisterButtonClicked += OnRegisterButtonClicked;
            registrationWindow.BackButtonClicked += OnBackButtonClicked;
            registrationWindow.ShowNotice += (message) => Utils.ShowNotice(message, okButtonClicked: () => registrationWindow.Show());
        }

        private void RemoveRegistrationWindow()
        {
            registrationWindow.RegisterButtonClicked -= OnRegisterButtonClicked;
            registrationWindow.BackButtonClicked -= OnBackButtonClicked;

            UserInterfaceContainer.GetInstance()?.Remove(registrationWindow);
        }

        private void OnRegisterButtonClicked(string email, string password, string firstName, string lastName)
        {
            var loginWindow = UserInterfaceContainer.GetInstance().Get<LoginWindow>().AssertNotNull();
            loginWindow.Hide(onFinished: () => Register(email, password, firstName, lastName));
        }

        private void Register(string email, string password, string firstName, string lastName)
        {
            var noticeWindow = Utils.ShowNotice(
                message: "Registration is in a process.. Please wait.", 
                okButtonClicked: () =>
                {
                    registrationWindow.Show();
                });
            noticeWindow.OkButton.interactable = false;

            Action registerAction = () => 
            {
                var parameters = new RegisterRequestParameters(email, password.CreateSha512(), firstName, lastName);
                coroutinesExecutor.StartTask((yield) => Register(yield, parameters), exception => ServiceConnectionProviderUtils.OnOperationFailed());
            };

            if (RegistrationConnectionProvider.GetInstance().IsConnected())
            {
                registerAction.Invoke();
            }
            else
            {
                RegistrationConnectionProvider.GetInstance().Connect(onConnected: registerAction);
            }
        }

        private async Task Register(IYield yield, RegisterRequestParameters parameters)
        {
            var registrationPeerLogic = ServiceContainer.RegistrationService.GetPeerLogic<IRegistrationPeerLogicAPI>().AssertNotNull();
            var responseParameters = await registrationPeerLogic.Register(yield, parameters);
            switch (responseParameters.Status)
            {
                case RegisterStatus.Succeed:
                {
                    var noticeWindow = UserInterfaceContainer.GetInstance().Get<NoticeWindow>().AssertNotNull();
                    noticeWindow.Message.text = "Registration is completed successfully.";
                    noticeWindow.OkButtonClickedAction = OnBackButtonClicked;
                    noticeWindow.OkButton.interactable = true;
                    break;
                }
                case RegisterStatus.EmailExists:
                {
                    var noticeWindow = UserInterfaceContainer.GetInstance().Get<NoticeWindow>().AssertNotNull();
                    noticeWindow.Message.text = "Email address already exists.";
                    noticeWindow.OkButton.interactable = true;
                    break;
                }
                default:
                {
                    var noticeWindow = UserInterfaceContainer.GetInstance().Get<NoticeWindow>().AssertNotNull();
                    noticeWindow.Message.text = "Something went wrong, please try again.";
                    noticeWindow.OkButton.interactable = true;
                    break;
                }
            }

            if (responseParameters.Status == RegisterStatus.Succeed)
            {
                OnRegistrationSucceed();
            }
        }

        private void OnRegistrationSucceed()
        {
            RegistrationConnectionProvider.GetInstance().Dispose();
        }

        private void OnBackButtonClicked()
        {
            registrationWindow.Hide(onFinished: () =>
            {
                registrationWindow.ResetInputFields();

                var loginWindow = UserInterfaceContainer.GetInstance().Get<LoginWindow>().AssertNotNull();
                loginWindow.Show();
            });
        }
    }
}
using System;
using System.Threading.Tasks;
using CommonTools.Coroutines;
using CommonTools.Log;
using Registration.Common;
using Scripts.Containers;
using Scripts.Services;
using Scripts.UI.Core;
using Scripts.UI.Windows;

namespace Scripts.UI.Controllers
{
    public class RegistrationController : MonoSingleton<RegistrationController>
    {
        private readonly ExternalCoroutinesExecutor coroutinesExecutor = new ExternalCoroutinesExecutor();

        private void Start()
        {
            CreateRegistrationWindow();
        }

        private void Update()
        {
            coroutinesExecutor.Update();
        }

        private void OnDestroy()
        {
            coroutinesExecutor.Dispose();

            RemoveRegistrationWindow();
        }

        private void CreateRegistrationWindow()
        {
            var registrationWindow = UserInterfaceContainer.Instance.Add<RegistrationWindow>().AssertNotNull();
            registrationWindow.RegisterButtonClicked += OnRegisterButtonClicked;
            registrationWindow.BackButtonClicked += OnBackButtonClicked;
            registrationWindow.ShowNotice += (message) => Utils.ShowNotice(message, okButtonClicked: () => registrationWindow.Show());
        }

        private void RemoveRegistrationWindow()
        {
            var registrationWindow = UserInterfaceContainer.Instance.Get<RegistrationWindow>().AssertNotNull();
            registrationWindow.RegisterButtonClicked -= OnRegisterButtonClicked;
            registrationWindow.BackButtonClicked -= OnBackButtonClicked;

            UserInterfaceContainer.Instance.Remove(registrationWindow);
        }

        private void OnRegisterButtonClicked(string email, string password, string firstName, string lastName)
        {
            var registrationWindow = UserInterfaceContainer.Instance.Get<LoginWindow>().AssertNotNull();
            registrationWindow.Hide(onFinished: () => Register(email, password, firstName, lastName));
        }

        private void Register(string email, string password, string firstName, string lastName)
        {
            var noticeWindow = Utils.ShowNotice("Registration is in a process.. Please wait.", okButtonClicked: () =>
            {
                var registrationWindow = UserInterfaceContainer.Instance.Get<RegistrationWindow>().AssertNotNull();
                registrationWindow.Show();
            });
            noticeWindow.OkButton.interactable = false;

            Action registerAction = () => 
            {
                var parameters = new RegisterRequestParameters(email, password.CreateSha512(), firstName, lastName);
                coroutinesExecutor.StartTask((yield) => Register(yield, parameters));
            };

            if (RegistrationConnectionProvider.Instance.IsConnected())
            {
                registerAction.Invoke();
            }
            else
            {
                RegistrationConnectionProvider.Instance.Connect(onConnected: registerAction);
            }
        }

        private async Task Register(IYield yield, RegisterRequestParameters parameters)
        {
            var registrationService = ServiceContainer.RegistrationService.GetPeerLogic<IRegistrationPeerLogicAPI>().AssertNotNull();
            var responseParameters = await registrationService.Register(yield, parameters);
            switch (responseParameters.Status)
            {
                case RegisterStatus.Succeed:
                {
                    var noticeWindow = UserInterfaceContainer.Instance.Get<NoticeWindow>().AssertNotNull();
                    noticeWindow.Message.text = "Registration is completed successfully.";
                    noticeWindow.OkButtonClickedAction = OnBackButtonClicked;
                    noticeWindow.OkButton.interactable = true;
                    break;
                }
                case RegisterStatus.EmailExists:
                {
                    var noticeWindow = UserInterfaceContainer.Instance.Get<NoticeWindow>().AssertNotNull();
                    noticeWindow.Message.text = "Email address already exists.";
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

            if (responseParameters.Status == RegisterStatus.Succeed)
            {
                OnRegistrationSucceed();
            }
        }

        private void OnRegistrationSucceed()
        {
            RegistrationConnectionProvider.Instance.Dispose();
        }

        private void OnBackButtonClicked()
        {
            var registrationWindow = UserInterfaceContainer.Instance.Get<RegistrationWindow>().AssertNotNull();
            registrationWindow.Hide(onFinished: () =>
            {
                registrationWindow.ResetInputFields();

                var loginWindow = UserInterfaceContainer.Instance.Get<LoginWindow>().AssertNotNull();
                loginWindow.Show();
            });
        }
    }
}
using System.Threading.Tasks;
using CommonTools.Coroutines;
using Login.Common;
using Registration.Common;
using Scripts.Containers;
using Scripts.Network.APIs;
using Scripts.UI.Windows;
using UnityEngine;

namespace Scripts.UI.Controllers
{
    [RequireComponent(typeof(IOnLoginFinishedListener))]
    [RequireComponent(typeof(IOnRegistrationFinishedListener))]
    public class AuthenticatorInteractor : MonoBehaviour
    {
        private IAuthenticatorApi authenticatorApi;
        private IOnLoginFinishedListener onLoginFinishedListener;
        private IOnRegistrationFinishedListener onRegistrationFinishedListener;

        private ExternalCoroutinesExecutor coroutinesExecutor;

        private void Awake()
        {
            authenticatorApi = ServiceContainer.AuthenticatorService
                .GetAuthenticatorApi();

            onLoginFinishedListener = GetComponent<IOnLoginFinishedListener>();
            onRegistrationFinishedListener =
                GetComponent<IOnRegistrationFinishedListener>();

            coroutinesExecutor = new ExternalCoroutinesExecutor();
        }

        private void Update()
        {
            coroutinesExecutor?.Update();
        }

        private void OnDestroy()
        {
            coroutinesExecutor?.Dispose();
        }

        public void Login(UIAuthenticationDetails uiAuthenticationDetails)
        {
            var parameters = new AuthenticateRequestParameters(
                uiAuthenticationDetails.Email,
                uiAuthenticationDetails.Password);

            // TODO: Handle exception situation
            coroutinesExecutor?.StartTask(
                (yield) => LoginAsync(yield, parameters));
        }

        private async Task LoginAsync(
            IYield yield,
            AuthenticateRequestParameters parameters)
        {
            var responseParameters =
                await authenticatorApi.AuthenticateAsync(yield, parameters);

            switch (responseParameters.Status)
            {
                case LoginStatus.Succeed:
                {
                    onLoginFinishedListener.OnLoginSucceed();
                    break;
                }

                case LoginStatus.UserNotExist:
                {
                    onLoginFinishedListener.OnInvalidEmailError();
                    break;
                }

                case LoginStatus.PasswordIncorrect:
                {
                    onLoginFinishedListener.OnInvalidPasswordError();
                    break;
                }

                case LoginStatus.NonAuthorized:
                {
                    break;
                }
            }
        }

        public void Register(UIRegistrationDetails uiRegistrationDetails)
        {
            var parameters = new RegisterRequestParameters(
                uiRegistrationDetails.Email,
                uiRegistrationDetails.Password,
                uiRegistrationDetails.FirstName,
                uiRegistrationDetails.LastName);

            coroutinesExecutor?.StartTask(
                (yield) => RegisterAsync(yield, parameters));
        }

        private async Task RegisterAsync(
            IYield yield,
            RegisterRequestParameters parameters)
        {
            var responseParameters =
                await authenticatorApi.RegisterAsync(yield, parameters);

            switch (responseParameters.Status)
            {
                case RegisterStatus.Succeed:
                {
                    onRegistrationFinishedListener.OnRegistrationSucceed();
                    break;
                }

                case RegisterStatus.EmailExists:
                {
                    onRegistrationFinishedListener.OnEmailExistsError();
                    break;
                }
            }
        }
    }
}
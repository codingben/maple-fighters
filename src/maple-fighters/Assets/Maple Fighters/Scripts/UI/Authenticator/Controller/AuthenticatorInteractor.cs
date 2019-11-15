using System.Threading.Tasks;
using CommonTools.Coroutines;
using Login.Common;
using Network.Scripts;
using Registration.Common;
using Scripts.Services.Authenticator;
using UnityEngine;

namespace Scripts.UI.Authenticator
{
    [RequireComponent(typeof(IOnConnectionFinishedListener))]
    [RequireComponent(typeof(IOnLoginFinishedListener))]
    [RequireComponent(typeof(IOnRegistrationFinishedListener))]
    public class AuthenticatorInteractor : MonoBehaviour
    {
        private AuthenticatorService authenticatorService;

        private IOnConnectionFinishedListener onConnectionFinishedListener;
        private IOnLoginFinishedListener onLoginFinishedListener;
        private IOnRegistrationFinishedListener onRegistrationFinishedListener;

        private ExternalCoroutinesExecutor coroutinesExecutor;

        private void Awake()
        {
            authenticatorService = FindObjectOfType<AuthenticatorService>();

            onConnectionFinishedListener =
                GetComponent<IOnConnectionFinishedListener>();
            onLoginFinishedListener = 
                GetComponent<IOnLoginFinishedListener>();
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

            coroutinesExecutor?.StartTask(
                method: (y) => LoginAsync(y, parameters),
                onException: (e) => onLoginFinishedListener.OnLoginFailed());
        }

        private async Task LoginAsync(
            IYield yield,
            AuthenticateRequestParameters parameters)
        {
            await ConnectIfNotConnectedAsync(yield);

            var authenticatorApi = authenticatorService?.AuthenticatorApi;
            if (authenticatorApi != null)
            {
                var responseParameters =
                    await authenticatorApi.AuthenticateAsync(yield, parameters);
                var status = responseParameters.Status;

                switch (status)
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
                        onLoginFinishedListener.OnNonAuthorizedError();
                        break;
                    }
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
                method: (y) => RegisterAsync(y, parameters),
                onException: (e) =>
                    onRegistrationFinishedListener.OnRegistrationFailed());
        }

        private async Task RegisterAsync(
            IYield yield,
            RegisterRequestParameters parameters)
        {
            await ConnectIfNotConnectedAsync(yield);

            var authenticatorApi = authenticatorService?.AuthenticatorApi;
            if (authenticatorApi != null)
            {
                var responseParameters =
                    await authenticatorApi.RegisterAsync(yield, parameters);
                var status = responseParameters.Status;

                switch (status)
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

        private async Task ConnectIfNotConnectedAsync(IYield yield)
        {
            if (authenticatorService != null && !authenticatorService.IsConnected)
            {
                var connectionStatus =
                    await authenticatorService.ConnectAsync(yield);
                if (connectionStatus == ConnectionStatus.Failed)
                {
                    onConnectionFinishedListener.OnConnectionFailed();
                }
            }
        }
    }
}
using Scripts.Constants;
using Scripts.Services;
using Scripts.Services.AuthenticatorApi;
using UnityEngine;

namespace Scripts.UI.Authenticator
{
    [RequireComponent(typeof(IOnLoginFinishedListener))]
    [RequireComponent(typeof(IOnRegistrationFinishedListener))]
    public class AuthenticatorInteractor : MonoBehaviour
    {
        private IAuthenticatorApi authenticatorApi;
        private IOnLoginFinishedListener onLoginFinishedListener;
        private IOnRegistrationFinishedListener onRegistrationFinishedListener;

        private void Awake()
        {
            authenticatorApi =
                ApiProvider.ProvideAuthenticatorApi();
            onLoginFinishedListener =
                GetComponent<IOnLoginFinishedListener>();
            onRegistrationFinishedListener =
                GetComponent<IOnRegistrationFinishedListener>();

            if (authenticatorApi != null)
            {
                authenticatorApi.LoginCallback += OnLoginCallback;
                authenticatorApi.RegisterCallback += OnRegisterCallback;
            }
        }

        private void OnDestroy()
        {
            if (authenticatorApi != null)
            {
                authenticatorApi.LoginCallback -= OnLoginCallback;
                authenticatorApi.RegisterCallback -= OnRegisterCallback;
            }
        }

        public void Login(UIAuthenticationDetails uiAuthenticationDetails)
        {
            var email = uiAuthenticationDetails.Email;
            var password = uiAuthenticationDetails.Password;

            authenticatorApi?.Login(email, password);
        }

        public void Register(UIRegistrationDetails uiRegistrationDetails)
        {
            var email = uiRegistrationDetails.Email;
            var password = uiRegistrationDetails.Password;
            var firstName = uiRegistrationDetails.FirstName;
            var lastName = uiRegistrationDetails.LastName;

            authenticatorApi?.Register(email, password, firstName, lastName);
        }

        private void OnLoginCallback(long statusCode, string json)
        {
            switch (statusCode)
            {
                case 200: // Ok    
                {
                    var userMetadata = FindObjectOfType<UserMetadata>();
                    if (userMetadata != null)
                    {
                        userMetadata.UserData = UserData.FromJson(json);
                    }
                    
                    onLoginFinishedListener.OnLoginSucceed();
                    break;
                }

                case 404: // Not Found
                {
                    var errorMessage = ErrorData.FromJsonToErrorMessage(json);
                    if (errorMessage == string.Empty)
                    {
                        errorMessage = NoticeMessages.AuthView.UnknownError;
                    }

                    onLoginFinishedListener.OnLoginFailed(errorMessage);
                    break;
                }

                default:
                {
                    var errorMessage = NoticeMessages.AuthView.UnknownError;

                    onLoginFinishedListener.OnLoginFailed(errorMessage);
                    break;
                }
            }
        }

        private void OnRegisterCallback(long statusCode, string json)
        {
            switch (statusCode)
            {
                case 200: // Ok
                {
                    onRegistrationFinishedListener.OnRegistrationSucceed();
                    break;
                }

                case 400: // Bad Request
                {
                    var errorMessage = ErrorData.FromJsonToErrorMessage(json);
                    if (errorMessage == string.Empty)
                    {
                        errorMessage = NoticeMessages.AuthView.UnknownError;
                    }

                    onRegistrationFinishedListener.OnRegistrationFailed(errorMessage);
                    break;
                }

                default:
                {
                    var errorMessage = NoticeMessages.AuthView.UnknownError;

                    onRegistrationFinishedListener.OnRegistrationFailed(errorMessage);
                    break;
                }
            }
        }
    }
}
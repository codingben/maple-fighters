using Scripts.Constants;
using Scripts.UI.Notice;
using UI.Manager;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Scripts.UI.Authenticator
{
    [RequireComponent(typeof(AuthenticatorInteractor))]
    public class AuthenticatorController : MonoBehaviour,
                                           IOnConnectionFinishedListener,
                                           IOnLoginFinishedListener,
                                           IOnRegistrationFinishedListener
    {
        private ILoginView loginView;
        private IRegistrationView registrationView;

        private AuthenticationValidator authenticationValidator;
        private AuthenticatorInteractor authenticatorInteractor;

        private void Awake()
        {
            authenticationValidator = new AuthenticationValidator();
            authenticatorInteractor = GetComponent<AuthenticatorInteractor>();

            CreateAndSubscribeToLoginWindow();
            CreateAndSubscribeToRegistrationWindow();
        }

        private void Start()
        {
            ShowLoginWindow();
        }

        private void CreateAndSubscribeToLoginWindow()
        {
            loginView = UIElementsCreator.GetInstance()
                .Create<LoginWindow>();
            loginView.LoginButtonClicked +=
                OnLoginButtonClicked;
            loginView.CreateAccountButtonClicked +=
                OnCreateAccountButtonClicked;
        }

        private void CreateAndSubscribeToRegistrationWindow()
        {
            registrationView = UIElementsCreator.GetInstance()
                .Create<RegistrationWindow>();
            registrationView.RegisterButtonClicked +=
                OnRegisterButtonClicked;
            registrationView.BackButtonClicked += 
                OnBackButtonClicked;
        }

        private void OnDestroy()
        {
            UnsubscribeFromLoginWindow();
            UnsubscribeFromRegistrationWindow();
        }

        private void UnsubscribeFromLoginWindow()
        {
            if (loginView != null)
            {
                loginView.LoginButtonClicked -= 
                    OnLoginButtonClicked;
                loginView.CreateAccountButtonClicked -=
                    OnCreateAccountButtonClicked;
            }
        }

        private void UnsubscribeFromRegistrationWindow()
        {
            if (registrationView != null)
            {
                registrationView.RegisterButtonClicked -=
                    OnRegisterButtonClicked;
                registrationView.BackButtonClicked -= 
                    OnBackButtonClicked;
            }
        }

        private void OnLoginButtonClicked(
            UIAuthenticationDetails uiAuthenticationDetails)
        {
            var email = uiAuthenticationDetails.Email;
            var password = uiAuthenticationDetails.Password;

            if (authenticationValidator.IsEmptyEmailAddress(email, out var message)
                || authenticationValidator.IsInvalidEmailAddress(email, out message)
                || authenticationValidator.IsEmptyPassword(password, out message)
                || authenticationValidator.IsPasswordTooShort(password, out message))
            {
                NoticeUtils.ShowNotice(message);
            }
            else
            {
                loginView?.DisableInteraction();
                authenticatorInteractor.Login(uiAuthenticationDetails);
            }
        }

        private void OnCreateAccountButtonClicked()
        {
            HideLoginWindow();
            ShowRegistrationWindow();
        }

        private void OnRegisterButtonClicked(
            UIRegistrationDetails uiRegistrationDetails)
        {
            var email = uiRegistrationDetails.Email;
            var password = uiRegistrationDetails.Password;
            var confirmPassword = uiRegistrationDetails.ConfirmPassword;
            var firstName = uiRegistrationDetails.FirstName;
            var lastName = uiRegistrationDetails.LastName;

            if (authenticationValidator.IsEmptyEmailAddress(email, out var message)
                || authenticationValidator.IsInvalidEmailAddress(email, out message)
                || authenticationValidator.IsEmptyPassword(password, out message)
                || authenticationValidator.IsEmptyConfirmPassword(confirmPassword, out message)
                || authenticationValidator.IsPasswordTooShort(password, out message)
                || authenticationValidator.IsConfirmPasswordTooShort(confirmPassword, out message)
                || authenticationValidator.ArePasswordsDoNotMatch(password, confirmPassword, out message)
                || authenticationValidator.IsFirstNameEmpty(firstName, out message)
                || authenticationValidator.IsLastNameEmpty(lastName, out message)
                || authenticationValidator.IsFirstNameTooShort(firstName, out message)
                || authenticationValidator.IsLastNameTooShort(lastName, out message))
            {
                NoticeUtils.ShowNotice(message);
            }
            else
            {
                registrationView?.DisableInteraction();
                authenticatorInteractor.Register(uiRegistrationDetails);
            }
        }

        private void OnBackButtonClicked()
        {
            HideRegistrationWindow();
            ShowLoginWindow();
        }

        private void ShowLoginWindow()
        {
            loginView?.Show();
        }

        private void HideLoginWindow()
        {
            if (loginView != null)
            {
                loginView.Email = string.Empty;
                loginView.Password = string.Empty;
                loginView.Hide();
            }
        }

        private void ShowRegistrationWindow()
        {
            registrationView?.Show();
        }

        private void HideRegistrationWindow()
        {
            if (registrationView != null)
            {
                registrationView.Email = string.Empty;
                registrationView.Password = string.Empty;
                registrationView.ConfirmPassword = string.Empty;
                registrationView.FirstName = string.Empty;
                registrationView.LastName = string.Empty;
                registrationView.Hide();
            }
        }

        public void OnConnectionFailed()
        {
            loginView?.EnableInteraction();
            registrationView?.EnableInteraction();

            NoticeUtils.ShowNotice(message: NoticeMessages.AuthView.ConnectionFailed);
        }

        public void OnLoginSucceed()
        {
            SceneManager.LoadScene(sceneName: SceneNames.Main);
        }

        public void OnLoginFailed()
        {
            loginView?.EnableInteraction();

            NoticeUtils.ShowNotice(message: NoticeMessages.AuthView.UnknownError);
        }

        public void OnInvalidEmailError()
        {
            loginView?.EnableInteraction();

            NoticeUtils.ShowNotice(message: NoticeMessages.AuthView.WrongEmailAddress);
        }

        public void OnInvalidPasswordError()
        {
            loginView?.EnableInteraction();

            NoticeUtils.ShowNotice(message: NoticeMessages.AuthView.WrongPassword);
        }

        public void OnNonAuthorizedError()
        {
            loginView?.EnableInteraction();

            NoticeUtils.ShowNotice(message: NoticeMessages.AuthView.NonAuthorized);
        }

        public void OnRegistrationSucceed()
        {
            registrationView?.EnableInteraction();

            HideRegistrationWindow();
            ShowLoginWindow();

            NoticeUtils.ShowNotice(message: NoticeMessages.AuthView.RegistrationSucceed);
        }

        public void OnRegistrationFailed()
        {
            registrationView?.EnableInteraction();

            NoticeUtils.ShowNotice(message: NoticeMessages.AuthView.UnknownError);
        }

        public void OnEmailExistsError()
        {
            registrationView?.EnableInteraction();

            NoticeUtils.ShowNotice(message: NoticeMessages.AuthView.EmailAddressExists);
        }
    }
}
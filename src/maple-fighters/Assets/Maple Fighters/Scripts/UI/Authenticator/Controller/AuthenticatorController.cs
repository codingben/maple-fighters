using Scripts.Constants;
using Scripts.UI.CharacterSelection;
using Scripts.UI.GameServerBrowser;
using Scripts.UI.MenuBackground;
using Scripts.UI.Notice;
using UI;
using UnityEngine;

namespace Scripts.UI.Authenticator
{
    [RequireComponent(typeof(AuthenticatorInteractor))]
    public class AuthenticatorController : MonoBehaviour,
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
        }

        private void Start()
        {
            CreateAndSubscribeToLoginWindow();
            CreateAndSubscribeToRegistrationWindow();

            SubscribeToBackgroundClicked();

#if UNITY_EDITOR
            CreateLoginButton();
#endif
        }

        private void CreateLoginButton()
        {
#if UNITY_EDITOR
            var loginButton = UICreator.GetInstance().Create<LoginButton>();
            if (loginButton != null)
            {
                loginButton.ButtonClicked += ShowLoginWindow;
            }
#endif
        }

        private void CreateAndSubscribeToLoginWindow()
        {
            loginView = UICreator
                .GetInstance()
                .Create<LoginWindow>();

            if (loginView != null)
            {
                loginView.LoginButtonClicked +=
                    OnLoginButtonClicked;
                loginView.CreateAccountButtonClicked +=
                    OnCreateAccountButtonClicked;
            }
        }

        private void CreateAndSubscribeToRegistrationWindow()
        {
            registrationView = UICreator
                .GetInstance()
                .Create<RegistrationWindow>();

            if (registrationView != null)
            {
                registrationView.RegisterButtonClicked +=
                    OnRegisterButtonClicked;
                registrationView.BackButtonClicked +=
                    OnBackButtonClicked;
            }
        }

        private void OnDestroy()
        {
            UnsubscribeFromLoginWindow();
            UnsubscribeFromRegistrationWindow();
            UnsubscribeFromBackgroundClicked();
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

        private void SubscribeToBackgroundClicked()
        {
            var backgroundController =
                FindObjectOfType<MenuBackgroundController>();
            if (backgroundController != null)
            {
                backgroundController.BackgroundClicked +=
                    OnBackgroundClicked;
            }
        }

        private void UnsubscribeFromBackgroundClicked()
        {
            var backgroundController =
                FindObjectOfType<MenuBackgroundController>();
            if (backgroundController != null)
            {
                backgroundController.BackgroundClicked -=
                    OnBackgroundClicked;
            }
        }

        private void OnBackgroundClicked()
        {
            HideLoginWindow();
            HideRegistrationWindow();
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
            HideLoginWindow();
            HideRegistrationWindow();
            HideCharacterView();
            HideGameServerBrowserView();

            loginView?.Show();
        }

        private void HideCharacterView()
        {
            var characterView = FindObjectOfType<CharacterViewController>();
            characterView?.HideCharacterSelectionOptionsWindow();
            characterView?.HideCharacterSelectionWindow();
            characterView?.HideCharacterNameWindow();
        }

        private void HideGameServerBrowserView()
        {
            var gameServerBrowser = FindObjectOfType<GameServerBrowserController>();
            gameServerBrowser?.HideGameServerBrowserWindow();
        }

        public void HideLoginWindow()
        {
            if (loginView != null &&
                loginView.IsShown)
            {
                loginView.Hide();
            }
        }

        private void ShowRegistrationWindow()
        {
            registrationView?.Show();
        }

        public void HideRegistrationWindow()
        {
            if (registrationView != null &&
                registrationView.IsShown)
            {
                registrationView.Hide();
            }
        }

        public void OnLoginSucceeded()
        {
            loginView?.Hide();

            ClearLoginWindow();

            var characterViewInteractor = FindObjectOfType<CharacterViewInteractor>();
            characterViewInteractor?.SetCharacterProviderApi();

            var characterViewController = FindObjectOfType<CharacterViewController>();
            characterViewController?.LoadCharacters();
        }

        public void OnLoginFailed(string reason)
        {
            loginView?.EnableInteraction();

            NoticeUtils.ShowNotice(message: reason);
        }

        public void OnRegistrationSucceeded()
        {
            registrationView?.EnableInteraction();

            ClearRegistrationWindow();
            HideRegistrationWindow();
            ShowLoginWindow();

            NoticeUtils.ShowNotice(message: NoticeMessages.AuthView.RegistrationSucceed);
        }

        public void OnRegistrationFailed(string reason)
        {
            registrationView?.EnableInteraction();

            NoticeUtils.ShowNotice(message: reason);
        }

        private void ClearLoginWindow()
        {
            if (loginView != null)
            {
                loginView.Email = string.Empty;
                loginView.Password = string.Empty;
            }
        }

        private void ClearRegistrationWindow()
        {
            if (registrationView != null)
            {
                registrationView.Email = string.Empty;
                registrationView.Password = string.Empty;
                registrationView.ConfirmPassword = string.Empty;
                registrationView.FirstName = string.Empty;
                registrationView.LastName = string.Empty;
            }
        }
    }
}
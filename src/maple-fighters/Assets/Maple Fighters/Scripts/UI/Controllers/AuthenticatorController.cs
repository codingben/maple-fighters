using Scripts.UI.Windows;
using UI.Manager;
using UnityEngine;

namespace Scripts.UI.Controllers
{
    public class AuthenticatorController : MonoBehaviour
    {
        private ILoginView loginView;
        private IRegistrationView registrationView;

        private AuthenticationValidator authenticationValidator;

        private void Awake()
        {
            authenticationValidator = new AuthenticationValidator();

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
            UIAuthenticationDetails authenticationDetails)
        {
            string message;

            var email = authenticationDetails.Email;
            var password = authenticationDetails.Password;

            if (authenticationValidator.IsEmptyEmailAddress(email, out message)
                || authenticationValidator.IsInvalidEmailAddress(email, out message)
                || authenticationValidator.IsEmptyPassword(password, out message)
                || authenticationValidator.IsPasswordTooShort(password, out message))
            {
                ShowNotice(message);
            }
            else
            {
                // TODO: Login
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
            string message;

            var email = uiRegistrationDetails.Email;
            var password = uiRegistrationDetails.Password;
            var confirmPassword = uiRegistrationDetails.ConfirmPassword;
            var firstName = uiRegistrationDetails.FirstName;
            var lastName = uiRegistrationDetails.LastName;

            if (authenticationValidator.IsEmptyEmailAddress(email, out message)
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
                ShowNotice(message);
            }
            else
            {
                // TODO: Register
            }
        }

        private void OnBackButtonClicked()
        {
            HideRegistrationWindow();
            ShowLoginWindow();
        }

        private void ShowNotice(string message)
        {
            // TODO: Use event bus system
            var noticeController = FindObjectOfType<NoticeController>();
            if (noticeController != null)
            {
                noticeController.Show(message);
            }
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
    }
}
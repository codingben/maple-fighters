using System;
using Scripts.UI.Windows;
using UI.Manager;
using UnityEngine;

namespace Scripts.UI.Controllers
{
    public class AuthenticatorController : MonoBehaviour
    {
        public event Action<UIAuthenticationDetails> Login;

        public event Action<UIRegistrationDetails> Register;

        [Header("Configuration")]
        [SerializeField]
        private int passwordLength;

        [SerializeField]
        private int firstNameLength;

        [SerializeField]
        private int lastNameLength;

        private ILoginView loginView;
        private IRegistrationView registrationView;

        private void Awake()
        {
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

            if (IsEmptyEmailAddress(email, out message)
                || IsInvalidEmailAddress(email, out message)
                || IsEmptyPassword(password, out message)
                || IsPasswordTooShort(password, out message))
            {
                ShowNotice(message);
            }
            else
            {
                Login?.Invoke(authenticationDetails);
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

            if (IsEmptyEmailAddress(email, out message)
                || IsInvalidEmailAddress(email, out message)
                || IsEmptyPassword(password, out message)
                || IsEmptyConfirmPassword(confirmPassword, out message)
                || IsPasswordTooShort(password, out message)
                || IsConfirmPasswordTooShort(confirmPassword, out message)
                || ArePasswordsDoNotMatch(password, confirmPassword, out message)
                || IsFirstNameEmpty(firstName, out message)
                || IsLastNameEmpty(lastName, out message)
                || IsFirstNameTooShort(firstName, out message)
                || IsLastNameTooShort(lastName, out message))
            {
                ShowNotice(message);
            }
            else
            {
                Register?.Invoke(uiRegistrationDetails);
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
            if (loginView != null)
            {
                loginView.Show();
            }
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
            if (registrationView != null)
            {
                registrationView.Show();
            }
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

        private bool IsEmptyEmailAddress(string value, out string message)
        {
            message = WindowMessages.EmptyEmailAddress;

            return string.IsNullOrWhiteSpace(value);
        }

        private bool IsInvalidEmailAddress(string value, out string message)
        {
            message = WindowMessages.InvalidEmailAddress;
            
            return WindowUtils.IsEmailAddressValid(value) == false;
        }

        private bool IsEmptyPassword(string value, out string message)
        {
            message = WindowMessages.EmptyPassword;
            
            return string.IsNullOrWhiteSpace(value);
        }

        private bool IsPasswordTooShort(string value, out string message)
        {
            message = WindowMessages.ShortPassword;
            
            return value.Length <= passwordLength;
        }

        private bool IsEmptyConfirmPassword(string value, out string message)
        {
            message = WindowMessages.EmptyConfirmPassword;
            
            return string.IsNullOrWhiteSpace(value);
        }

        private bool IsConfirmPasswordTooShort(string value, out string message)
        {
            message = WindowMessages.ShortPassword;

            return value.Length <= passwordLength;
        }

        private bool ArePasswordsDoNotMatch(string value1, string value2, out string message)
        {
            message = WindowMessages.PasswordsDoNotMatch;
            
            return value1 != value2;
        }

        private bool IsFirstNameEmpty(string value, out string message)
        {
            message = WindowMessages.EmptyFirstName;

            return string.IsNullOrWhiteSpace(value);
        }

        private bool IsLastNameEmpty(string value, out string message)
        {
            message = WindowMessages.EmptyLastName;

            return string.IsNullOrWhiteSpace(value);
        }

        private bool IsFirstNameTooShort(string value, out string message)
        {
            message = WindowMessages.ShortFirstName;
            
            return value.Length < firstNameLength;
        }

        private bool IsLastNameTooShort(string value, out string message)
        {
            message = WindowMessages.ShortLastName;
            
            return value.Length < lastNameLength;
        }
    }
}
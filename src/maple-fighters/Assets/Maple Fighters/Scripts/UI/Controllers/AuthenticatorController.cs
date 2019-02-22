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
        private int passwordCharactersLength;

        [SerializeField]
        private int firstNameCharactersLength;

        [SerializeField]
        private int lastNameCharactersLength;

        private LoginWindow loginWindow;
        private RegistrationWindow registrationWindow;

        private void Awake()
        {
            CreateLoginWindow();
            CreateRegistrationWindow();
        }

        private void Start()
        {
            ShowLoginWindow();
        }

        private void CreateLoginWindow()
        {
            loginWindow = UIElementsCreator.GetInstance().Create<LoginWindow>();
            loginWindow.LoginButtonClicked += OnLoginButtonClicked;
            loginWindow.CreateAccountButtonClicked +=
                OnCreateAccountButtonClicked;
        }

        private void CreateRegistrationWindow()
        {
            registrationWindow = UIElementsCreator.GetInstance()
                .Create<RegistrationWindow>();
            registrationWindow.RegisterButtonClicked +=
                OnRegisterButtonClicked;
            registrationWindow.BackButtonClicked += OnBackButtonClicked;
        }

        private void OnDestroy()
        {
            DestroyLoginWindow();
            DestroyRegistrationWindow();
        }

        private void DestroyLoginWindow()
        {
            if (loginWindow != null)
            {
                loginWindow.LoginButtonClicked -= OnLoginButtonClicked;
                loginWindow.CreateAccountButtonClicked -=
                    OnCreateAccountButtonClicked;

                Destroy(loginWindow.gameObject);
            }
        }

        private void DestroyRegistrationWindow()
        {
            if (registrationWindow != null)
            {
                registrationWindow.RegisterButtonClicked -=
                    OnRegisterButtonClicked;
                registrationWindow.BackButtonClicked -= OnBackButtonClicked;

                Destroy(registrationWindow.gameObject);
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
            var firstName = uiRegistrationDetails.Firstname;
            var lastName = uiRegistrationDetails.Lastname;

            if (IsEmptyEmailAddress(email, out message)
                || IsInvalidEmailAddress(email, out message)
                || IsEmptyPassword(password, out message)
                || IsEmptyConfirmPassword(password, out message)
                || IsPasswordTooShort(password, out message)
                || IsConfirmPasswordTooShort(password, out message)
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
            if (loginWindow != null)
            {
                loginWindow.Show();
            }
        }

        private void HideLoginWindow()
        {
            if (loginWindow != null)
            {
                loginWindow.Email = string.Empty;
                loginWindow.Password = string.Empty;
                loginWindow.Hide();
            }
        }

        private void ShowRegistrationWindow()
        {
            if (registrationWindow != null)
            {
                registrationWindow.Show();
            }
        }

        private void HideRegistrationWindow()
        {
            if (registrationWindow != null)
            {
                registrationWindow.Email = string.Empty;
                registrationWindow.Password = string.Empty;
                registrationWindow.ConfirmPassword = string.Empty;
                registrationWindow.FirstName = string.Empty;
                registrationWindow.LastName = string.Empty;
                registrationWindow.Hide();
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
            
            return value.Length <= passwordCharactersLength;
        }

        private bool IsEmptyConfirmPassword(string value, out string message)
        {
            message = WindowMessages.EmptyConfirmPassword;
            
            return string.IsNullOrWhiteSpace(value);
        }

        private bool IsConfirmPasswordTooShort(string value, out string message)
        {
            message = WindowMessages.ShortPassword;

            return value.Length <= passwordCharactersLength;
        }

        private bool ArePasswordsDoNotMatch(string a, string b, out string message)
        {
            message = WindowMessages.PasswordsDoNotMatch;
            
            return a != b;
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
            
            return value.Length < firstNameCharactersLength;
        }

        private bool IsLastNameTooShort(string value, out string message)
        {
            message = WindowMessages.ShortLastName;
            
            return value.Length < lastNameCharactersLength;
        }
    }
}
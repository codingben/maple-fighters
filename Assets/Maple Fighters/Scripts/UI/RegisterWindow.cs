using CommonTools.Log;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Scripts.UI
{
    public class RegisterWindow : UserInterfaceWindowFadeEffect
    {
        [Header("Configuration")]
        [SerializeField] private int passwordCharacters;
        [SerializeField] private int firstLastNameCharacters;
        [Header("Input Fields")]
        [SerializeField] private TMP_InputField emailInputField;
        [SerializeField] private TMP_InputField passwordInputField;
        [SerializeField] private TMP_InputField rePasswordInputField;
        [SerializeField] private TMP_InputField firstNameInputField;
        [SerializeField] private TMP_InputField lastNameInputField;
        [Header("Buttons")]
        [SerializeField] private Button backButton;
        [SerializeField] private Button registerButton;

        private LoginWindow loginWindow;

        private void Start()
        {
            UserInterfaceContainer.Instance.AddOnly(this);

            backButton.onClick.AddListener(OnBackButtonClicked);
            registerButton.onClick.AddListener(OnRegisterButtonClicked);
        }

        private void OnBackButtonClicked()
        {
            Hide();
            ResetInputFields();

            if (loginWindow == null)
            {
                loginWindow = UserInterfaceContainer.Instance.Get<LoginWindow>().AssertNotNull();
            }

            loginWindow.Show();
        }

        private void OnRegisterButtonClicked()
        {
            if (emailInputField.text == string.Empty)
            {
                ShowNotice("Email address can not be empty.");
                return;
            }

            if (!emailInputField.text.IsValidEmailAddress())
            {
                ShowNotice("Email address is not valid.");
                return;
            }

            if (passwordInputField.text == string.Empty || rePasswordInputField.text == string.Empty)
            {
                ShowNotice("Passwords can not be empty.");
                return;
            }

            if (passwordInputField.text.Length <= passwordCharacters)
            {
                ShowNotice("Please enter a longer password.");
                return;
            }

            if (firstNameInputField.text.Length < firstLastNameCharacters || 
                lastNameInputField.text.Length < firstLastNameCharacters)
            {
                ShowNotice("First or last name is too short.");
                return;
            }

            if (passwordInputField.text != rePasswordInputField.text)
            {
                ShowNotice("Passwords are not match.");
                return;
            }

            LogUtils.Log(MessageBuilder.Trace());
        }

        private void ResetInputFields()
        {
            emailInputField.text = string.Empty;
            passwordInputField.text = string.Empty;
            rePasswordInputField.text = string.Empty;
            firstNameInputField.text = string.Empty;
            lastNameInputField.text = string.Empty;
        }

        private void ShowNotice(string message)
        {
            Hide();

            var noticeWindow = UserInterfaceContainer.Instance.Add<NoticeWindow>();
            noticeWindow.Initialize(message, delegate { UserInterfaceContainer.Instance.Get<RegisterWindow>().Show(); });
            noticeWindow.Show();
        }
    }
}
using CommonTools.Log;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Scripts.UI
{
    public class LoginWindow : UserInterfaceWindowFadeEffect
    {
        [Header("Configuration")]
        [SerializeField] private int passwordCharacters;
        [Header("Input Fields")]
        [SerializeField] private TMP_InputField emailInputField;
        [SerializeField] private TMP_InputField passwordInputField;
        [Header("Buttons")]
        [SerializeField] private Button loginButton;
        [SerializeField] private Button registerButton;

        private RegisterWindow registerWindow;

        private void Start()
        {
            UserInterfaceContainer.Instance.AddOnly(this);

            loginButton.onClick.AddListener(OnLoginButtonClicked);
            registerButton.onClick.AddListener(OnRegisterButtonClicked);
        }

        private void OnLoginButtonClicked()
        {
            if (!emailInputField.text.IsValidEmailAddress())
            {
                ShowNotice("Email address is not valid.");
                return;
            }

            if (passwordInputField.text.Length <= passwordCharacters)
            {
                ShowNotice("Password is not match.");
                return;
            }

            LogUtils.Log(MessageBuilder.Trace());
        }

        private void OnRegisterButtonClicked()
        {
            Hide();
            ResetInputFields();

            if (registerWindow == null)
            {
                registerWindow = UserInterfaceContainer.Instance.Get<RegisterWindow>().AssertNotNull();
            }

            registerWindow.Show();
        }

        private void ResetInputFields()
        {
            emailInputField.text = string.Empty;
            passwordInputField.text = string.Empty;
        }

        private void ShowNotice(string message)
        {
            Hide();

            var noticeWindow = UserInterfaceContainer.Instance.Add<NoticeWindow>();
            noticeWindow.Initialize(message, delegate { UserInterfaceContainer.Instance.Get<LoginWindow>().Show(); });
            noticeWindow.Show();
        }
    }
}
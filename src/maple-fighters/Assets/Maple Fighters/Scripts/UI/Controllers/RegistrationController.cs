using System;
using Scripts.UI.Windows;
using UI.Manager;
using UnityEngine;

namespace Scripts.UI.Controllers
{
    public class RegistrationController : MonoBehaviour
    {
        public event Action<UIRegistrationDetails> Register;

        public event Action Back;

        private RegistrationWindow registrationWindow;

        private void Awake()
        {
            registrationWindow = UIElementsCreator.GetInstance()
                .Create<RegistrationWindow>();
            registrationWindow.RegisterButtonClicked += OnRegisterButtonClicked;
            registrationWindow.BackButtonClicked += OnBackButtonClicked;
            registrationWindow.ShowNotice += OnShowNotice;
        }

        private void Start()
        {
            // TODO: Use event bus system
            var loginController = FindObjectOfType<LoginController>();
            if (loginController != null)
            {
                loginController.Register += OnRegister;
            }
        }

        private void OnDestroy()
        {
            if (registrationWindow != null)
            {
                registrationWindow.RegisterButtonClicked -= OnRegisterButtonClicked;
                registrationWindow.BackButtonClicked -= OnBackButtonClicked;

                Destroy(registrationWindow.gameObject);
            }

            // TODO: Use event bus system
            var loginController = FindObjectOfType<LoginController>();
            if (loginController != null)
            {
                loginController.Register -= OnRegister;
            }
        }

        private void OnShowNotice(string message)
        {
            NoticeController.GetInstance().Show(message);
        }

        private void OnRegister()
        {
            registrationWindow.Show();
        }

        private void OnRegisterButtonClicked(
            UIRegistrationDetails uiRegistrationDetails)
        {
            if (RegistrationConnectionProvider.GetInstance().IsConnected())
            {
                Register?.Invoke(uiRegistrationDetails);
            }
            else
            {
                RegistrationConnectionProvider.GetInstance()
                    .Connect(
                        () => Register?.Invoke(
                            uiRegistrationDetails));
            }
        }

        private void OnBackButtonClicked()
        {
            if (registrationWindow != null)
            {
                registrationWindow.Hide();
                registrationWindow.ResetInputFields();
            }

            Back?.Invoke();
        }
    }
}
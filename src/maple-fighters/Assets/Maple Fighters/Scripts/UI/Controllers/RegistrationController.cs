using System;
using Scripts.UI.Windows;
using Scripts.Utils;
using UI.Manager;

namespace Scripts.UI.Controllers
{
    public class RegistrationController : MonoSingleton<RegistrationController>
    {
        public event Action<UIRegistrationDetails> Register;

        public event Action Back;

        private RegistrationWindow registrationWindow;

        protected override void OnAwake()
        {
            base.OnAwake();

            registrationWindow = UIElementsCreator.GetInstance()
                .Create<RegistrationWindow>();
            registrationWindow.RegisterButtonClicked += OnRegisterButtonClicked;
            registrationWindow.BackButtonClicked += OnBackButtonClicked;
        }

        private void Start()
        {
            LoginController.GetInstance().Register += OnRegister;
        }

        private void OnRegister()
        {
            registrationWindow.Show();
        }

        protected override void OnDestroying()
        {
            base.OnDestroying();

            if (registrationWindow != null)
            {
                LoginController.GetInstance().Register -= OnRegister;

                registrationWindow.RegisterButtonClicked -= OnRegisterButtonClicked;
                registrationWindow.BackButtonClicked -= OnBackButtonClicked;

                Destroy(registrationWindow.gameObject);
            }
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
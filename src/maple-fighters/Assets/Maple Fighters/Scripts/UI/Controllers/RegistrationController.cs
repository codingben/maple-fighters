using System;
using Scripts.UI.Windows;
using Scripts.Utils;
using UI.Manager;

namespace Scripts.UI.Controllers
{
    public class RegistrationController : MonoSingleton<RegistrationController>
    {
        public event Action<UIRegistrationDetails> RegisterButtonClicked;

        public event Action BackButtonClicked;

        private RegistrationWindow registrationWindow;

        protected override void OnAwake()
        {
            base.OnAwake();

            registrationWindow = UIElementsCreator.GetInstance()
                .Create<RegistrationWindow>();
            registrationWindow.RegisterButtonClicked += OnRegisterButtonClicked;
            registrationWindow.BackButtonClicked += OnBackButtonClicked;
        }
        
        protected override void OnDestroying()
        {
            base.OnDestroying();

            if (registrationWindow != null)
            {
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
                RegisterButtonClicked?.Invoke(uiRegistrationDetails);
            }
            else
            {
                RegistrationConnectionProvider.GetInstance()
                    .Connect(
                        () => RegisterButtonClicked?.Invoke(
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

            BackButtonClicked?.Invoke();
        }
    }
}
using System;
using UI.Manager;

namespace Scripts.UI.Authenticator
{
    public interface IRegistrationView : IView
    {
        event Action<UIRegistrationDetails> RegisterButtonClicked;

        event Action BackButtonClicked;

        string Email { set; }

        string Password { set; }

        string ConfirmPassword { set; }

        string FirstName { set; }

        string LastName { set; }

        void EnableInteraction();

        void DisableInteraction();
    }
}
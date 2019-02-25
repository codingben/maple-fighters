using System;

namespace Scripts.UI.Windows
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
    }
}
using System;
using UI;

namespace Scripts.UI.Authenticator
{
    public interface ILoginView : IView
    {
        event Action<UIAuthenticationDetails> LoginButtonClicked;

        event Action CreateAccountButtonClicked;
        
        event Action LoginAsGuestButtonClicked;

        string Email { set; }

        string Password { set; }

        void EnableInteraction();

        void DisableInteraction();
    }
}
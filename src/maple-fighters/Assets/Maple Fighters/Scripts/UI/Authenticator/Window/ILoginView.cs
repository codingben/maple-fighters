using System;
using UI;

namespace Scripts.UI.Authenticator
{
    public interface ILoginView : IView
    {
        event Action<UIAuthenticationDetails> LoginButtonClicked;

        event Action CreateAccountButtonClicked;

        string Email { get; set; }

        string Password { set; }

        void EnableInteraction();

        void DisableInteraction();
    }
}
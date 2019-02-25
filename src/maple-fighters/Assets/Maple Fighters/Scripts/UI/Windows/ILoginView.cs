using System;

namespace Scripts.UI.Windows
{
    public interface ILoginView : IView
    {
        event Action<UIAuthenticationDetails> LoginButtonClicked;

        event Action CreateAccountButtonClicked;

        string Email { set; }

        string Password { set; }
    }
}
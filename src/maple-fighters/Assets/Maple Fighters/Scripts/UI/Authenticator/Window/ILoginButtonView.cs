using System;
using UI;

namespace Scripts.UI.Authenticator
{
    public interface ILoginButtonView : IView
    {
        event Action ButtonClicked;
    }
}
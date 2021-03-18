using System;
using UI;

namespace Scripts.UI.ScreenFade
{
    public interface IScreenFadeView : IView
    {
        event Action FadeInCompleted;

        event Action FadeOutCompleted;
    }
}
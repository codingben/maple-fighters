using System;

namespace Scripts.UI.ScreenFade
{
    public interface IScreenFadeView : IView
    {
        event Action FadeOutCompleted;
    }
}
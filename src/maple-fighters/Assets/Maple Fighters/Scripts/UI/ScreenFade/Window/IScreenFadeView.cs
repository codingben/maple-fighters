using System;

namespace Scripts.UI
{
    public interface IScreenFadeView : IView
    {
        event Action FadeOutCompleted;
    }
}
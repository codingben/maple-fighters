using System;
using Scripts.UI.Windows;

namespace Scripts.UI
{
    public interface IScreenFadeView : IView
    {
        event Action FadeOutCompleted;
    }
}
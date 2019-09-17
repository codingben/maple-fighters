using System;

namespace Scripts.UI.Windows
{
    public interface INoticeView : IView
    {
        event Action FadeOutCompleted;

        event Action OkButtonClicked;

        string Message { set; }

        void ShowBackground();

        void HideBackground();
    }
}
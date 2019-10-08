using System;
using UI.Manager;

namespace Scripts.UI.Notice
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
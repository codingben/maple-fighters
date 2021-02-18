namespace UI.Manager
{
    public interface IView
    {
        bool IsShown { get; }

        void Show();

        void Hide();
    }
}
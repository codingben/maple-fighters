namespace UI
{
    public interface IView
    {
        bool IsShown { get; }

        void Show();

        void Hide();
    }
}
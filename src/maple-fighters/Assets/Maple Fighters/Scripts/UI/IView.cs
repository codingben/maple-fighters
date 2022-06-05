namespace UI
{
    /// <summary>
    /// Base interface for views (e.g. ISampleWindow: IView).
    /// </summary>
    public interface IView
    {
        bool IsShown { get; }

        void Show();

        void Hide();
    }
}
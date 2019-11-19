using UI.Manager;

namespace Scripts.UI.CharacterSelection
{
    public interface ILoadingView : IView
    {
        ILoadingAnimation LoadingAnimation { get; }
    }
}
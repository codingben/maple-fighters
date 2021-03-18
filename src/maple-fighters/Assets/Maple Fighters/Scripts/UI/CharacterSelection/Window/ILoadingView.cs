using UI;

namespace Scripts.UI.CharacterSelection
{
    public interface ILoadingView : IView
    {
        ILoadingAnimation LoadingAnimation { get; }
    }
}
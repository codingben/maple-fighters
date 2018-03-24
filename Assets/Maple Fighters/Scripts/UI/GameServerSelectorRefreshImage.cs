using Scripts.UI.Core;

namespace Scripts.UI.Windows
{
    public class GameServerSelectorRefreshImage : UserInterfaceBaseFadeEffect
    {
        public override void Hide()
        {
            Hide(() => UserInterfaceContainer.Instance.Remove(this));
        }
    }
}
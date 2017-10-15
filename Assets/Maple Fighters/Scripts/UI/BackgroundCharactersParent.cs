using Scripts.UI.Core;

namespace Scripts.UI
{
    public class BackgroundCharactersParent : UniqueUserInterfaceBase
    {
        public override void Hide()
        {
            UserInterfaceContainer.Instance.Remove(this);
        }
    }
}
using Scripts.UI.Core;

namespace Scripts.UI
{
    public class BackgroundImage : UniqueUserInterfaceBase
    {
        public override void Hide()
        {
            UserInterfaceContainer.GetInstance()?.Remove(this);
        }
    }
}
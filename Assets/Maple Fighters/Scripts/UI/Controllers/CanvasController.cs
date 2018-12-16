using Scripts.UI.Core;

namespace Scripts.UI.Controllers
{
    public class CanvasController : UserInterfaceBaseFadeEffect
    {
        protected override void OnAwake()
        {
            base.OnAwake();

            UserInterfaceContainer.GetInstance().AddOnly(this);
        }
    }
}
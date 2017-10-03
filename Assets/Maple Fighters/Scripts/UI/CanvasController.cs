namespace Scripts.UI
{
    public class CanvasController : UserInterfaceBaseFadeEffect
    {
        protected override void OnAwake()
        {
            base.OnAwake();

            UserInterfaceContainer.Instance.AddOnly(this);
        }
    }
}
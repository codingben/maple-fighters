namespace ServerApplication.Common.ComponentModel
{
    public class CommonComponent : IComponent
    {
        public void Dispose()
        {
            OnDestroy();
        }

        protected virtual void OnDestroy()
        {
            // Left blank intentionally
        }
    }
}
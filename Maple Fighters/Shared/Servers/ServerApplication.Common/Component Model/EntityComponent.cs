using Shared.ServerApplication.Common.PeerLogic;

namespace ServerApplication.Common.ComponentModel
{
    public class EntityComponent : CommonComponent
    {
        protected IEntity GameObject { get; set; }

        public void Awake(IEntity gameObject)
        {
            GameObject = gameObject;

            OnAwake();
        }

        protected virtual void OnAwake()
        {
            // Left blank intentionally
        }
    }
}
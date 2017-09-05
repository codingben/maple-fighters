using ServerApplication.Common.ComponentModel;

namespace Game.InterestManagement
{
    public class GameObjectComponent : CommonComponent
    {
        protected IGameObject GameObject { get; set; }

        public void Awake(IGameObject gameObject)
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
using Game.Application.Components;

namespace Game.Application.Objects.Components
{
    public class MobHealthController : ComponentBase, IMobHealthController
    {
        private int healthAmount;
        private bool isDead;

        protected override void OnAwake()
        {
            base.OnAwake();

            var mobConfigDataProvider = Components.Get<IMobConfigDataProvider>();
            var mobConfigData = mobConfigDataProvider.Provide();

            healthAmount = mobConfigData.Health;
        }

        public void Damage(int amount)
        {
            if (isDead) return;

            healthAmount -= amount;

            if (healthAmount <= 0)
            {
                isDead = true;

                var gameObjectGetter = Components.Get<IGameObjectGetter>();
                var gameObject = gameObjectGetter?.Get();
                gameObject?.Dispose();
            }
        }
    }
}
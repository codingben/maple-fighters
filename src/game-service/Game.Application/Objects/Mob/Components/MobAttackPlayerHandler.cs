using Game.Application.Components;

namespace Game.Application.Objects.Components
{
    public class MobAttackPlayerHandler : ComponentBase, IMobAttackPlayerHandler
    {
        private readonly int id;
        private IMobBehaviourManager behaviourManager;

        public MobAttackPlayerHandler(int id)
        {
            this.id = id;
        }

        protected override void OnAwake()
        {
            base.OnAwake();

            behaviourManager = Components.Get<IMobBehaviourManager>();
        }

        public void Attack(IGameObject player)
        {
            if (CanAttack())
            {
                var playerAttacked = player?.Components.Get<IPlayerAttacked>();
                playerAttacked?.Attack(id);
            }
        }

        private bool CanAttack()
        {
            return behaviourManager.GetBehaviour() == MobBehaviourType.Move;
        }
    }
}
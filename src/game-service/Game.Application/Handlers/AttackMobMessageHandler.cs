using Game.Application.Objects;
using Game.Application.Objects.Components;
using Game.Messages;
using Game.MessageTools;

namespace Game.Application.Handlers
{
    public class AttackMobMessageHandler : IMessageHandler<AttackMobMessage>
    {
        private readonly IProximityChecker proximityChecker;

        public AttackMobMessageHandler(IGameObject player)
        {
            proximityChecker = player.Components.Get<IProximityChecker>();
        }

        public void Handle(AttackMobMessage message)
        {
            var nearbyGameObjects = proximityChecker?.GetNearbyGameObjects();
            var mobId = message.MobId;
            var damageAmount = message.DamageAmount;

            foreach (var gameObject in nearbyGameObjects)
            {
                if (gameObject.Id != mobId)
                {
                    continue;
                }

                var behaviourManager =
                    gameObject.Components.Get<IMobBehaviourManager>();
                var healthController =
                    gameObject.Components.Get<IMobHealthController>();

                behaviourManager?.ChangeBehaviour(type: MobBehaviourType.Attacked);
                healthController?.Damage(damageAmount);
            }
        }
    }
}
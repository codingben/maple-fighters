using System;
using Game.Application.Objects;
using Game.Application.Objects.Components;
using Game.Messages;
using Game.MessageTools;
using InterestManagement;

namespace Game.Application.Handlers
{
    public class AttackMobMessageHandler : IMessageHandler<AttackMobMessage>
    {
        private readonly IGameObject player;
        private readonly IProximityChecker proximityChecker;

        public AttackMobMessageHandler(IGameObject player)
        {
            this.player = player;

            proximityChecker = player.Components.Get<IProximityChecker>();
        }

        public void Handle(AttackMobMessage message)
        {
            var nearbyGameObjects = proximityChecker?.GetNearbyGameObjects();
            var mobId = message.MobId;
            var distance = message.Distance;
            var damageAmount = message.DamageAmount;

            foreach (var gameObject in nearbyGameObjects)
            {
                var playerPosition = player.Transform.Position;
                var nearbyPosition = gameObject.Transform.Position;

                if (Vector2.Distance(playerPosition, nearbyPosition) >= distance)
                {
                    continue;
                }

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
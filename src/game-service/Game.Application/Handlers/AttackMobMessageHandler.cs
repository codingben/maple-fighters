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
        private readonly Random random = new();

        public AttackMobMessageHandler(IGameObject player)
        {
            this.player = player;

            proximityChecker = player.Components.Get<IProximityChecker>();
        }

        public void Handle(AttackMobMessage message)
        {
            var nearbyGameObjects = proximityChecker?.GetNearbyGameObjects();
            var distance = 1.5f;

            foreach (var gameObject in nearbyGameObjects)
            {
                var playerPosition = player.Transform.Position;
                var nearbyPosition = gameObject.Transform.Position;

                if (Vector2.Distance(playerPosition, nearbyPosition) >= distance)
                {
                    continue;
                }

                var behaviourManager =
                    gameObject.Components.Get<IMobBehaviourManager>();
                behaviourManager?.ChangeBehaviour(type: MobBehaviourType.Attacked);

                var damageAmount =
                    random.Next(minValue: 0, maxValue: 50);
                var healthController =
                    gameObject.Components.Get<IMobHealthController>();
                healthController?.Damage(damageAmount);
            }
        }
    }
}
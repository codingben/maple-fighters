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
            var distance = 1.5f;

            foreach (var nearbyGameObject in nearbyGameObjects)
            {
                var playerPosition = player.Transform.Position;
                var nearbyPosition = nearbyGameObject.Transform.Position;

                if (Vector2.Distance(playerPosition, nearbyPosition) >= distance)
                {
                    continue;
                }

                var mobBehaviourManager =
                    nearbyGameObject.Components.Get<IMobBehaviourManager>();
                mobBehaviourManager?.ChangeBehaviour(type: MobBehaviourType.Attacked);
            }
        }
    }
}
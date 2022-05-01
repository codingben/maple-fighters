using Game.Application.Components;
using Game.Messages;
using Game.MessageTools;
using Game.Application.Objects;
using Game.Application.Objects.Components;

namespace Game.Application.Handlers
{
    public class ChangeSceneMessageHandler : IMessageHandler<ChangeSceneMessage>
    {
        private readonly IProximityChecker proximityChecker;
        private readonly IMessageSender messageSender;
        private readonly IPresenceSceneProvider presenceSceneProvider;
        private readonly IGameSceneCollection gameSceneCollection;

        public ChangeSceneMessageHandler(IGameObject player, IGameSceneCollection gameSceneCollection)
        {
            this.gameSceneCollection = gameSceneCollection;

            proximityChecker =
                player.Components.Get<IProximityChecker>();
            presenceSceneProvider =
                player.Components.Get<IPresenceSceneProvider>();
            messageSender =
                player.Components.Get<IMessageSender>();
        }

        public void Handle(ChangeSceneMessage message)
        {
            var portalId = message.PortalId;
            var portal = GetPortal(portalId);
            if (portal != null)
            {
                var portalTeleportationData =
                    portal.Components.Get<IPortalTeleportationData>();
                var map = portalTeleportationData.GetDestinationMap();
                var mapName = ((Map)map).ToString().ToLower();

                if (gameSceneCollection.TryGet(mapName, out var gameScene))
                {
                    SetPlayerPresenceScene(gameScene);
                    SendSceneChangedMessage(map);
                }
            }
        }

        private void SetPlayerPresenceScene(IGameScene gameScene)
        {
            presenceSceneProvider.SetScene(gameScene);
        }

        private void SendSceneChangedMessage(byte map)
        {
            var messageCode = (byte)MessageCodes.SceneChanged;
            var message = new SceneChangedMessage
            {
                Map = map
            };

            messageSender.SendMessage(messageCode, message);
        }

        private IGameObject GetPortal(int id)
        {
            var nearbyGameObjects = proximityChecker?.GetNearbyGameObjects();

            foreach (var gameObject in nearbyGameObjects)
            {
                if (gameObject.Id != id)
                {
                    continue;
                }

                return gameObject;
            }

            return null;
        }
    }
}
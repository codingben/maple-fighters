using Game.Application.Components;
using Game.Messages;
using Game.Network;
using Game.Application.Objects;
using Game.Application.Objects.Components;

namespace Game.Application.Handlers
{
    public class ChangeSceneMessageHandler : IMessageHandler<ChangeSceneMessage>
    {
        private readonly IProximityChecker proximityChecker;
        private readonly IMessageSender messageSender;
        private readonly IPresenceMapProvider presenceMapProvider;
        private readonly IGameSceneCollection gameSceneCollection;

        public ChangeSceneMessageHandler(IGameObject player, IGameSceneCollection gameSceneCollection)
        {
            this.gameSceneCollection = gameSceneCollection;

            proximityChecker = player.Components.Get<IProximityChecker>();
            presenceMapProvider = player.Components.Get<IPresenceMapProvider>();
            messageSender = player.Components.Get<IMessageSender>();
        }

        public void Handle(ChangeSceneMessage message)
        {
            var portalId = message.PortalId;
            var portal = GetPortal(portalId);
            if (portal != null)
            {
                var portalData = portal.Components.Get<IPortalData>();
                var map = portalData.GetMap();
                var gameSceneExists =
                    gameSceneCollection.TryGet((Map)map, out var gameScene);
                if (gameSceneExists)
                {
                    presenceMapProvider.SetMap(gameScene);

                    SendSceneChangedMessage(map);
                }
            }
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
            var nearbyGameObjects = proximityChecker.GetNearbyGameObjects();

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
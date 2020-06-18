using Game.Application.Components;
using Game.Application.Messages;
using Game.Application.Network;
using Game.Application.Objects;
using Game.Application.Objects.Components;

namespace Game.Application.Handlers
{
    public class ChangeSceneHandler : IMessageHandler
    {
        private IPresenceSceneProvider presenceSceneProvider;
        private IGameSceneContainer gameSceneContainer;
        private IProximityChecker proximityChecker;
        private IMessageSender messageSender;

        public ChangeSceneHandler(
            IPresenceSceneProvider presenceSceneProvider,
            IGameSceneContainer gameSceneContainer,
            IProximityChecker proximityChecker,
            IMessageSender messageSender)
        {
            this.presenceSceneProvider = presenceSceneProvider;
            this.gameSceneContainer = gameSceneContainer;
            this.proximityChecker = proximityChecker;
            this.messageSender = messageSender;
        }

        public void Handle(byte[] rawData)
        {
            var message =
                MessageUtils.DeserializeMessage<ChangeSceneMessage>(rawData);
            var portalId = message.PortalId;
            var portal = GetPortal(portalId);
            if (portal != null)
            {
                var portalData = portal.Components.Get<IPortalData>();
                var map = portalData.GetMap();
                var sceneExists =
                    gameSceneContainer.TryGetScene((Map)map, out var newScene);
                if (sceneExists)
                {
                    presenceSceneProvider.SetScene(newScene);

                    SendSceneChangedMessage(map);
                }
            }
        }

        private void SendSceneChangedMessage(byte map)
        {
            var message = new SceneChangedMessage
            {
                Map = map
            };

            messageSender.SendMessage((byte)MessageCodes.SceneChanged, message);
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
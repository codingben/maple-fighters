using Game.Application.Components;
using Game.Application.Messages;
using Game.Application.Network;
using Game.Application.Objects;
using Game.Application.Objects.Components;

namespace Game.Application.Handlers
{
    public class ChangeSceneMessageHandler : IMessageHandler
    {
        private IMessageSender messageSender;
        private IProximityChecker proximityChecker;
        private IGameSceneCollection gameSceneCollection;
        private IPresenceSceneProvider presenceSceneProvider;

        public ChangeSceneMessageHandler(
            IMessageSender messageSender,
            IProximityChecker proximityChecker,
            IGameSceneCollection gameSceneCollection,
            IPresenceSceneProvider presenceSceneProvider)
        {
            this.messageSender = messageSender;
            this.proximityChecker = proximityChecker;
            this.gameSceneCollection = gameSceneCollection;
            this.presenceSceneProvider = presenceSceneProvider;
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
                    gameSceneCollection.TryGetScene((Map)map, out var newScene);
                if (sceneExists)
                {
                    presenceSceneProvider.SetScene(newScene);
                    proximityChecker.ChangeScene();

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
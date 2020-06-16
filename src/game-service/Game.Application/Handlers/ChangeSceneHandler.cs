using Game.Application.Messages;
using Game.Application.Network;
using Game.Application.Objects;
using Game.Application.Objects.Components;

namespace Game.Application.Handlers
{
    public class ChangeSceneHandler : IMessageHandler
    {
        private IProximityChecker proximityChecker;
        private IMessageSender messageSender;

        public ChangeSceneHandler(IProximityChecker proximityChecker, IMessageSender messageSender)
        {
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

                SendSceneChangedMessage(map);
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
            foreach (var gameObject in proximityChecker.GetGameObjects())
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
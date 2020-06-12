using Common.ComponentModel;
using System.Collections.Generic;
using WebSocketSharp;
using WebSocketSharp.Server;
using Game.Application.Objects;
using Game.Application.Objects.Components;
using Common.Components;
using Game.Application.Components;

namespace Game.Application
{
    public class GameService : WebSocketBehavior, IGameService
    {
        private readonly IExposedComponents components;
        private readonly ISessionDataContainer sessionDataContainer;
        private readonly IDictionary<byte, IMessageHandler> handlers;
        private readonly IGameObject player;

        public GameService(IExposedComponents components)
        {
            this.components = components;
            this.sessionDataContainer = components.Get<ISessionDataContainer>();

            handlers = new Dictionary<byte, IMessageHandler>();
            player = CreatePlayerGameObject();
        }

        protected override void OnOpen()
        {
            sessionDataContainer.AddSessionData(player.Id, new SessionData(ID));

            AddHandlerForChangePlayerPosition();
        }

        protected override void OnClose(CloseEventArgs eventArgs)
        {
            sessionDataContainer.RemoveSessionData(player.Id);

            RemoveHandlerForChangePlayerPosition();
        }

        protected override void OnError(ErrorEventArgs eventArgs)
        {
            // TODO: Log $"e.Message"
        }

        protected override void OnMessage(MessageEventArgs eventArgs)
        {
            if (eventArgs.IsBinary)
            {
                var message = MessageUtils.FromMessage<MessageData>(eventArgs.RawData);
                var code = message.Code;
                var rawData = message.RawData;

                if (handlers.TryGetValue(code, out var handler))
                {
                    handler?.Handle(rawData);
                }
            }
            else
            {
                // TODO: Log "Only binary data is allowed."
            }
        }

        private void AddHandlerForChangePlayerPosition()
        {
            var transform = player.Transform;
            var prxomitiyChecker = player.Components.Get<IProximityChecker>();
            var handler = new ChangePositionMessageHandler(transform, prxomitiyChecker, SendMessage);

            handlers.Add((byte)MessageCodes.ChangePlayerPosition, handler);
        }

        private void RemoveHandlerForChangePlayerPosition()
        {
            handlers.Remove((byte)MessageCodes.ChangePlayerPosition);
        }

        public void SendMessage(byte[] data, int id)
        {
            if (sessionDataContainer.GetSessionData(id, out var sessionData))
            {
                Sessions.SendTo(data, sessionData.Id);
            }
        }

        private IGameObject CreatePlayerGameObject()
        {
            var idGenerator = components.Get<IIdGenerator>();
            var id = idGenerator.GenerateId();
            var player = new GameObject(id, nameof(GameObjectType.Player));
            var gameSceneContainer = components.Get<IGameSceneContainer>();
            gameSceneContainer.TryGetScene(Map.Lobby, out var scene);

            player.Transform.SetPosition(scene.PlayerSpawnData.Position);
            player.Transform.SetSize(scene.PlayerSpawnData.Size);

            // TODO: Dispose won't be called
            player.Components.Add(new GameObjectGetter(player));
            player.Components.Add(new AnimationData());
            player.Components.Add(new PresenceSceneProvider(scene));
            player.Components.Add(new ProximityChecker());

            return player;
        }
    }
}
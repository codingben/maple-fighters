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
    public class GameService : WebSocketBehavior
    {
        private readonly IExposedComponents components;
        private readonly IDictionary<byte, IMessageHandler> handlers;
        private readonly IGameObject player;

        public GameService(IExposedComponents components)
        {
            this.components = components;

            handlers = new Dictionary<byte, IMessageHandler>();
            player = CreatePlayerGameObject();
        }

        protected override void OnOpen()
        {
            handlers.Add((byte)MessageCodes.ChangePlayerPosition, new ChangePositionMessageHandler(player));
        }

        protected override void OnClose(CloseEventArgs eventArgs)
        {
            handlers.Remove((byte)MessageCodes.ChangePlayerPosition);
        }

        protected override void OnError(ErrorEventArgs eventArgs)
        {
            // TODO: Log $"e.Message"
        }

        protected override void OnMessage(MessageEventArgs eventArgs)
        {
            if (eventArgs.IsBinary)
            {
                var message = MessageUtils.GetMessage<MessageData>(eventArgs.RawData);
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
            player.Components.Add(new PresenceSceneProvider(scene));
            player.Components.Add(new ProximityChecker());

            return player;
        }
    }
}
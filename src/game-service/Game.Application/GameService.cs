using Common.ComponentModel;
using System.Collections.Generic;
using WebSocketSharp;
using WebSocketSharp.Server;
using Game.Application.Objects;
using Common.MathematicsHelper;
using Common.Components;

namespace Game.Application
{
    public class GameService : WebSocketBehavior
    {
        private readonly IExposedComponents components;
        private readonly Dictionary<byte, IMessageHandler> handlers;
        private readonly GameObject player;

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

        private GameObject CreatePlayerGameObject()
        {
            var idGenerator = components.Get<IIdGenerator>();
            var id = idGenerator.GenerateId();
            var name = "Player"; // (Map from Object Type to Name)
            var player = new GameObject(id, name);
            player.Transform.SetPosition(Vector2.Zero);
            player.Transform.SetSize(Vector2.One);

            // Add components

            return player;
        }
    }
}
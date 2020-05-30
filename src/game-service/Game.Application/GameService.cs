using System.Collections.Generic;
using WebSocketSharp;
using WebSocketSharp.Server;

namespace Game.Application
{
    public class GameService : WebSocketBehavior
    {
        private readonly Dictionary<byte, IMessageHandler> handlers;

        public GameService()
        {
            handlers = new Dictionary<byte, IMessageHandler>();
        }

        protected override void OnOpen()
        {
            handlers.Add((byte)MessageCodes.ChangePlayerPosition, new ChangePositionMessageHandler());
        }

        protected override void OnClose(CloseEventArgs e)
        {
            handlers.Remove((byte)MessageCodes.ChangePlayerPosition);
        }

        protected override void OnError(WebSocketSharp.ErrorEventArgs e)
        {
            // TODO: Log $"e.Message"
        }

        protected override void OnMessage(MessageEventArgs e)
        {
            if (e.IsBinary)
            {
                var message = MessageUtils.GetMessage<MessageData>(e.RawData);
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
    }
}
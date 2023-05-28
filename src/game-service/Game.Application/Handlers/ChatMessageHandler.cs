using Game.Application.Components;
using Game.Application.Objects;
using Game.Application.Objects.Components;
using Game.Messages;
using Game.MessageTools;

namespace Game.Application.Handlers
{
    public class ChatMessageHandler : IMessageHandler<ChatMessage>
    {
        private readonly IGameClientCollection gameClientCollection;
        private readonly IMessageSender messageSender;

        public ChatMessageHandler(IGameObject player, IGameClientCollection gameClientCollection)
        {
            this.gameClientCollection = gameClientCollection;

            messageSender = player.Components.Get<IMessageSender>();
        }

        public void Handle(ChatMessage message)
        {
            var senderId = message.SenderId;
            var name = message.Name;
            var content = message.Content;

            SendChatMessage(senderId, name, content);
            SendBubbleNotification(senderId, content);
        }

        private void SendChatMessage(int senderId, string name, string content)
        {
            foreach (var gameClient in gameClientCollection)
            {
                var onnectionProvider = gameClient.Components.Get<IWebSocketConnectionProvider>();
                var messageCode = (byte)MessageCodes.ChatMessage;
                var message = new ChatMessage()
                {
                    SenderId = senderId,
                    Name = name,
                    Content = content,
                    ContentFormatted = GenerateMessageFormatted(name, content)
                };

                onnectionProvider.SendMessage(messageCode, message);
            }
        }

        private void SendBubbleNotification(int senderId, string content)
        {
            var messageCode = (byte)MessageCodes.BubbleNotification;
            var message = new BubbleNotificationMessage()
            {
                NotifierId = senderId,
                Message = content,
                Time = 3 // Seconds
            };

            messageSender.SendMessage(messageCode, message);
            messageSender.SendMessageToNearbyGameObjects(messageCode, message);
        }

        private string GenerateMessageFormatted(string name, string content)
        {
            return $"<b>{name}: {content}</b>";
        }
    }
}
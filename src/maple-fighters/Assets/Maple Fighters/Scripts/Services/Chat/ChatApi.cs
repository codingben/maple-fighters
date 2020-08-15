using System;

namespace Scripts.Services.Chat
{
    public class ChatApi : IChatApi
    {
        public Action<string> ChatMessageReceived { get; }

        public void SendChatMessage(string message)
        {
            throw new NotImplementedException();
        }
    }
}
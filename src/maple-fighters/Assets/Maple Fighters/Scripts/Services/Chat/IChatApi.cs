using System;

namespace Scripts.Services.Chat
{
    public interface IChatApi
    {
        Action<string> ChatMessageReceived { get; set; }

        void SendChatMessage(string message);
    }
}
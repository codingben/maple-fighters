using System;

namespace Scripts.Services.Chat
{
    public interface IChatApi
    {
        Action<string> ChatMessageReceived { get; }

        void SendChatMessage(string message);
    }
}
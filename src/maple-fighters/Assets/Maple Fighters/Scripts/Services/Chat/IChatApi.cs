using System;

namespace Scripts.Services.ChatApi
{
    public interface IChatApi
    {
        Action<string> ChatMessageReceived { get; set; }

        void SendChatMessage(string message);
    }
}
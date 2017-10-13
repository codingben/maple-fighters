using System;
using Chat.Common;

namespace Scripts.Services
{
    public interface IChatService
    {
        event Action Authenticated;

        void Connect();
        void Disconnect();

        void SendChatMessage(ChatMessageRequestParameters parameters);

        UnityEvent<ChatMessageEventParameters> ChatMessageReceived { get; }
    }
}
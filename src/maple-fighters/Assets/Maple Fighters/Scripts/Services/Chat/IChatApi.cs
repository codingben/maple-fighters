using System.Threading.Tasks;
using Chat.Common;
using Network.Scripts;

namespace Scripts.Services.Chat
{
    public interface IChatApi
    {
        UnityEvent<ChatMessageEventParameters> ChatMessageReceived { get; }

        Task SendChatMessage(ChatMessageRequestParameters parameters);
    }
}
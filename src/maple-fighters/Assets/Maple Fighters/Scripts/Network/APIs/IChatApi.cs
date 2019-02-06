using System.Threading.Tasks;
using Chat.Common;
using Scripts.Network.Core;

namespace Scripts.Network.APIs
{
    public interface IChatApi : IApiBase
    {
        Task SendChatMessage(ChatMessageRequestParameters parameters);

        UnityEvent<ChatMessageEventParameters> ChatMessageReceived { get; }
    }
}
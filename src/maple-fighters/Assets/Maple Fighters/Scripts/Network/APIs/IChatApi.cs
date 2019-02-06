using System.Threading.Tasks;
using Chat.Common;

namespace Scripts.Network
{
    public interface IChatApi : IApiBase
    {
        Task SendChatMessage(ChatMessageRequestParameters parameters);

        UnityEvent<ChatMessageEventParameters> ChatMessageReceived { get; }
    }
}
using System.Threading.Tasks;
using Chat.Common;

namespace Scripts.Services
{
    public interface IChatApi
    {
        Task SendChatMessage(ChatMessageRequestParameters parameters);

        UnityEvent<ChatMessageEventParameters> ChatMessageReceived { get; }
    }
}
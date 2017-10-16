using System.Threading.Tasks;
using Chat.Common;
using CommonTools.Coroutines;

namespace Scripts.Services
{
    public interface IChatService : IServiceBase
    {
        Task<AuthenticationStatus> Authenticate(IYield yield);

        void SendChatMessage(ChatMessageRequestParameters parameters);

        UnityEvent<ChatMessageEventParameters> ChatMessageReceived { get; }
    }
}
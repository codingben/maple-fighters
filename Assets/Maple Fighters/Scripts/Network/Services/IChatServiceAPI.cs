using System.Threading.Tasks;
using Authorization.Client.Common;
using Chat.Common;
using CommonTools.Coroutines;

namespace Scripts.Services
{
    public interface IChatServiceAPI : IServiceBase
    {
        Task<AuthorizeResponseParameters> Authorize(IYield yield, AuthorizeRequestParameters parameters);

        void SendChatMessage(ChatMessageRequestParameters parameters);

        UnityEvent<ChatMessageEventParameters> ChatMessageReceived { get; }
    }
}
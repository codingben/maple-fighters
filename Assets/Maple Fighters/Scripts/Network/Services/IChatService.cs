using System.Threading.Tasks;
using Chat.Common;
using CommonTools.Coroutines;

namespace Scripts.Services
{
    public interface IChatService
    {
        Task<ConnectionStatus> Connect(IYield yield);
        void Disconnect();

        Task<AuthenticateStatus> Authenticate(IYield yield);

        void SendChatMessage(ChatMessageRequestParameters parameters);

        UnityEvent<ChatMessageEventParameters> ChatMessageReceived { get; }
    }
}
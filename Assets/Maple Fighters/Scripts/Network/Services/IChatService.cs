using System.Threading.Tasks;
using Chat.Common;
using CommonTools.Coroutines;

namespace Scripts.Services
{
    public interface IChatService : IServiceBase
    {
        Task<AuthenticationStatus> Authenticate(IYield yield);

        UnityEvent<ChatMessageEventParameters> ChatMessageReceived { get; }
    }
}
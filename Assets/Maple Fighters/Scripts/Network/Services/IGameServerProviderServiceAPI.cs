using System.Threading.Tasks;
using Authorization.Client.Common;
using CommonTools.Coroutines;
using GameServerProvider.Client.Common;

namespace Scripts.Services
{
    public interface IGameServerProviderServiceAPI : IServiceBase
    {
        Task<AuthorizeResponseParameters> Authorize(IYield yield, AuthorizeRequestParameters parameters);
        Task<GameServersProviderResponseParameters> ProvideGameServers(IYield yield);
    }
}
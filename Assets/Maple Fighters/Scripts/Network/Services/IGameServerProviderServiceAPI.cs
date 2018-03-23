using System.Threading.Tasks;
using CommonTools.Coroutines;
using GameServerProvider.Client.Common;

namespace Scripts.Services
{
    public interface IGameServerProviderServiceAPI : IPeerLogicBase
    {
        Task<GameServersProviderResponseParameters> ProvideGameServers(IYield yield);
    }
}
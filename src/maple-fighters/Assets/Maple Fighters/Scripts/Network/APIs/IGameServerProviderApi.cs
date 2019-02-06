using System.Threading.Tasks;
using CommonTools.Coroutines;
using GameServerProvider.Client.Common;

namespace Scripts.Network.APIs
{
    public interface IGameServerProviderApi : IApiBase
    {
        Task<GameServersProviderResponseParameters> ProvideGameServersAsync(
            IYield yield);
    }
}
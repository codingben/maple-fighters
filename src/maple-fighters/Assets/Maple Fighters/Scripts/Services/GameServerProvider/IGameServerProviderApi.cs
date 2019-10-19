using System.Threading.Tasks;
using CommonTools.Coroutines;
using GameServerProvider.Client.Common;

namespace Scripts.Services.GameServerProvider
{
    public interface IGameServerProviderApi
    {
        Task<GameServersProviderResponseParameters> ProvideGameServersAsync(IYield yield);
    }
}
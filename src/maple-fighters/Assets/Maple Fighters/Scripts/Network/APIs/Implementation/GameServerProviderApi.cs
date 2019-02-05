using System.Threading.Tasks;
using CommonCommunicationInterfaces;
using CommonTools.Coroutines;
using CommunicationHelper;
using GameServerProvider.Client.Common;

namespace Scripts.Services
{
    public class GameServerProviderApi : ApiBase<GameServerProviderOperations, EmptyEventCode>, IGameServerProviderApi
    {
        public async Task<GameServersProviderResponseParameters>
            ProvideGameServersAsync(IYield yield)
        {
            return 
                await ServerPeerHandler
                    .SendOperationAsync<EmptyParameters, GameServersProviderResponseParameters>(
                        yield,
                        GameServerProviderOperations.ProvideGameServers,
                        new EmptyParameters(),
                        MessageSendOptions.DefaultReliable());
        }
    }
}
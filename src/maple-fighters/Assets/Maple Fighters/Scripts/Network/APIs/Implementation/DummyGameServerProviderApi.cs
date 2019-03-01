using System.Threading.Tasks;
using CommonTools.Coroutines;
using CommunicationHelper;
using GameServerProvider.Client.Common;
using Scripts.Network.Core;

namespace Scripts.Network.APIs
{
    public class DummyGameServerProviderApi : ApiBase<GameServerProviderOperations, EmptyEventCode>, IGameServerProviderApi
    {
        public Task<GameServersProviderResponseParameters>
            ProvideGameServersAsync(IYield yield)
        {
            var gameServers = new[] 
            {
                new GameServerInformationParameters(
                    name: "Game 1",
                    ip: "127.0.0.1",
                    port: 8000,
                    connections: 10,
                    maxConnections: 100)
            };

            return Task.FromResult(
                new GameServersProviderResponseParameters(gameServers));
        }
    }
}
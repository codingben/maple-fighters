using System.Threading.Tasks;
using CommonCommunicationInterfaces;
using CommonTools.Coroutines;
using CommunicationHelper;
using GameServerProvider.Client.Common;
using Network.Scripts;

namespace Scripts.Services.GameServerProvider
{
    internal class DummyGameServerProviderApi : NetworkApi<GameServerProviderOperations, EmptyEventCode>, IGameServerProviderApi
    {
        internal DummyGameServerProviderApi(IServerPeer serverPeer, bool log = false)
            : base(serverPeer, log)
        {
            // Left blank intentionally
        }

        public async Task<GameServersProviderResponseParameters> ProvideGameServersAsync(
            IYield yield)
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

            return 
                await Task.FromResult(
                    new GameServersProviderResponseParameters(gameServers));
        }
    }
}
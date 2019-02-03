using System.Threading.Tasks;
using CommonCommunicationInterfaces;
using CommonTools.Coroutines;
using CommunicationHelper;
using GameServerProvider.Client.Common;

namespace Scripts.Services
{
    public class GameServerProviderApi : IGameServerProviderApi
    {
        public ServerPeerHandler<GameServerProviderOperations, EmptyEventCode> ServerPeer
        {
            get;
        }

        public GameServerProviderApi()
        {
            ServerPeer =
                new ServerPeerHandler<GameServerProviderOperations, EmptyEventCode>();
        }

        public async Task<GameServersProviderResponseParameters>
            ProvideGameServersAsync(IYield yield)
        {
            return 
                await ServerPeer
                    .SendOperation<EmptyParameters, GameServersProviderResponseParameters>(
                        yield,
                        GameServerProviderOperations.ProvideGameServers,
                        new EmptyParameters(),
                        MessageSendOptions.DefaultReliable());
        }
    }
}
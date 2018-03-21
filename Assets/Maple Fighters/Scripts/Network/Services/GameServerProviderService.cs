using System.Threading.Tasks;
using Authorization.Client.Common;
using CommonCommunicationInterfaces;
using CommonTools.Coroutines;
using CommunicationHelper;
using GameServerProvider.Client.Common;

namespace Scripts.Services
{
    public class GameServerProviderService : ServiceBase, IGameServerProviderServiceAPI
    {
        public GameServerProviderService()
        {
            SetServerPeerHandler<ClientOperations, EmptyEventCode>();
        }

        public async Task<AuthorizeResponseParameters> Authorize(IYield yield, AuthorizeRequestParameters parameters)
        {
            return await ServerPeerHandler.SendOperation<AuthorizeRequestParameters, AuthorizeResponseParameters>
                (yield, (byte)ClientOperations.Authorize, parameters, MessageSendOptions.DefaultReliable());
        }

        public async Task<GameServersProviderResponseParameters> ProvideGameServers(IYield yield)
        {
            var parameters = new EmptyParameters();
            return await ServerPeerHandler.SendOperation<EmptyParameters, GameServersProviderResponseParameters>
                (yield, (byte)ClientOperations.ProvideGameServers, parameters, MessageSendOptions.DefaultReliable());
        }
    }
}
using System.Threading.Tasks;
using CommonCommunicationInterfaces;
using CommonTools.Coroutines;
using GameServerProvider.Client.Common;

namespace Scripts.Services
{
    public sealed class GameServerProviderPeerLogic : PeerLogicBase, IGameServerProviderPeerLogicAPI
    {
        public async Task<GameServersProviderResponseParameters> ProvideGameServers(IYield yield)
        {
            var responseParameters = await ServerPeerHandler.SendOperation<EmptyParameters, GameServersProviderResponseParameters>
                (yield, (byte)GameServerProviderOperations.ProvideGameServers, new EmptyParameters(), MessageSendOptions.DefaultReliable());
            return responseParameters;
        }
    }
}
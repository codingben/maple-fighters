using System.Threading.Tasks;
using CommonCommunicationInterfaces;
using CommonTools.Coroutines;
using CommunicationHelper;
using GameServerProvider.Client.Common;
using Network.Scripts;

namespace Scripts.Services.GameServerProvider
{
    internal class GameServerProviderApi : NetworkApi<GameServerProviderOperations, EmptyEventCode>, IGameServerProviderApi
    {
        internal GameServerProviderApi(IServerPeer serverPeer, bool log = false)
            : base(serverPeer, log)
        {
            // Left blank intentionally
        }

        public async Task<GameServersProviderResponseParameters> ProvideGameServersAsync(
            IYield yield)
        {
            var id =
                OperationRequestSender.Send(
                    GameServerProviderOperations.ProvideGameServers,
                    new EmptyParameters(),
                    MessageSendOptions.DefaultReliable());

            return
                await SubscriptionProvider
                    .ProvideSubscription<GameServersProviderResponseParameters>(
                        yield,
                        id);
        }
    }
}
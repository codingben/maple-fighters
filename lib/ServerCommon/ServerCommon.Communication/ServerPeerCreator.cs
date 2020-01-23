using System;
using System.Threading.Tasks;
using Common.ComponentModel;
using CommonCommunicationInterfaces;
using CommonTools.Coroutines;
using CommonTools.Log;
using ServerCommunicationInterfaces;

namespace ServerCommon.Communication
{
    public class ServerPeerCreator : ComponentBase, IServerPeerCreator
    {
        private readonly Action<IOutboundServerPeer> onServerPeerCreated;
        private IServerConnectorProvider serverConnectorProvider;

        public ServerPeerCreator(Action<IOutboundServerPeer> onServerPeerCreated)
        {
            this.onServerPeerCreated = onServerPeerCreated;
        }

        protected override void OnAwake()
        {
            base.OnAwake();

            serverConnectorProvider = Components.Get<IServerConnectorProvider>();
        }

        public async Task ConnectAsync(IYield yield, string ip, int port)
        {
            try
            {
                var serverConnector = serverConnectorProvider.Provide();
                var serverConnectionInfo = new PeerConnectionInformation(ip, port);
                var serverPeer = await serverConnector.Connect(yield, serverConnectionInfo);
                if (serverPeer != null)
                {
                    onServerPeerCreated?.Invoke(serverPeer);
                }
            }
            catch (Exception exception)
            {
                LogUtils.Log(exception.Message);
            }
        }
    }
}
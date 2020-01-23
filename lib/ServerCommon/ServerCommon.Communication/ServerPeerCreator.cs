using System;
using System.Threading.Tasks;
using Common.ComponentModel;
using CommonCommunicationInterfaces;
using CommonTools.Coroutines;
using CommonTools.Log;
using ServerCommunicationInterfaces;

namespace ServerCommon.Communication
{
    public class ServerPeerCreator : ComponentBase
    {
        private readonly Action<IOutboundServerPeer> onServerPeerCreated;

        private IServerConnectorProvider serverConnectorProvider;
        private ICoroutinesExecuter coroutinesExecuter;

        public ServerPeerCreator(Action<IOutboundServerPeer> onServerPeerCreated)
        {
            this.onServerPeerCreated = onServerPeerCreated;
        }

        protected override void OnAwake()
        {
            base.OnAwake();

            serverConnectorProvider = Components.Get<IServerConnectorProvider>();
            coroutinesExecuter = Components.Get<ICoroutinesExecuter>();
        }

        protected override void OnRemoved()
        {
            base.OnRemoved();

            coroutinesExecuter.Dispose();
        }

        public void Connect(string ip, int port)
        {
            coroutinesExecuter.StartTask((y) => ConnectAsync(y, ip, port));
        }

        public async Task ConnectAsync(IYield yield, string ip, int port)
        {
            try
            {
                var serverConnector = serverConnectorProvider.Provide();
                var peer = new PeerConnectionInformation(ip, port);
                var serverPeer = await serverConnector.Connect(yield, peer);
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
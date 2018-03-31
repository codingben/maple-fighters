using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CommonCommunicationInterfaces;
using CommonTools.Coroutines;
using CommonTools.Log;
using ServerApplication.Common.Components;
using ServerCommunicationInterfaces;

namespace ServerCommunication.Common
{
    internal class ServiceConnectorProvider : IServiceConnectorProvider
    {
        public IPeerDisconnectionNotifier PeerDisconnectionNotifier => outboundServerPeer?.PeerDisconnectionNotifier;

        private IOutboundServerPeer outboundServerPeer;
        private readonly ICoroutinesExecuter coroutinesExecutor;
        private readonly IServerConnectorProvider serverConnectorProvider;
        private readonly Action<IOutboundServerPeer> onConnected;

        public ServiceConnectorProvider(ICoroutinesExecuter coroutinesExecutor, IServerConnectorProvider serverConnectorProvider, Action<IOutboundServerPeer> onConnected)
        {
            this.coroutinesExecutor = coroutinesExecutor;
            this.serverConnectorProvider = serverConnectorProvider;
            this.onConnected = onConnected;
        }

        public void Connect(PeerConnectionInformation connectionInformation)
        {
            coroutinesExecutor.StartCoroutine(ConnectContinuously(connectionInformation));
        }

        private IEnumerator<IYieldInstruction> ConnectContinuously(PeerConnectionInformation connectionInformation)
        {
            const int DELAY_TIME = 10;

            outboundServerPeer = null;

            while (true)
            {
                if (IsConnected())
                {
                    yield break;
                }

                coroutinesExecutor.StartTask((yield) => Connect(yield, connectionInformation));
                yield return new WaitForSeconds(DELAY_TIME);
            }
        }

        private async Task Connect(IYield yield, PeerConnectionInformation connectionInformation)
        {
            try
            {
                outboundServerPeer = await serverConnectorProvider.GetServerConnector().Connect(yield, connectionInformation);
            }
            catch (CouldNotConnectToPeerException exception)
            {
                if (exception.Message != string.Empty)
                {
                    LogUtils.Log(MessageBuilder.Trace(exception.Message));
                }
            }
            finally
            {
                if (IsConnected())
                {
                    onConnected?.Invoke(outboundServerPeer);
                }
            }
        }

        public void SetNetworkTrafficState(NetworkTrafficState state)
        {
            outboundServerPeer.NetworkTrafficState = state;
        }

        public void Dispose()
        {
            if (IsConnected())
            {
                Disconnect();
            }
        }

        private void Disconnect()
        {
            outboundServerPeer.Disconnect();
            outboundServerPeer = null;
        }

        public bool IsConnected()
        {
            return outboundServerPeer != null && outboundServerPeer.IsConnected;
        }
    }
}
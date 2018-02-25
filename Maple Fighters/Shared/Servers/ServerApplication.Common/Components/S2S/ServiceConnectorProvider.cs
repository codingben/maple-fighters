using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CommonCommunicationInterfaces;
using CommonTools.Coroutines;
using CommonTools.Log;
using ServerApplication.Common.Components.Coroutines;
using ServerCommunicationInterfaces;

namespace ServerApplication.Common.Components
{
    internal class ServiceConnectorProvider
    {
        public IPeerDisconnectionNotifier PeerDisconnectionNotifier => outboundServerPeer.PeerDisconnectionNotifier;

        private IOutboundServerPeer outboundServerPeer;
        private readonly ICoroutinesExecuter coroutinesExecutor;
        private readonly IServerConnectorProvider serverConnectorProvider;

        public ServiceConnectorProvider(ICoroutinesExecuter coroutinesExecutor, IServerConnectorProvider serverConnectorProvider)
        {
            this.coroutinesExecutor = coroutinesExecutor;
            this.serverConnectorProvider = serverConnectorProvider;
        }

        public void Connect(PeerConnectionInformation connectionInformation, Action<IOutboundServerPeer> onConnected = null)
        {
            coroutinesExecutor.StartCoroutine(ConnectContinuously(connectionInformation, onConnected));
        }

        private IEnumerator<IYieldInstruction> ConnectContinuously(PeerConnectionInformation connectionInformation, Action<IOutboundServerPeer> onConnected)
        {
            const int WAIT_TIME = 30;

            outboundServerPeer = null;

            while (true)
            {
                if (IsConnected())
                {
                    yield break;
                }

                coroutinesExecutor.StartTask((yield) => Connect(yield, connectionInformation, onConnected));
                yield return new WaitForSeconds(WAIT_TIME);
            }
        }

        private async Task Connect(IYield yield, PeerConnectionInformation connectionInformation, Action<IOutboundServerPeer> onConnected = null)
        {
            try
            {
                LogUtils.Log($"An attempt to connect to a server - {connectionInformation.Ip}:{connectionInformation.Port}");

                outboundServerPeer = await serverConnectorProvider.GetServerConnector().Connect(yield, connectionInformation);
            }
            catch (CouldNotConnectToPeerException exception)
            {
                if (exception.Message != string.Empty)
                {
                    LogUtils.Log(MessageBuilder.Trace(exception.Message));
                }

                LogUtils.Log($"Could not connect to a server - {connectionInformation.Ip}:{connectionInformation.Port}");
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

        public void Disconnect()
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
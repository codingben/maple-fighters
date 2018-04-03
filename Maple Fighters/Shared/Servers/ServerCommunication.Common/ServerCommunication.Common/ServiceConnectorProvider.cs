using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CommonCommunicationInterfaces;
using CommonTools.Coroutines;
using CommonTools.Log;
using ServerApplication.Common.Components;
using ServerApplication.Common.Components.Interfaces;
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

        private bool isConnecting;
        private string exceptionMessage;

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
                yield return new WaitForSeconds(DELAY_TIME);

                if (IsConnected())
                {
                    yield break;
                }

                if (!isConnecting)
                {
                    coroutinesExecutor.StartTask((yield) => Connect(yield, connectionInformation));
                }
            }
        }

        private async Task Connect(IYield yield, PeerConnectionInformation connectionInformation)
        {
            isConnecting = true;

            try
            {
                outboundServerPeer = await serverConnectorProvider.GetServerConnector().Connect(yield, connectionInformation);
            }
            catch (CouldNotConnectToPeerException exception)
            {
                if (exception.Message != string.Empty && !exception.Message.Equals(exceptionMessage))
                {
                    LogUtils.Log($"Failed connect to {connectionInformation.Ip}:{connectionInformation.Port}. Details: {exception.Message}");
                    exceptionMessage = exception.Message;
                }
            }
            finally
            {
                if (IsConnected())
                {
                    onConnected?.Invoke(outboundServerPeer);
                }
            }

            isConnecting = false;
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
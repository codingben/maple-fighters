using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CommonCommunicationInterfaces;
using CommonTools.Coroutines;
using CommonTools.Log;
using Components.Common.Interfaces;
using ServerApplication.Common.Components.Interfaces;
using ServerCommunicationInterfaces;

namespace ServerCommunication.Common
{
    internal class ServiceConnectorProvider : IServiceConnectorProvider
    {
        public IPeerDisconnectionNotifier PeerDisconnectionNotifier => outboundServerPeer?.PeerDisconnectionNotifier;

        private IOutboundServerPeer outboundServerPeer;
        private readonly ICoroutinesManager coroutinesManager;
        private readonly IServerConnectorProvider serverConnectorProvider;
        private readonly Action<IOutboundServerPeer> onConnected;
        private ICoroutine connectContinuously;

        private bool isConnecting;
        private string exceptionMessage;

        public ServiceConnectorProvider(ICoroutinesManager coroutinesManager, IServerConnectorProvider serverConnectorProvider, Action<IOutboundServerPeer> onConnected)
        {
            this.coroutinesManager = coroutinesManager;
            this.serverConnectorProvider = serverConnectorProvider;
            this.onConnected = onConnected;
        }

        public void Connect(PeerConnectionInformation connectionInformation)
        {
            connectContinuously = coroutinesManager.StartCoroutine(ConnectContinuously(connectionInformation));
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
                    coroutinesManager.StartTask((yield) => Connect(yield, connectionInformation));
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

            connectContinuously?.Dispose();
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
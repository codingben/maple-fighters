using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Common.ComponentModel;
using CommonCommunicationInterfaces;
using CommonTools.Coroutines;
using CommonTools.Log;
using ServerCommunicationInterfaces;

namespace ServerCommon.Communication.Components
{
    [ComponentSettings(ExposedState.Unexposable)]
    public class S2sConnectionProvider : ComponentBase, IS2sConnectionProvider
    {
        private readonly IServerConnector serverConnector;
        private ICoroutinesExecutor coroutinesExecutor;
        private IOutboundServerPeer outboundServerPeer;
        private ICoroutine connectContinuously;

        private Action<IOutboundServerPeer> onConnected;
        private Action<DisconnectReason, string> onDisconnected;

        private ConnectionState connectionState = ConnectionState.Disconnected;

        public S2sConnectionProvider(IServerConnector serverConnector)
        {
            this.serverConnector = serverConnector;
        }

        protected override void OnAwake()
        {
            base.OnAwake();

            coroutinesExecutor =
                Components.Get<ICoroutinesExecutor>().AssertNotNull();
        }

        protected override void OnRemoved()
        {
            base.OnRemoved();

            coroutinesExecutor?.Dispose();
        }

        public void Connect(
            PeerConnectionInformation connectionDetails,
            Action<IOutboundServerPeer> connected,
            Action<DisconnectReason, string> disconnected)
        {
            onConnected = connected;
            onDisconnected = disconnected;

            connectContinuously =
                coroutinesExecutor.StartCoroutine(
                    ConnectContinuously(connectionDetails));
        }

        private IEnumerator<IYieldInstruction> ConnectContinuously(
            PeerConnectionInformation connectionDetails)
        {
            const int DELAY_TIME = 10;

            outboundServerPeer = null;

            while (true)
            {
                yield return new WaitForSeconds(DELAY_TIME);

                if (connectionState == ConnectionState.Disconnected)
                {
                    coroutinesExecutor.StartTask(
                        yield => ConnectAsync(yield, connectionDetails));
                }
            }
        }

        private async Task ConnectAsync(
            IYield yield,
            PeerConnectionInformation connectionDetails)
        {
            connectionState = ConnectionState.Connecting;

            try
            {
                outboundServerPeer =
                    await serverConnector.Connect(yield, connectionDetails);

                connectionState =
                    IsConnected()
                        ? ConnectionState.Connected
                        : ConnectionState.Disconnected;
            }
            catch (CouldNotConnectToPeerException exception)
            {
                LogUtils.Log(
                    $"Failed connect to {connectionDetails.Ip}:{connectionDetails.Port}. Reason: {exception.Message}");
            }
            finally
            {
                if (IsConnected())
                {
                    SubscribeToDisconnectionNotifier();

                    connectContinuously?.Dispose();
                    onConnected?.Invoke(outboundServerPeer);

                    outboundServerPeer.NetworkTrafficState =
                        NetworkTrafficState.Flowing;
                }
            }
        }

        public void Dispose()
        {
            connectContinuously?.Dispose();
        }

        public void Disconnect()
        {
            if (IsConnected())
            {
                UnsubscribeFromDisconnectionNotifier();

                outboundServerPeer.Disconnect();
                outboundServerPeer = null;
            }
        }

        private void SubscribeToDisconnectionNotifier()
        {
            outboundServerPeer.PeerDisconnectionNotifier.Disconnected +=
                onDisconnected;
        }

        private void UnsubscribeFromDisconnectionNotifier()
        {
            outboundServerPeer.PeerDisconnectionNotifier.Disconnected -=
                onDisconnected;
        }

        private bool IsConnected()
        {
            return outboundServerPeer != null && outboundServerPeer.IsConnected;
        }
    }
}
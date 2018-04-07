using System.Threading.Tasks;
using CommonCommunicationInterfaces;
using CommonTools.Coroutines;
using CommonTools.Log;
using ComponentModel.Common;
using Components.Common.Interfaces;
using JsonConfig;
using PeerLogic.Common.Components.Interfaces;

namespace PeerLogic.Common.Components
{
    public class InactivityTimeout : Component
    {
        private IClientPeerProvider clientPeerProvider;
        private ICoroutine disconnectInactivityTask;

        private readonly WaitForSeconds waitForSeconds;
        private readonly bool lookForOperationsRequest;

        public InactivityTimeout()
        {
            LogUtils.Assert(Config.Global.InactivityTimeout, MessageBuilder.Trace("Could not find a configuration for the inactivity timeout."));

            lookForOperationsRequest = (bool)Config.Global.InactivityTimeout.LookForOperationRequests;

            var time = (int)Config.Global.InactivityTimeout.Time;
            waitForSeconds = new WaitForSeconds(time);
        }

        protected override void OnAwake()
        {
            base.OnAwake();

            clientPeerProvider = Components.GetComponent<IClientPeerProvider>().AssertNotNull();

            if (lookForOperationsRequest)
            {
                SubscribeToOperationRequested();
            }

            var coroutinesManager = Components.GetComponent<ICoroutinesManager>().AssertNotNull();
            disconnectInactivityTask = coroutinesManager.StartTask(DisconnectInactivity);
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();

            disconnectInactivityTask.Dispose();

            if (lookForOperationsRequest)
            {
                UnsubscribeFromOperationRequested();
            }
        }

        private void SubscribeToOperationRequested()
        {
            clientPeerProvider.Peer.OperationRequestNotifier.OperationRequested += OnOperationRequestReceived;
        }

        private void UnsubscribeFromOperationRequested()
        {
            clientPeerProvider.Peer.OperationRequestNotifier.OperationRequested -= OnOperationRequestReceived;
        }

        private void OnOperationRequestReceived(RawMessageData rawMessageData, short code, MessageSendOptions messageSendOptions)
        {
            waitForSeconds.Reset();
        }

        private async Task DisconnectInactivity(IYield yield)
        {
            await yield.Return(waitForSeconds);
            Disconnect();
        }

        private void Disconnect()
        {
            var ip = clientPeerProvider.Peer.ConnectionInformation.Ip;
            var port = clientPeerProvider.Peer.ConnectionInformation.Port;

            LogUtils.Log(MessageBuilder.Trace($"Disconnecting a peer {ip}:{port} due to inactivity timeout."));

            clientPeerProvider.Peer.Disconnect();
        }
    }
}
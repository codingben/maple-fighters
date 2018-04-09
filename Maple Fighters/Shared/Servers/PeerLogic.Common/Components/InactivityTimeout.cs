using System.Threading.Tasks;
using CommonCommunicationInterfaces;
using CommonTools.Coroutines;
using CommonTools.Log;
using ComponentModel.Common;
using Components.Common.Interfaces;
using PeerLogic.Common.Components.Interfaces;

namespace PeerLogic.Common.Components
{
    public class InactivityTimeout : Component
    {
        private IClientPeerProvider clientPeerProvider;
        private ICoroutine disconnectInactivityTask;

        private readonly WaitForSeconds waitForSeconds;
        private readonly bool lookForOperations;

        public InactivityTimeout(int seconds, bool lookForOperations)
        {
            this.lookForOperations = lookForOperations;
            waitForSeconds = new WaitForSeconds(seconds);
        }

        protected override void OnAwake()
        {
            base.OnAwake();

            clientPeerProvider = Components.GetComponent<IClientPeerProvider>().AssertNotNull();

            if (lookForOperations)
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

            if (lookForOperations)
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
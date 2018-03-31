using System.Threading.Tasks;
using CommonCommunicationInterfaces;
using CommonTools.Coroutines;
using CommonTools.Log;
using ComponentModel.Common;
using JsonConfig;

namespace PeerLogic.Common.Components
{
    public class InactivityTimeout : Component
    {
        private IClientPeerGetter peerGetter;
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

            peerGetter = Components.GetComponent<IClientPeerGetter>().AssertNotNull();

            if (lookForOperationsRequest)
            {
                SubscribeToOperationRequested();
            }

            var coroutinesExecutor = Components.GetComponent<ICoroutinesExecutor>().AssertNotNull();
            disconnectInactivityTask = coroutinesExecutor.StartTask(DisconnectInactivity);
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
            peerGetter.Peer.OperationRequestNotifier.OperationRequested += OnOperationRequestReceived;
        }

        private void UnsubscribeFromOperationRequested()
        {
            peerGetter.Peer.OperationRequestNotifier.OperationRequested -= OnOperationRequestReceived;
        }

        private void OnOperationRequestReceived(RawMessageData rawMessageData, short code, MessageSendOptions messageSendOptions)
        {
            waitForSeconds.Reset();
        }

        private async Task DisconnectInactivity(IYield yield)
        {
            await yield.Return(waitForSeconds);
            peerGetter.Peer.Disconnect();
        }
    }
}
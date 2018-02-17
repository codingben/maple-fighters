using CommonTools.Log;
using PeerLogic.Common;
using PeerLogic.Common.Components;
using Server2.Common;
using ServerCommunicationInterfaces;

namespace Server2
{
    internal class ServerPeerLogic : PeerLogicBase<ServerOperations, ServerEvents>
    {
        public override void Initialize(IClientPeerWrapper<IClientPeer> peer)
        {
            base.Initialize(peer);

            AddCommonComponents();
            AddHandlerForTestOperation();
        }

        private void AddHandlerForTestOperation()
        {
            var eventSender = Components.GetComponent<IEventSenderWrapper>().AssertNotNull();
            OperationRequestHandlerRegister.SetHandler(ServerOperations.Server1Operation, new Server1OperationHandler(eventSender));
        }
    }
}
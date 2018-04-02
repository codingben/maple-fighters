using CommonTools.Log;
using PeerLogic.Common;
using PeerLogic.Common.Components.Interfaces;
using Server2.Common;

namespace Server2
{
    internal class ServerPeerLogic : PeerLogicBase<ServerOperations, ServerEvents>
    {
        public override void Initialize(IClientPeerWrapper peer)
        {
            base.Initialize(peer);

            AddCommonComponents();
            AddHandlerForTestOperation();
        }

        private void AddHandlerForTestOperation()
        {
            var eventSender = Components.GetComponent<IEventSenderWrapper>().AssertNotNull();
            OperationHandlerRegister.SetHandler(ServerOperations.Server1Operation, new Server1OperationHandler(eventSender));
        }
    }
}
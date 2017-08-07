using CommonCommunicationInterfaces;
using ServerCommunicationHelper;
using ServerCommunicationInterfaces;
using Shared.Game.Common;

namespace Game.Application.PeerLogic.Operations
{
    internal class TestOperation : IOperationRequestHandler<TestRequestParameters, TestResponseParameters>
    {
        private readonly IEventSender eventSender;

        public TestOperation(IEventSender eventSender)
        {
            this.eventSender = eventSender;
        }

        public TestResponseParameters? Handle(MessageData<TestRequestParameters> messageData, ref MessageSendOptions sendOptions)
        {
            eventSender.Send(new MessageData<TestParameters>((byte)GameEvents.Test, new TestParameters(10)), MessageSendOptions.DefaultReliable());

            return new TestResponseParameters(10);
        }
    }
}
using CommonCommunicationInterfaces;
using CommonTools.Log;
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
            LogUtils.Log($"TestOperation::Handle() -> {messageData.Parameters.Number}");

            eventSender.Send(new MessageData<TestParameters>((byte)GameEvents.Test, new TestParameters(15)), MessageSendOptions.DefaultReliable());

            return new TestResponseParameters(10);
        }
    }
}
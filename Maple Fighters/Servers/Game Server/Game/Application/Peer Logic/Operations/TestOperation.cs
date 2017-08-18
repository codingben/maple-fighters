using CommonCommunicationInterfaces;
using CommonTools.Log;
using ServerApplication.Common.ComponentModel;
using ServerApplication.Common.Components;
using ServerCommunicationHelper;
using ServerCommunicationInterfaces;
using Shared.Game.Common;

namespace Game.Application.PeerLogic.Operations
{
    internal class TestOperation : IOperationRequestHandler<TestRequestParameters, TestResponseParameters>
    {
        private readonly IEventSender eventSender;
        private readonly RandomNumberGenerator randomNumberGenerator;

        public TestOperation(IEventSender eventSender)
        {
            this.eventSender = eventSender;

            randomNumberGenerator = ServerComponents.Container.GetComponent<RandomNumberGenerator>().AssertNotNull() as RandomNumberGenerator;
        }

        public TestResponseParameters? Handle(MessageData<TestRequestParameters> messageData, ref MessageSendOptions sendOptions)
        {
            var randomNumber = randomNumberGenerator.GenerateId();

            LogUtils.Log($"TestOperation::Handle() Random Number -> {randomNumber}");

            eventSender.Send(new MessageData<TestParameters>((byte)GameEvents.Test, new TestParameters(15)), MessageSendOptions.DefaultReliable());

            return new TestResponseParameters(10);
        }
    }
}
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

        public TestOperation(IEventSender eventSender)
        {
            this.eventSender = eventSender;
        }

        public TestResponseParameters? Handle(MessageData<TestRequestParameters> messageData, ref MessageSendOptions sendOptions)
        {
            ServerComponents.Container.GetComponent(out RandomNumberGenerator idGenerator);

            if (idGenerator != null)
            {
                var randomNumber = idGenerator.GenerateId();

                LogUtils.Log($"TestOperation::Handle() Random Number -> {randomNumber}");
            }

            eventSender.Send(new MessageData<TestParameters>((byte)GameEvents.Test, new TestParameters(15)), MessageSendOptions.DefaultReliable());

            return new TestResponseParameters(10);
        }
    }
}
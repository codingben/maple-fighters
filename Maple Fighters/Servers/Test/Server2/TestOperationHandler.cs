using CommonCommunicationInterfaces;
using CommonTools.Log;
using PeerLogic.Common.Components;
using Server2.Common;
using ServerCommunicationHelper;

namespace Server2
{
    internal class TestOperationHandler : IOperationRequestHandler<EmptyParameters, EmptyParameters>
    {
        private readonly IEventSenderWrapper eventSender;

        public TestOperationHandler(IEventSenderWrapper eventSender)
        {
            this.eventSender = eventSender;
        }

        public EmptyParameters? Handle(MessageData<EmptyParameters> messageData, ref MessageSendOptions sendOptions)
        {
            LogUtils.Log(MessageBuilder.Trace("Hello world!"));
            eventSender.Send((byte)ServerEvents.Server1Event, new EmptyParameters(), MessageSendOptions.DefaultReliable());
            return null; // TODO: Implement an operation response
        }
    }
}
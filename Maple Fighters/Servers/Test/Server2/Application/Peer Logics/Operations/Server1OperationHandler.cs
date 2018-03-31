using CommonCommunicationInterfaces;
using CommonTools.Log;
using PeerLogic.Common.Components;
using Server2.Common;
using ServerCommunicationHelper;

namespace Server2
{
    internal class Server1OperationHandler : IOperationRequestHandler<Server1OperationRequestParameters, Server1OperationResponseParameters>
    {
        private readonly IEventSenderWrapper eventSender;

        public Server1OperationHandler(IEventSenderWrapper eventSender)
        {
            this.eventSender = eventSender;
        }

        public Server1OperationResponseParameters? Handle(MessageData<Server1OperationRequestParameters> messageData, ref MessageSendOptions sendOptions)
        {
            LogUtils.Log(MessageBuilder.Trace($"Received number: {messageData.Parameters.Number}"));
            eventSender.Send((byte)ServerEvents.Server1Event, new EmptyParameters(), MessageSendOptions.DefaultReliable());
            return new Server1OperationResponseParameters(100);
        }
    }
}
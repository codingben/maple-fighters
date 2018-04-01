using CommonCommunicationInterfaces;

namespace PeerLogic.Common.Components
{
    public interface IEventSenderWrapper
    {
        void Send<TParameters>(byte code, TParameters parameters, MessageSendOptions messageSendOptions)
            where TParameters : struct, IParameters;
    }
}
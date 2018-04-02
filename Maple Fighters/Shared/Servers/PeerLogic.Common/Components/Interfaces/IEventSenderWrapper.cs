using CommonCommunicationInterfaces;

namespace PeerLogic.Common.Components.Interfaces
{
    public interface IEventSenderWrapper
    {
        void Send<TParameters>(byte code, TParameters parameters, MessageSendOptions messageSendOptions)
            where TParameters : struct, IParameters;
    }
}
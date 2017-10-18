using CommonCommunicationInterfaces;
using ComponentModel.Common;

namespace PeerLogic.Common.Components
{
    public interface IEventSenderWrapper : IExposableComponent
    {
        void Send<TParameters>(byte code, TParameters parameters, MessageSendOptions messageSendOptions)
            where TParameters : struct, IParameters;
    }
}
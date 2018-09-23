using System;
using CommonCommunicationInterfaces;

namespace ServerCommon.PeerLogic.Components
{
    public interface IEventSenderProvider
    {
        void Send<TEnumCode, TParameters>(
            TEnumCode code,
            TParameters parameters,
            MessageSendOptions messageSendOptions)
            where TEnumCode : IComparable, IFormattable, IConvertible
            where TParameters : struct, IParameters;
    }
}
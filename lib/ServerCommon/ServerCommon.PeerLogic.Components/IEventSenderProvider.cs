using System;
using CommonCommunicationInterfaces;

namespace ServerCommon.PeerLogic.Components
{
    public interface IEventSenderProvider
    {
        void Send<TParameters, TEnumCode>(
            TEnumCode code,
            TParameters parameters,
            MessageSendOptions messageSendOptions)
            where TParameters : struct, IParameters
            where TEnumCode : IComparable, IFormattable, IConvertible;
    }
}
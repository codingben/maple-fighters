using System;
using CommonCommunicationInterfaces;

namespace ServerCommon.PeerLogic.Components
{
    public interface IEventSenderProvider
    {
        void Send<TParameters, T>(
            T code, TParameters parameters, MessageSendOptions options)
            where TParameters : struct, IParameters
            where T : IComparable, IFormattable, IConvertible;
    }
}
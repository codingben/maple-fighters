using System;
using System.Threading.Tasks;
using CommonCommunicationInterfaces;
using CommonTools.Coroutines;

namespace ServerCommon.Communication.PeerLogic
{
    public interface IServerPeerLogic<in TOperationCode, in TEventCode> : IDisposable
        where TOperationCode : IComparable, IFormattable, IConvertible
        where TEventCode : IComparable, IFormattable, IConvertible
    {
        void SendOperation<TParameters>(
            TOperationCode code,
            TParameters parameters)
            where TParameters : struct, IParameters;

        Task<TResponseParameters>
            SendOperationAsync<TRequestParameters, TResponseParameters>(
                IYield yield,
                TOperationCode code,
                TRequestParameters parameters)
            where TRequestParameters : struct, IParameters
            where TResponseParameters : struct, IParameters;

        void SetEventHandler<TEventParameters>(
            TEventCode code,
            Action<TEventParameters> action)
            where TEventParameters : struct, IParameters;

        void RemoveEventHandler(TEventCode code);
    }
}
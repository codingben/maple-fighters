using System;
using System.Threading.Tasks;
using CommonCommunicationInterfaces;
using CommonTools.Coroutines;

namespace ServerApplication.Common.Components
{
    public interface IOutboundServerPeerLogic : IDisposable
    {
        void SendOperation<TParams>(byte operationCode, TParams parameters)
            where TParams : struct, IParameters;

        Task<TResponseParams> SendOperation<TRequestParams, TResponseParams>(IYield yield, byte operationCode, TRequestParams parameters)
            where TRequestParams : struct, IParameters
            where TResponseParams : struct, IParameters;

        void SetEventHandler<TParams>(byte eventCode, Action<TParams> action)
            where TParams : struct, IParameters;

        void RemoveEventHandler(byte eventCode);
    }
}
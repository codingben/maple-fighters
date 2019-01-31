using System;
using System.Threading.Tasks;
using CommonCommunicationInterfaces;
using CommonTools.Coroutines;

namespace Scripts.Services
{
    public interface IServerPeerHandler : IDisposable
    {
        void SendOperation<TParams>(
            byte operationCode,
            TParams parameters,
            MessageSendOptions messageSendOptions)
            where TParams : struct, IParameters;

        Task<TResponseParams> SendOperation<TRequestParams, TResponseParams>(
            IYield yield,
            byte operationCode,
            TRequestParams parameters,
            MessageSendOptions messageSendOptions)
            where TRequestParams : struct, IParameters
            where TResponseParams : struct, IParameters;

        void SetEventHandler<TParams>(
            byte eventCode,
            UnityEvent<TParams> action)
            where TParams : struct, IParameters;

        void RemoveEventHandler(byte eventCode);
    }
}
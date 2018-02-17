using System;
using System.Threading.Tasks;
using CommonCommunicationInterfaces;
using CommonTools.Coroutines;
using Scripts.Utils;

namespace Scripts.Services
{
    public interface IServiceBase : IDisposable
    {
        Task<ConnectionStatus> Connect(IYield yield, ICoroutinesExecutor coroutinesExecutor, ConnectionInformation connectionInformation);

        void SetNetworkTrafficState(NetworkTrafficState state);

        void SendOperation<TParams>(byte operationCode, TParams parameters, MessageSendOptions messageSendOptions)
            where TParams : struct, IParameters;

        Task<TResponseParams> SendYieldOperation<TRequestParams, TResponseParams>(IYield yield, byte operationCode, TRequestParams parameters, MessageSendOptions messageSendOptions)
            where TRequestParams : struct, IParameters
            where TResponseParams : struct, IParameters;

        NetworkTrafficState? NetworkTrafficState();
        bool IsConnected();
    }
}
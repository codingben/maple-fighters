using System.Threading.Tasks;
using CommonCommunicationInterfaces;
using CommonTools.Coroutines;
using ComponentModel.Common;

namespace ServerApplication.Common.Components
{
    public interface IServiceBase : IExposableComponent
    {
        void SendOperation<TParams>(byte operationCode, TParams parameters)
            where TParams : struct, IParameters;

        Task<TResponseParams> SendYieldOperation<TRequestParams, TResponseParams>(IYield yield, byte operationCode, TRequestParams parameters)
            where TRequestParams : struct, IParameters
            where TResponseParams : struct, IParameters;

        bool IsConnected();
    }
}
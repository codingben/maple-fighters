using System;
using System.Threading.Tasks;
using CommonCommunicationInterfaces;
using CommonTools.Coroutines;
using ComponentModel.Common;

namespace Server2.Common
{
    public interface IServer2ServiceAPI : IExposableComponent
    {
        Task<Server1OperationResponseParameters> Server1Operation(IYield yield, Server1OperationRequestParameters parameters);

        event Action<EmptyParameters> TestAction;
    }
}
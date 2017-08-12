using System.Threading.Tasks;
using CommonTools.Coroutines;
using Shared.Game.Common;

namespace Scripts.Services
{
    public interface IGameService
    {
        Task TestOperationRequestAsync(Yield yield, TestRequestParameters requestParameters);

        UnityEvent<TestParameters> TestEvent { get; }
    }
}
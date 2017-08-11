using Shared.Game.Common;
using UnityEngine.Events;

namespace Scripts.Services
{
    public interface IGameService
    {
        void TestOperationRequest(TestRequestParameters parameters);

        UnityEvent<TestParameters> TestEvent { get; }
    }
}
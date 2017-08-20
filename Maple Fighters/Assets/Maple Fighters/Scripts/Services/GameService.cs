using System.Threading.Tasks;
using CommonCommunicationInterfaces;
using CommonTools.Coroutines;
using Shared.Game.Common;
using CommunicationHelper;
using UnityEngine;

namespace Scripts.Services
{
    public sealed class GameService : ServiceBase<GameOperations, GameEvents>, IGameService
    {
        public UnityEvent<TestParameters> TestEvent { get; }

        public GameService()
        {
            TestEvent = new UnityEvent<TestParameters>();
        }

        protected override void Initiate()
        {
            Connect();
        }

        protected override void OnConnected()
        {
            AddEventsHandlers();

            CoroutinesExecuter.StartTask(y => TestOperationRequestAsync(y, new TestRequestParameters(5)));
        }

        protected override void OnDisconnected()
        {
            RemoveEventsHandlers();
        }

        private void AddEventsHandlers()
        {
            EventHandlerRegister.SetHandler(GameEvents.Test, new EventInvoker<TestParameters>(unityEvent =>
            {
                TestEvent.Invoke(unityEvent.Parameters);
                return true;
            }));
        }

        private void RemoveEventsHandlers()
        {
            EventHandlerRegister.RemoveHandler(GameEvents.Test);
        }

        public async Task TestOperationRequestAsync(Yield yield, TestRequestParameters requestParameters)
        {
            if (!IsConnected())
            {
                Debug.LogWarning("GameService::TestOperationRequestAsync() -> Peer is not connected to a server.");
                return;
            }

            var requestId = OperationRequestSender.Send(GameOperations.Test, requestParameters, MessageSendOptions.DefaultReliable());
            var responseParameters = await SubscriptionProvider.ProvideSubscription<TestResponseParameters>(yield, requestId);

            Debug.Log(responseParameters.Number);
        }
    }
}
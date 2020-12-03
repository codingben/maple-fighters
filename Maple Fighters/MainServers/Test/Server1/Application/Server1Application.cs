using System.Collections.Generic;
using CommonCommunicationInterfaces;
using CommonTools.Coroutines;
using CommonTools.Log;
using Components.Common.Interfaces;
using Server2.Common;
using ServerApplication.Common.ApplicationBase;
using ServerCommunicationInterfaces;

namespace Server1
{
    public class Server1Application : ApplicationBase
    {
        public Server1Application(IFiberProvider fiberProvider, IServerConnector serverConnector) 
            : base(fiberProvider, serverConnector)
        {
            // Left blank intentionally
        }

        public override void Startup()
        {
            base.Startup();

            AddCommonComponents();

            ServerComponents.AddComponent(new Server2Service());

            RunTestForServer2Service();
        }

        private void RunTestForServer2Service()
        {
            var server2Service = ServerComponents.GetComponent<IServer2ServiceAPI>().AssertNotNull();
            server2Service.TestAction += OnTestEvent;

            var coroutinesExecutor = ServerComponents.GetComponent<ICoroutinesManager>().AssertNotNull();
            coroutinesExecutor.StartCoroutine(Server2Service());
        }

        private IEnumerator<IYieldInstruction> Server2Service()
        {
            yield return new WaitForSeconds(15);

            var server2Service = ServerComponents.GetComponent<IServer2ServiceAPI>().AssertNotNull();
            var coroutinesExecutor = ServerComponents.GetComponent<ICoroutinesManager>().AssertNotNull();
            coroutinesExecutor.StartTask(async (y) =>
            {
                var parameters = new Server1OperationRequestParameters(50);
                var responseParams = await server2Service.Server1Operation(y, parameters);
                LogUtils.Log(MessageBuilder.Trace($"Received number: {responseParams.Number}"));
            });
        }

        private void OnTestEvent(EmptyParameters emptyParameters)
        {
            LogUtils.Log(MessageBuilder.Trace("Hello world!"));
        }
    }
}
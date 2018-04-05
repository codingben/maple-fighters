using System;
using System.Threading.Tasks;
using CommonTools.Coroutines;
using CommonTools.Log;
using CommunicationHelper;
using ServerApplication.Common.ApplicationBase;
using ServerApplication.Common.Components;
using ServerCommunicationInterfaces;

namespace ServerCommunication.Common
{
    public class ServerAuthenticationPeerLogic : OutboundServerPeerLogicBase<AuthenticationOperations, EmptyEventCode>
    {
        private readonly string secretKey;
        private readonly Action onAuthenticated;
        private ICoroutine authenticationTask;
        private bool isAuthenticated;

        public ServerAuthenticationPeerLogic(IOutboundServerPeer outboundServerPeer, string secretKey, Action onAuthenticated) 
            : base(outboundServerPeer)
        {
            this.secretKey = secretKey.AssertNotNull(MessageBuilder.Trace("Secret key not found."));
            this.onAuthenticated = onAuthenticated;
        }

        protected override void OnInitialized()
        {
            base.OnInitialized();

            Authenticate();
        }

        protected override void OnDispose()
        {
            base.OnDispose();

            if (!isAuthenticated)
            {
                authenticationTask?.Dispose();
            }
        }

        private void Authenticate()
        {
            var coroutinesExecutor = Server.Components.GetComponent<ICoroutinesExecuter>().AssertNotNull();
            authenticationTask = coroutinesExecutor.StartTask(Authenticate);
        }

        private async Task Authenticate(IYield yield)
        {
            var parameters = new AuthenticateRequestParameters(secretKey);
            var responseParameters = await SendOperation<AuthenticateRequestParameters, AuthenticateResponseParameters>
                (yield, (byte)AuthenticationOperations.Authenticate, parameters);
            if (responseParameters.Status == AuthenticationStatus.Succeed)
            {
                isAuthenticated = true;
                onAuthenticated.Invoke();
            }
        }
    }
}
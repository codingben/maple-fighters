using System.Collections.Generic;
using System.Threading.Tasks;
using Authorization.Client.Common;
using CommonCommunicationInterfaces;
using CommonTools.Coroutines;
using Scripts.Utils;
using WaitForSeconds = CommonTools.Coroutines.WaitForSeconds;

namespace Scripts.Services
{
    public abstract class ServiceConnectionProvider<T> : DontDestroyOnLoad<T>
        where T : ServiceConnectionProvider<T>
    {
        protected readonly ExternalCoroutinesExecutor CoroutinesExecutor = new ExternalCoroutinesExecutor();

        private IServiceBase serviceBase;
        private ICoroutine disconnectAutomatically;

        private void Update()
        {
            CoroutinesExecutor.Update();
        }

        private void OnDestroy()
        {
            CoroutinesExecutor.Dispose();
        }

        protected async Task Connect(IYield yield, IServiceBase serviceBase, ConnectionInformation connectionInformation)
        {
            this.serviceBase = serviceBase;

            OnPreConnection();

            var connectionStatus = await serviceBase.Connect(yield, CoroutinesExecutor, connectionInformation);
            if (connectionStatus == ConnectionStatus.Failed)
            {
                OnConnectionFailed();
                return;
            }

            OnConnectionEstablished();
        }

        protected abstract void OnPreConnection();
        protected abstract void OnConnectionFailed();
        protected abstract void OnConnectionEstablished();

        protected async Task Authorize(IYield yield, byte operationCode)
        {
            OnPreAuthorization();

            var parameters = new AuthorizeRequestParameters(AccessTokenProvider.AccessToken);
            var authorizationStatus = await serviceBase.SendOperation<AuthorizeRequestParameters, AuthorizeResponseParameters>
                    (yield, operationCode, parameters, MessageSendOptions.DefaultReliable());
            if (authorizationStatus.Status == AuthorizationStatus.Failed)
            {
                OnNonAuthorized();
                return;
            }

            OnAuthorized();
        }

        protected abstract void OnPreAuthorization();
        protected abstract void OnNonAuthorized();
        protected abstract void OnAuthorized();

        public void DisconnectAutomatically()
        {
            if (disconnectAutomatically == null)
            {
                disconnectAutomatically = CoroutinesExecutor.StartCoroutine(DisconnectAutomaticallyTimer());
            }
        }

        protected IEnumerator<IYieldInstruction> DisconnectAutomaticallyTimer()
        {
            const int AUTO_TIME_FOR_DISCONNECT = 60;
            yield return new WaitForSeconds(AUTO_TIME_FOR_DISCONNECT);
            Disconnect();
        }

        public void Disconnect()
        {
            disconnectAutomatically?.Dispose();
            disconnectAutomatically = null;

            serviceBase?.Dispose();
        }

        private void OnApplicationQuit()
        {
            Disconnect();
        }

        public bool IsConnected()
        {
            return serviceBase != null && serviceBase.IsConnected();
        }
    }
}
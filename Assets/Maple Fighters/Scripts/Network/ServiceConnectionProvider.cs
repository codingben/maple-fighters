using System.Collections.Generic;
using System.Threading.Tasks;
using Authorization.Client.Common;
using CommonTools.Coroutines;
using Scripts.ScriptableObjects;
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

        protected async Task Connect(IYield yield, IServiceBase serviceBase, ServerConnectionInformation serverConnectionInformation)
        {
            this.serviceBase = serviceBase;

            OnPreConnection();

            var connectionStatus = await serviceBase.ServiceConnectionHandler.Connect(yield, CoroutinesExecutor, serverConnectionInformation);
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

        protected async Task Authorize(IYield yield)
        {
            OnPreAuthorization();

            var parameters = new AuthorizeRequestParameters(AccessTokenProvider.AccessToken);
            var authorizationStatus = await Authorize(yield, parameters);
            if (authorizationStatus.Status == AuthorizationStatus.Failed)
            {
                OnNonAuthorized();
                return;
            }

            OnAuthorized();
        }

        protected abstract Task<AuthorizeResponseParameters> Authorize(IYield yield, AuthorizeRequestParameters parameters);

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
            return serviceBase != null && serviceBase.ServiceConnectionHandler.IsConnected();
        }

        protected ServerConnectionInformation GetServerConnectionInformation(ServerType serverType)
        {
            var connectionInformation = ServicesConfiguration.GetInstance().GetConnectionInformation(serverType);
            var peerConnectionInformation = NetworkConfiguration.GetInstance().GetPeerConnectionInformation(connectionInformation);
            return new ServerConnectionInformation(serverType, peerConnectionInformation);
        }
    }
}
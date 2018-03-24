using System.Collections.Generic;
using System.Threading.Tasks;
using Authorization.Client.Common;
using CommonCommunicationInterfaces;
using CommonTools.Coroutines;
using CommonTools.Log;
using Scripts.ScriptableObjects;
using Scripts.Utils;
using WaitForSeconds = CommonTools.Coroutines.WaitForSeconds;

namespace Scripts.Services
{
    public abstract class ServiceConnectionProviderBase<T> : DontDestroyOnLoad<T>
        where T : ServiceConnectionProviderBase<T>
    {
        protected ExternalCoroutinesExecutor CoroutinesExecutor
        {
            get
            {
                if (coroutinesExecutor == null)
                {
                    coroutinesExecutor = new ExternalCoroutinesExecutor();
                }

                return coroutinesExecutor;
            }
        }
        private ExternalCoroutinesExecutor coroutinesExecutor = new ExternalCoroutinesExecutor();

        private IServiceBase serviceBase => GetServiceBase();
        private ICoroutine disconnectAutomatically;

        private void Update()
        {
            CoroutinesExecutor?.Update();
        }

        private void OnDestroy()
        {
            CoroutinesExecutor?.Dispose();
        }

        protected async Task Connect(IYield yield, ServerConnectionInformation serverConnectionInformation)
        {
            if (serviceBase == null || CoroutinesExecutor == null)
            {
                LogUtils.Log("A service base is not initialized.");
                return;
            }

            OnPreConnection();

            var connectionStatus = ConnectionStatus.Failed;

            try
            {
                connectionStatus = await serviceBase.ServiceConnectionHandler.Connect(yield, CoroutinesExecutor, serverConnectionInformation);
            }
            catch (ServerConnectionFailed exception)
            {
                LogUtils.Log(exception.Message, LogMessageType.Error);
            }

            if (connectionStatus == ConnectionStatus.Failed)
            {
                OnConnectionFailed();
                return;
            }

            SubscribeToDisconnectionNotifier();
            OnConnectionEstablished();
        }

        protected abstract void OnPreConnection();
        protected abstract void OnConnectionFailed();
        protected abstract void OnConnectionEstablished();

        protected virtual void OnDisconnected(DisconnectReason reason, string details)
        {
            UnsubscribeFromDisconnectionNotifier();
        }

        private void SubscribeToDisconnectionNotifier()
        {
            serviceBase.ServiceConnectionHandler.ConnectionNotifier.Disconnected += OnDisconnected;
        }

        private void UnsubscribeFromDisconnectionNotifier()
        {
            serviceBase.ServiceConnectionHandler.ConnectionNotifier.Disconnected -= OnDisconnected;
        }

        protected async Task Authorize(IYield yield)
        {
            OnPreAuthorization();

            var parameters = new AuthorizeRequestParameters(AccessTokenProvider.AccessToken);
            var authorizationStatus = await Authorize(yield, parameters);
            if (authorizationStatus.Status == AuthorizationStatus.Failed)
            {
                Dispose();
                return;
            }

            OnAuthorized();
        }

        protected abstract Task<AuthorizeResponseParameters> Authorize(IYield yield, AuthorizeRequestParameters parameters);

        protected abstract void OnPreAuthorization();
        protected abstract void OnAuthorized();

        protected abstract IServiceBase GetServiceBase();

        protected void DisconnectAutomatically(int timer)
        {
            if (disconnectAutomatically == null)
            {
                disconnectAutomatically = CoroutinesExecutor.StartCoroutine(DisconnectAutomaticallyTimer(timer));
            }
        }

        protected IEnumerator<IYieldInstruction> DisconnectAutomaticallyTimer(int timer)
        {
            yield return new WaitForSeconds(timer);
            Dispose();
        }

        private void OnApplicationQuit()
        {
            Dispose();
        }

        public void Dispose()
        {
            CoroutinesExecutor?.Dispose();

            disconnectAutomatically?.Dispose();
            disconnectAutomatically = null;

            serviceBase.ServiceConnectionHandler?.Dispose();
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

        protected ServerConnectionInformation GetServerConnectionInformation(ServerType serverType, PeerConnectionInformation peerConnectionInformation)
        {
            return new ServerConnectionInformation(serverType, peerConnectionInformation);
        }
    }
}
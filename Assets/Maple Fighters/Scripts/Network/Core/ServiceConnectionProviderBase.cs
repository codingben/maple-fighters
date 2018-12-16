using System;
using System.Threading.Tasks;
using Authorization.Client.Common;
using CommonCommunicationInterfaces;
using CommonTools.Coroutines;
using CommonTools.Log;
using CommunicationHelper;
using Scripts.ScriptableObjects;
using Scripts.Utils;

namespace Scripts.Services
{
    public abstract class ServiceConnectionProviderBase<T> : MonoSingleton<T>, IServiceConnectionProviderBase
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

        private ExternalCoroutinesExecutor coroutinesExecutor;
        private IServiceBase serviceBase;

        protected bool IsDestroying { get; private set; }

        private void Update()
        {
            CoroutinesExecutor?.Update();
        }

        protected override void OnDestroying()
        {
            base.OnDestroying();

            IsDestroying = true;

            CoroutinesExecutor?.Dispose();
        }

        protected override void OnApplicationQuitting()
        {
            base.OnApplicationQuitting();

            Dispose();
        }

        protected async Task Connect(IYield yield, ServerConnectionInformation serverConnectionInformation, bool authorize = true)
        {
            serviceBase = GetServiceBase();

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

            if (authorize)
            {
                serviceBase.SetPeerLogic<AuthorizationPeerLogic, AuthorizationOperations, EmptyEventCode>();
            }
            else
            {
                SetPeerLogicAfterAuthorization();
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
            Dispose();
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

            var authorizationStatus = AuthorizationStatus.Failed;

            try
            {
                var parameters = new AuthorizeRequestParameters(AccessTokenProvider.AccessToken);
                var responseParameters = await Authorize(yield, parameters);
                authorizationStatus = responseParameters.Status;
            }
            catch (Exception)
            {
                // Left blank intentionally
            }

            if (authorizationStatus == AuthorizationStatus.Failed)
            {
                OnNonAuthorized();
                return;
            }

            SetPeerLogicAfterAuthorization();
            OnAuthorized();
        }

        protected abstract Task<AuthorizeResponseParameters> Authorize(IYield yield, AuthorizeRequestParameters parameters);

        protected abstract void OnPreAuthorization();

        protected abstract void OnNonAuthorized();

        protected abstract void OnAuthorized();

        protected abstract void SetPeerLogicAfterAuthorization();

        protected abstract IServiceBase GetServiceBase();

        public void Dispose()
        {
            if (serviceBase != null)
            {
                serviceBase.Dispose();
                serviceBase = null;
            }
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
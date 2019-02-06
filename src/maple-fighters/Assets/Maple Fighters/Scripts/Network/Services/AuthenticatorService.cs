using Scripts.Network.APIs;
using Scripts.Network.Core;
using Scripts.ScriptableObjects;
using UnityEngine;

namespace Scripts.Network
{
    public class AuthenticatorService : ServiceBase, IAuthenticatorService
    {
        public IAuthenticatorApi GetAuthenticatorApi() => authenticatorApi;

        private IAuthenticatorApi authenticatorApi;

        protected override void OnAwake()
        {
            base.OnAwake();

            var connectionInformation =
                ServerConfiguration.GetInstance()
                    .GetConnectionInformation(ServerType.Authenticator);

            Connect(connectionInformation);
        }

        protected override void OnDestroying()
        {
            base.OnDestroying();

            Disconnect();
        }

        protected override void OnConnected()
        {
            base.OnConnected();

            authenticatorApi = new AuthenticatorApi();
            authenticatorApi.SetServerPeer(GetServerPeer());

            Debug.Log("Connected to the authenticator server.");
        }

        protected override void OnDisconnected()
        {
            base.OnDisconnected();

            authenticatorApi.Dispose();

            Debug.Log("Disconnected from the authenticator server.");
        }
    }
}
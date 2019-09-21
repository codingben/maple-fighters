using Scripts.Network.APIs;
using Scripts.Network.Core;
using Scripts.ScriptableObjects;
using UnityEngine;

namespace Scripts.Network.Services
{
    public class AuthenticatorService : ServiceBase, IAuthenticatorService
    {
        public IAuthenticatorApi GetAuthenticatorApi() => authenticatorApi;

        private IAuthenticatorApi authenticatorApi;

        protected override void OnAwake()
        {
            base.OnAwake();

            Connect();
        }

        protected override void OnDestroying()
        {
            base.OnDestroying();

            Disconnect();
        }

        protected override void OnConnected()
        {
            base.OnConnected();

            if (GameConfiguration.GetInstance().Environment
                == Environment.Production)
            {
                authenticatorApi = new AuthenticatorApi();
            }
            else
            {
                authenticatorApi = new DummyAuthenticatorApi();
            }

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
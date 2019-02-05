using UnityEngine;

namespace Scripts.Services
{
    public class AuthenticatorService : ServiceBase, IAuthenticatorService
    {
        public IAuthenticatorApi AuthenticatorApi => authenticatorApi;

        private AuthenticatorApi authenticatorApi;

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
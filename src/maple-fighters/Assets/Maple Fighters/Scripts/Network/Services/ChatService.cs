using UnityEngine;

namespace Scripts.Services
{
    public class ChatService : ServiceBase, IChatService
    {
        public IAuthorizerApi AuthorizerApi => authorizerApi;

        public IChatApi ChatApi => chatApi;

        private AuthorizerApi authorizerApi;
        private ChatApi chatApi;

        protected override void OnConnected()
        {
            base.OnConnected();

            authorizerApi = new AuthorizerApi();
            authorizerApi.SetServerPeer(GetServerPeer());

            chatApi = new ChatApi();
            chatApi.SetServerPeer(GetServerPeer());

            Debug.Log("Connected to the chat server.");
        }

        protected override void OnDisconnected()
        {
            base.OnDisconnected();

            authorizerApi.Dispose();
            chatApi.Dispose();

            Debug.Log("Disconnected from the chat server.");
        }
    }
}
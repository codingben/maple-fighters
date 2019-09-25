using Scripts.Network.APIs;
using Scripts.Network.Core;
using UnityEngine;

namespace Scripts.Network.Services
{
    public class ChatService : ServiceBase, IChatService
    {
        public IAuthorizerApi GetAuthorizerApi() => authorizerApi;

        public IChatApi GetChatApi() => chatApi;

        private IAuthorizerApi authorizerApi;
        private IChatApi chatApi;

        protected override void OnAwake()
        {
            base.OnAwake();

            DontDestroyOnLoad(gameObject);

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
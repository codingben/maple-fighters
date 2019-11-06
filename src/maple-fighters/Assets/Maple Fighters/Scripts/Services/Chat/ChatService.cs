using System;
using Network.Scripts;
using Network.Utils;
using ScriptableObjects.Configurations;
using Scripts.Services.Authorizer;

namespace Scripts.Services.Chat
{
    public class ChatService : Singleton<ChatService>, IChatService
    {
        public IAuthorizerApi AuthorizerApi { get; set; }

        public IChatApi ChatApi { get; set; }

        private void Awake()
        {
            var networkConfiguration = NetworkConfiguration.GetInstance();
            if (networkConfiguration != null)
            {
                switch (networkConfiguration.Environment)
                {
                    case HostingEnvironment.Production:
                    {
                        break;
                    }

                    case HostingEnvironment.Development:
                    {
                        var dummyPeer = new DummyPeer();

                        AuthorizerApi = new DummyAuthorizerApi(dummyPeer);
                        ChatApi = new DummyChatApi(dummyPeer);
                        break;
                    }
                }
            }
        }

        private void OnDestroy()
        {
            (AuthorizerApi as IDisposable)?.Dispose();
            (ChatApi as IDisposable)?.Dispose();
        }
    }
}
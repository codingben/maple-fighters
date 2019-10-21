using System;
using Network.Scripts;
using Network.Utils;
using Scripts.ScriptableObjects;
using Scripts.Services.Authorizer;

namespace Scripts.Services.Chat
{
    public class ChatService : Singleton<ChatService>, IChatService
    {
        public IAuthorizerApi AuthorizerApi { get; set; }

        public IChatApi ChatApi { get; set; }

        private void Awake()
        {
            var gameConfiguration = GameConfiguration.GetInstance();
            if (gameConfiguration != null)
            {
                if (gameConfiguration.Environment == HostingEnvironment.Development)
                {
                    var dummyPeer = new DummyPeer();

                    AuthorizerApi = new DummyAuthorizerApi(dummyPeer);
                    ChatApi = new DummyChatApi(dummyPeer);
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
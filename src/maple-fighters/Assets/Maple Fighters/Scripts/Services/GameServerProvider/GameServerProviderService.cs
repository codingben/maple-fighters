using System;
using Network.Scripts;
using Network.Utils;
using Scripts.ScriptableObjects;
using Scripts.Services.Authorizer;

namespace Scripts.Services.GameServerProvider
{
    public class GameServerProviderService : Singleton<GameServerProviderService>, IGameServerProviderService
    {
        public IAuthorizerApi AuthorizerApi { get; set; }

        public IGameServerProviderApi GameServerProviderApi { get; set; }

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
                        GameServerProviderApi = new DummyGameServerProviderApi(dummyPeer);
                        break;
                    }
                }
            }
        }

        private void OnDestroy()
        {
            (AuthorizerApi as IDisposable)?.Dispose();
            (GameServerProviderApi as IDisposable)?.Dispose();
        }
    }
}
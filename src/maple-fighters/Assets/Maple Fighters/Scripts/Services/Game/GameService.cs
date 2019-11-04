using System;
using Network.Scripts;
using Network.Utils;
using Scripts.ScriptableObjects;
using Scripts.Services.Authorizer;

namespace Scripts.Services.Game
{
    public class GameService : Singleton<GameService>, IGameService
    {
        public IAuthorizerApi AuthorizerApi { get; set; }

        public ICharacterSelectorApi CharacterSelectorApi { get; set; }

        public IGameSceneApi GameSceneApi { get; set; }

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
                        CharacterSelectorApi = new DummyCharacterSelectorApi(dummyPeer);
                        GameSceneApi = new DummyGameSceneApi(dummyPeer);
                        break;
                    }
                }
            }

            DontDestroyOnLoad(gameObject);
        }

        private void OnDestroy()
        {
            (AuthorizerApi as IDisposable)?.Dispose();
            (CharacterSelectorApi as IDisposable)?.Dispose();
            (GameSceneApi as IDisposable)?.Dispose();
        }
    }
}
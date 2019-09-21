using Scripts.Network.APIs;
using Scripts.Network.Core;
using Scripts.ScriptableObjects;
using UnityEngine;

namespace Scripts.Network.Services
{
    public class GameServerProviderService : ServiceBase, IGameServerProviderService
    {
        public IAuthorizerApi GetAuthorizerApi() => authorizerApi;

        public IGameServerProviderApi GetGameServerProviderApi() => gameServerProviderApi;

        private IAuthorizerApi authorizerApi;
        private IGameServerProviderApi gameServerProviderApi;

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

            authorizerApi = new AuthorizerApi();
            authorizerApi.SetServerPeer(GetServerPeer());

            if (GameConfiguration.GetInstance().Environment
                == Environment.Production)
            {
                gameServerProviderApi = new GameServerProviderApi();
            }
            else
            {
                gameServerProviderApi = new DummyGameServerProviderApi();
            }

            gameServerProviderApi.SetServerPeer(GetServerPeer());

            Debug.Log("Connected to the game server provider.");
        }

        protected override void OnDisconnected()
        {
            base.OnDisconnected();

            authorizerApi.Dispose();
            gameServerProviderApi.Dispose();

            Debug.Log("Disconnected from the game server provider.");
        }
    }
}
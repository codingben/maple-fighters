using UnityEngine;

namespace Scripts.Services
{
    public class GameServerProviderService : ServiceBase, IGameServerProviderService
    {
        public IAuthorizerApi GetAuthorizerApi() => authorizerApi;

        public IGameServerProviderApi GetGameServerProviderApi() => gameServerProviderApi;

        private AuthorizerApi authorizerApi;
        private GameServerProviderApi gameServerProviderApi;

        protected override void OnConnected()
        {
            base.OnConnected();

            authorizerApi = new AuthorizerApi();
            authorizerApi.SetServerPeer(GetServerPeer());

            gameServerProviderApi = new GameServerProviderApi();
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
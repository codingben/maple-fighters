using Scripts.Network;
using UnityEngine;

namespace Scripts.Containers
{
    public static class ServiceContainer
    {
        public static IAuthenticatorService AuthenticatorService
        {
            get
            {
                if (authenticatorService == null)
                {
                    var gameObject = new GameObject("Authenticator Service");
                    authenticatorService = 
                        gameObject.AddComponent<AuthenticatorService>();
                }

                return authenticatorService;
            }
        }

        public static IGameServerProviderService GameServerProviderService
        {
            get
            {
                if (gameServerProviderService == null)
                {
                    var gameObject = 
                        new GameObject("Game Server Provider Service");
                    gameServerProviderService = 
                        gameObject.AddComponent<GameServerProviderService>();
                }

                return gameServerProviderService;
            }
        }

        public static IGameService GameService
        {
            get
            {
                if (gameService == null)
                {
                    var gameObject = new GameObject("Game Service");
                    gameService = gameObject.AddComponent<GameService>();
                }

                return gameService;
            }
        }
        
        public static IChatService ChatService
        {
            get
            {
                if (chatService == null)
                {
                    var gameObject = new GameObject("Chat Service");
                    chatService = gameObject.AddComponent<ChatService>();
                }

                return chatService;
            }
        }

        private static IAuthenticatorService authenticatorService;
        private static IGameServerProviderService gameServerProviderService;
        private static IGameService gameService;
        private static IChatService chatService;
    }
}
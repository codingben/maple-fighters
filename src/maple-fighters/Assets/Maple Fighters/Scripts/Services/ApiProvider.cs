using ScriptableObjects.Configurations;
using Scripts.Services.AuthenticatorApi;
using Scripts.Services.CharacterProviderApi;
using Scripts.Services.ChatApi;
using Scripts.Services.GameApi;
using Scripts.Services.GameProviderApi;

namespace Scripts.Services
{
    public static class ApiProvider
    {
        #region AuthenticatorApi
        public static IAuthenticatorApi ProvideAuthenticatorApi()
        {
            if (authenticatorApi == null)
            {
                // TODO: Get environment only for this service
                authenticatorApi = DummyAuthenticatorApi.GetInstance();
            }

            return authenticatorApi;
        }

        public static void RemoveAuthenticatorApi()
        {
            authenticatorApi = null;
        }

        private static IAuthenticatorApi authenticatorApi;
        #endregion

        #region GameApi
        public static IGameApi ProvideGameApi()
        {
            if (gameApi == null)
            {
                // TODO: Get environment only for this service
                var networkConfiguration = NetworkConfiguration.GetInstance();
                if (networkConfiguration.IsProduction())
                {
                    gameApi = WebSocketGameApi.GetInstance();
                }
                else
                {
                    gameApi = DummyGameApi.GetInstance();
                }
            }

            return gameApi;
        }

        public static void RemoveGameApiProvider()
        {
            gameApi = null;
        }

        private static IGameApi gameApi;
        #endregion

        #region GameProviderApi
        public static IGameProviderApi ProvideGameProviderApi()
        {
            if (gameProviderApi == null)
            {
                // TODO: Get environment only for this service
                var networkConfiguration = NetworkConfiguration.GetInstance();
                if (networkConfiguration.IsProduction())
                {
                    gameProviderApi = HttpGameProviderApi.GetInstance();
                }
                else
                {
                    gameProviderApi = DummyGameProviderApi.GetInstance();
                }
            }

            return gameProviderApi;
        }

        public static void RemoveGameProviderApi()
        {
            gameProviderApi = null;
        }

        private static IGameProviderApi gameProviderApi;
        #endregion

        #region CharacterProviderApi
        public static ICharacterProviderApi ProvideCharacterProviderApi()
        {
            if (characterProviderApi == null)
            {
                // TODO: Get environment only for this service
                characterProviderApi = DummyCharacterProviderApi.GetInstance();
            }

            return characterProviderApi;
        }

        public static void RemoveCharacterProviderApi()
        {
            characterProviderApi = null;
        }

        private static ICharacterProviderApi characterProviderApi;
        #endregion

        #region ChatApi
        public static IChatApi ProvideChatApi()
        {
            if (chatApi == null)
            {
                // TODO: Get environment only for this service
                var networkConfiguration = NetworkConfiguration.GetInstance();
                if (networkConfiguration.IsProduction())
                {
                    chatApi = WebSocketChatApi.GetInstance();
                }
                else
                {
                    chatApi = DummyChatApi.GetInstance();
                }
            }

            return chatApi;
        }

        public static void RemoveChatApiProvider()
        {
            chatApi = null;
        }

        private static IChatApi chatApi;
        #endregion
    }
}
using ScriptableObjects.Configurations;
using Scripts.Services.AuthenticatorApi;
using Scripts.Services.CharacterProviderApi;
using Scripts.Services.ChatApi;
using Scripts.Services.GameApi;
using Scripts.Services.GameProviderApi;
using UnityEngine;

namespace Scripts.Services
{
    public static class ApiProvider
    {
        #region AuthenticatorApi
        public static IAuthenticatorApi ProvideAuthenticatorApi()
        {
            var networkConfiguration = NetworkConfiguration.GetInstance();
            if (networkConfiguration.IsProduction() ||
                networkConfiguration.IsDevelopment())
            {
                authenticatorApi = HttpAuthenticatorApi.GetInstance();
            }
            else
            {
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
            var networkConfiguration = NetworkConfiguration.GetInstance();
            if (networkConfiguration.IsProduction() ||
                networkConfiguration.IsDevelopment())
            {
                gameApi = WebSocketGameApi.GetInstance();
            }
            else
            {
                gameApi = DummyGameApi.GetInstance();
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
            var networkConfiguration = NetworkConfiguration.GetInstance();
            if (networkConfiguration.IsProduction() ||
                networkConfiguration.IsDevelopment())
            {
                gameProviderApi = HttpGameProviderApi.GetInstance();
            }
            else
            {
                gameProviderApi = DummyGameProviderApi.GetInstance();
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
            characterProviderApi = DummyCharacterProviderApi.GetInstance();

            var networkConfiguration = NetworkConfiguration.GetInstance();
            if (networkConfiguration.IsProduction() ||
                networkConfiguration.IsDevelopment())
            {
                var userMetadata = Object.FindObjectOfType<UserMetadata>();
                if (userMetadata != null && userMetadata.IsLoggedIn)
                {
                    characterProviderApi = HttpCharacterProviderApi.GetInstance();
                }
                else
                {
                    characterProviderApi = DummyCharacterProviderApi.GetInstance();
                }
            }
            else
            {
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
            var networkConfiguration = NetworkConfiguration.GetInstance();
            if (networkConfiguration.IsProduction() ||
                networkConfiguration.IsDevelopment())
            {
                chatApi = WebSocketChatApi.GetInstance();
            }
            else
            {
                chatApi = DummyChatApi.GetInstance();
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
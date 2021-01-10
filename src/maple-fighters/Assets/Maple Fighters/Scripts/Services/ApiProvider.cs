using ScriptableObjects.Configurations;
using Scripts.Services.AuthenticatorApi;
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
                var networkConfiguration = NetworkConfiguration.GetInstance();
                if (networkConfiguration.IsProduction())
                {
                    authenticatorApi = HttpAuthenticatorApi.GetInstance();
                }
                else
                {
                    authenticatorApi = DummyAuthenticatorApi.GetInstance();
                }
            }

            return authenticatorApi;
        }

        private static IAuthenticatorApi authenticatorApi;
        #endregion

        #region GameApi
        public static IGameApi ProvideGameApi()
        {
            if (gameApi == null)
            {
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

        private static IGameApi gameApi;
        #endregion

        #region GameProviderApi
        public static IGameProviderApi ProvideGameProviderApi()
        {
            if (gameProviderApi == null)
            {
                var networkConfiguration = NetworkConfiguration.GetInstance();
                if (networkConfiguration.IsProduction())
                {
                    gameProviderApi = HttpGameProviderApi.GetInstance();
                }
                else
                {
                    // TODO: Implement
                }
            }

            return gameProviderApi;
        }

        private static IGameProviderApi gameProviderApi;
        #endregion
    }
}
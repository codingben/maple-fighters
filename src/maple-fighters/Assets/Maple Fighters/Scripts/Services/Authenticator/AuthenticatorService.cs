using System;
using Network.Scripts;
using Network.Utils;
using Scripts.ScriptableObjects;

namespace Scripts.Services.Authenticator
{
    public class AuthenticatorService : Singleton<AuthenticatorService>, IAuthenticatorService
    {
        public IAuthenticatorApi AuthenticatorApi { get; set; }

        private void Awake()
        {
            var gameConfiguration = GameConfiguration.GetInstance();
            if (gameConfiguration != null)
            {
                if (gameConfiguration.Environment == HostingEnvironment.Development)
                {
                    AuthenticatorApi =
                        new DummyAuthenticatorApi(new DummyPeer());
                }
            }
        }

        private void OnDestroy()
        {
            (AuthenticatorApi as IDisposable)?.Dispose();
        }
    }
}
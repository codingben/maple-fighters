using System;
using Network.Scripts;
using Network.Utils;
using ScriptableObjects.Configurations;

namespace Scripts.Services.Authenticator
{
    public class AuthenticatorService : Singleton<AuthenticatorService>, IAuthenticatorService
    {
        public IAuthenticatorApi AuthenticatorApi { get; set; }

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
                        AuthenticatorApi =
                            new DummyAuthenticatorApi(new DummyPeer());
                        break;
                    }
                }
            }
        }

        private void OnDestroy()
        {
            (AuthenticatorApi as IDisposable)?.Dispose();
        }
    }
}
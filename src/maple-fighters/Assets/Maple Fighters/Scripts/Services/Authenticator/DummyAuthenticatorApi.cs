using UnityEngine;

namespace Scripts.Services.AuthenticatorApi
{
    public class DummyAuthenticatorApi : MonoBehaviour, IAuthenticatorApi
    {
        public static DummyAuthenticatorApi GetInstance()
        {
            if (instance == null)
            {
                var authenticatorApi =
                    new GameObject("Dummy Authenticator Api");
                instance =
                    authenticatorApi.AddComponent<DummyAuthenticatorApi>();
            }

            return instance;
        }

        private static DummyAuthenticatorApi instance;

        public void Authenticate(string email, string password)
        {
            throw new System.NotImplementedException();
        }

        public void Register(
            string email,
            string password,
            string firstname,
            string lastname)
        {
            throw new System.NotImplementedException();
        }
    }
}
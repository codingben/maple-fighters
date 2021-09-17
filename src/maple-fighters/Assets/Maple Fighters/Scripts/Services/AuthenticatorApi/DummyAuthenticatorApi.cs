using System;
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

        public Action<long, string> LoginCallback { get; set; }

        public Action<long, string> RegisterCallback { get; set; }

        public void Login(string email, string password)
        {
            var statusCode = 200;
            var json = string.Empty;

            LoginCallback?.Invoke(statusCode, json);
        }

        public void Register(
            string email,
            string password,
            string firstname,
            string lastname)
        {
            var statusCode = 200;
            var json = string.Empty;

            RegisterCallback?.Invoke(statusCode, json);
        }

        private void OnDestroy()
        {
            ApiProvider.RemoveAuthenticatorApi();
        }
    }
}
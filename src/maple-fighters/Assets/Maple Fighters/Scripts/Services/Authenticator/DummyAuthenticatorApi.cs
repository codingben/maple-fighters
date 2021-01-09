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

        public Action<long, string> Authentication { get; set; }

        public Action<long, string> Registration { get; set; }

        public void Authenticate(string email, string password)
        {
            var statusCode = 200;
            var json = string.Empty;

            Authentication?.Invoke(statusCode, json);
        }

        public void Register(
            string email,
            string password,
            string firstname,
            string lastname)
        {
            var statusCode = 200;
            var json = string.Empty;

            Registration?.Invoke(statusCode, json);
        }
    }
}
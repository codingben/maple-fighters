using System;
using Proyecto26;
using ScriptableObjects.Configurations;
using UnityEngine;

namespace Scripts.Services.AuthenticatorApi
{
    public class HttpAuthenticatorApi : MonoBehaviour, IAuthenticatorApi
    {
        public static HttpAuthenticatorApi GetInstance()
        {
            if (instance == null)
            {
                var authenticatorApi =
                    new GameObject("Http Authenticator Api");
                instance =
                    authenticatorApi.AddComponent<HttpAuthenticatorApi>();
            }

            return instance;
        }

        private static HttpAuthenticatorApi instance;

        public Action<long, string> LoginCallback { get; set; }

        public Action<long, string> RegisterCallback { get; set; }

        private string url;

        private void Awake()
        {
            var networkConfiguration = NetworkConfiguration.GetInstance();
            if (networkConfiguration != null)
            {
                var serverData =
                    networkConfiguration.GetServerData(ServerType.Authenticator);

                url = serverData.Url;
            }
        }

        public void Login(string email, string password)
        {
            var loginData = new LoginData()
            {
                email = email,
                password = password
            }.ToString();

            RestClient.Post($"{url}/login", loginData, OnLoginCallback);
        }

        private void OnLoginCallback(RequestException request, ResponseHelper response)
        {
            var statusCode = response.StatusCode;
            var json = response.Text;

            LoginCallback?.Invoke(statusCode, json);
        }

        public void Register(
            string email,
            string password,
            string firstname,
            string lastname)
        {
            var registrationData = new RegistrationData()
            {
                email = email,
                password = password,
                firstname = firstname,
                lastname = lastname
            }.ToString();

            RestClient.Post($"{url}/register", registrationData, OnRegisterCallback);
        }

        private void OnRegisterCallback(RequestException request, ResponseHelper response)
        {
            var statusCode = response.StatusCode;
            var json = response.Text;

            RegisterCallback?.Invoke(statusCode, json);
        }
    }
}
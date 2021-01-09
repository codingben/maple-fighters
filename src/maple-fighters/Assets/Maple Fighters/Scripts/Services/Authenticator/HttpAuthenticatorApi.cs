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

        public Action<long, string> Authentication { get; set; }

        public Action<long, string> Registration { get; set; }

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

        public void Authenticate(string email, string password)
        {
            var loginData = new LoginData()
            {
                email = email,
                password = password
            }.ToString();

            RestClient.Post(url + "/login", loginData, OnAuthentication);
        }

        private void OnAuthentication(RequestException request, ResponseHelper response)
        {
            var statusCode = response.StatusCode;
            var json = response.Text;

            Authentication?.Invoke(statusCode, json);
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

            RestClient.Post(url + "/register", registrationData, OnRegistration);
        }

        private void OnRegistration(RequestException request, ResponseHelper response)
        {
            var statusCode = response.StatusCode;
            var json = response.Text;

            Registration?.Invoke(statusCode, json);
        }
    }
}
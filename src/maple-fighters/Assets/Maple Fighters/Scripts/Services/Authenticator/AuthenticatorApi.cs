using Proyecto26;
using ScriptableObjects.Configurations;
using UnityEngine;

namespace Scripts.Services.AuthenticatorApi
{
    public class AuthenticatorApi : MonoBehaviour, IAuthenticatorApi
    {
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

        public void OnAuthentication(RequestException request, ResponseHelper response)
        {
            print($"{response.StatusCode}");
            print($"{response.Text}");
            print($"{response.Data}");
            print($"{response.Error}");
        }

        public void Register()
        {
            // TODO: Implement
        }
    }
}
using System;
using Proyecto26;
using ScriptableObjects.Configurations;
using UnityEngine;

namespace Scripts.Services.GameProviderApi
{
    public class HttpGameProviderApi : MonoBehaviour, IGameProviderApi
    {
        public static HttpGameProviderApi GetInstance()
        {
            if (instance == null)
            {
                var gameProviderApi =
                    new GameObject("Http GameProvider Api");
                instance =
                    gameProviderApi.AddComponent<HttpGameProviderApi>();
            }

            return instance;
        }

        private static HttpGameProviderApi instance;

        public Action<long, string> GamesCallback { get; set; }

        private string url;

        private void Awake()
        {
            var networkConfiguration = NetworkConfiguration.GetInstance();
            if (networkConfiguration != null)
            {
                var serverData =
                    networkConfiguration.GetServerData(ServerType.GameProvider);

                url = serverData.Url;
            }
        }

        public void Games()
        {
            RestClient.Get($"{url}/games", OnGamesCallback);
        }

        private void OnGamesCallback(RequestException request, ResponseHelper response)
        {
            var statusCode = response.StatusCode;
            var json = response.Text;

            GamesCallback?.Invoke(statusCode, json);
        }
    }
}
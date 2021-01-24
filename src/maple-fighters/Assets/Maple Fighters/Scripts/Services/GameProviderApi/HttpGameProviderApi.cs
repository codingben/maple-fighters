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

        public Action<long, string> GetGamesCallback { get; set; }

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

        public void GetGames()
        {
            RestClient.Get($"{url}/games", OnGetGamesCallback);
        }

        private void OnGetGamesCallback(RequestException request, ResponseHelper response)
        {
            var statusCode = response.StatusCode;
            var json = response.Text;

            GetGamesCallback?.Invoke(statusCode, json);
        }
    }
}
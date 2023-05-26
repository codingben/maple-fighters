using System;
using UnityEngine;

namespace Scripts.Services.GameProviderApi
{
    public class DummyGameProviderApi : MonoBehaviour, IGameProviderApi
    {
        public static DummyGameProviderApi GetInstance()
        {
            if (instance == null)
            {
                var gameProviderApi =
                    new GameObject("Dummy GameProvider Api");
                instance =
                    gameProviderApi.AddComponent<DummyGameProviderApi>();
            }

            return instance;
        }

        private static DummyGameProviderApi instance;

        public Action<long, string> GetGamesCallback { get; set; }

        public void GetGames()
        {
            var statusCode = (long)StatusCodes.Ok;
            var json = "[{\"name\":\"Europe\",\"protocol\":\"ws\",\"url\":\"localhost/game\"}]";

            GetGamesCallback?.Invoke(statusCode, json);
        }

        private void OnDestroy()
        {
            ApiProvider.RemoveGameProviderApi();
        }
    }
}
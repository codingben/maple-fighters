using System.Linq;
using Newtonsoft.Json;
using Scripts.Services;
using Scripts.Services.GameProviderApi;
using UnityEngine;

namespace Scripts.UI.GameServerBrowser
{
    [RequireComponent(typeof(IOnGameServerReceivedListener))]
    public class GameServerBrowserInteractor : MonoBehaviour
    {
        private IGameProviderApi gameProviderApi;
        private IOnGameServerReceivedListener onGameServerReceivedListener;

        private void Awake()
        {
            gameProviderApi =
                ApiProvider.ProvideGameProviderApi();
            onGameServerReceivedListener =
                GetComponent<IOnGameServerReceivedListener>();

            if (gameProviderApi != null)
            {
                gameProviderApi.GetGamesCallback += OnGetGamesCallback;
            }
        }

        private void OnDestroy()
        {
            if (gameProviderApi != null)
            {
                gameProviderApi.GetGamesCallback -= OnGetGamesCallback;
            }
        }

        public void GetGames()
        {
            gameProviderApi?.GetGames();
        }

        public void SetGameServerInfo(string ip, int port)
        {
            // TODO: Refactor. Don't use hard-coded protocol
            UserData.GameServerUrl = $"ws://{ip}:{port}/game";
        }

        private void OnGetGamesCallback(long statusCode, string json)
        {
            if (statusCode == 200) // Ok
            {
                var gameData = JsonConvert.DeserializeObject<GameData[]>(json);
                if (gameData != null)
                {
                    var uiGameData =
                        gameData.Select((x) => new UIGameServerButtonData(
                            x.ip,
                            x.name,
                            x.port,
                            connections: 0,
                            maxConnections: 1000));

                    onGameServerReceivedListener.OnGameServerReceived(uiGameData);
                }
            }
        }
    }
}
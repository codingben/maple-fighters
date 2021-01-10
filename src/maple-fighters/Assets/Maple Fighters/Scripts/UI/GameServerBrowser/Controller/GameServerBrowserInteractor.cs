using System.Linq;
using Proyecto26;
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
                gameProviderApi.GamesCallback += OnGamesCallback;
            }
        }

        private void OnDestroy()
        {
            if (gameProviderApi != null)
            {
                gameProviderApi.GamesCallback -= OnGamesCallback;
            }
        }

        public void GetGames()
        {
            gameProviderApi?.Games();
        }

        public void SetGameServerInfo(string ip, int port)
        {
            // TODO: Set game server data
        }

        private void OnGamesCallback(long statusCode, string json)
        {
            if (statusCode == 200) // Ok
            {
                var gameData = JsonHelper.ArrayFromJson<GameData>(json);
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
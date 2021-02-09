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

        public void SetGameServerInfo(UIGameServerButtonData gameServerData)
        {
            var protocol = gameServerData.Protocol;
            var url = gameServerData.Url;

            UserData.GameServerUrl = $"{protocol}://{url}";
        }

        private void OnGetGamesCallback(long statusCode, string json)
        {
            if (statusCode == 200) // Ok
            {
                var gameData = JsonHelper.ArrayFromJson<GameData>(json);
                if (gameData.Length != 0)
                {
                    var uiGameData =
                        gameData.Select((x) => new UIGameServerButtonData(
                            x.name,
                            x.protocol,
                            x.url));

                    onGameServerReceivedListener.OnGameServerReceived(uiGameData);
                }
            }
        }
    }
}
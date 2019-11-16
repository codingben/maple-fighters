using System.Linq;
using System.Threading.Tasks;
using CommonTools.Coroutines;
using Network.Scripts;
using Scripts.Services.GameServerProvider;
using UnityEngine;

namespace Scripts.UI.GameServerBrowser
{
    [RequireComponent(typeof(IOnConnectionFinishedListener))]
    [RequireComponent(typeof(IOnGameServerReceivedListener))]
    public class GameServerBrowserInteractor : MonoBehaviour
    {
        private GameServerProviderService gameServerProviderService;

        private IOnConnectionFinishedListener onConnectionFinishedListener;
        private IOnGameServerReceivedListener onGameServerReceivedListener;

        private ExternalCoroutinesExecutor coroutinesExecutor;

        private void Awake()
        {
            gameServerProviderService =
                FindObjectOfType<GameServerProviderService>();

            onConnectionFinishedListener =
                GetComponent<IOnConnectionFinishedListener>();
            onGameServerReceivedListener =
                GetComponent<IOnGameServerReceivedListener>();

            coroutinesExecutor = new ExternalCoroutinesExecutor();
        }

        private void Update()
        {
            coroutinesExecutor?.Update();
        }

        private void OnDestroy()
        {
            coroutinesExecutor?.Dispose();
        }

        public void ProvideGameServers()
        {
            coroutinesExecutor?.StartTask(ProvideGameServersAsync);
        }

        private async Task ProvideGameServersAsync(IYield yield)
        {
            await ConnectIfNotConnectedAsync(yield);

            var gameServerProviderApi =
                gameServerProviderService?.GameServerProviderApi;
            if (gameServerProviderApi != null)
            {
                var responseParameters =
                    await gameServerProviderApi.ProvideGameServersAsync(yield);
                var gameServers =
                    responseParameters.GameServerInformations.Select(
                        (x) => new UIGameServerButtonData(
                            x.IP,
                            x.Name,
                            x.Port,
                            x.Connections,
                            x.MaxConnections));

                onGameServerReceivedListener.OnGameServerReceived(gameServers);
            }
        }

        private async Task ConnectIfNotConnectedAsync(IYield yield)
        {
            if (gameServerProviderService != null)
            {
                var connectionStatus =
                    await gameServerProviderService.ConnectAsync(yield);
                if (connectionStatus == ConnectionStatus.Failed)
                {
                    onConnectionFinishedListener.OnConnectionFailed();
                }
            }
        }
    }
}
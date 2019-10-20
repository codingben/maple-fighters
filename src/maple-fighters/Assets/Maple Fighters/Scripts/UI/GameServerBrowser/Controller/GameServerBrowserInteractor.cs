using System.Linq;
using System.Threading.Tasks;
using CommonTools.Coroutines;
using Scripts.Services.GameServerProvider;
using UnityEngine;

namespace Scripts.UI.GameServerBrowser
{
    [RequireComponent(typeof(IOnGameServerReceivedListener))]
    public class GameServerBrowserInteractor : MonoBehaviour
    {
        private IGameServerProviderService gameServerProviderService;
        private IOnGameServerReceivedListener onGameServerReceivedListener;

        private ExternalCoroutinesExecutor coroutinesExecutor;

        private void Awake()
        {
            gameServerProviderService = GameServerProviderService.GetInstance();

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
            var api = gameServerProviderService?.GameServerProviderApi;
            if (api != null)
            {
                var responseParameters =
                    await api.ProvideGameServersAsync(yield);
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
    }
}
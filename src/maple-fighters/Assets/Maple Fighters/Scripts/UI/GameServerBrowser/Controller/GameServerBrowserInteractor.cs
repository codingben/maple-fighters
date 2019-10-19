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
        private IGameServerProviderApi gameServerProviderApi;
        private IOnGameServerReceivedListener onGameServerReceivedListener;

        private ExternalCoroutinesExecutor coroutinesExecutor;

        private void Awake()
        {
            gameServerProviderApi = GameServerProviderService.GetInstance()
                .GameServerProviderApi;

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
            var responseParameters =
                await gameServerProviderApi.ProvideGameServersAsync(yield);

            var gameServers = responseParameters.GameServerInformations;
            onGameServerReceivedListener.OnGameServerReceived(
                gameServers.Select(
                    x => new UIGameServerButtonData(
                        x.IP,
                        x.Name,
                        x.Port,
                        x.Connections,
                        x.MaxConnections)));
        }
    }
}
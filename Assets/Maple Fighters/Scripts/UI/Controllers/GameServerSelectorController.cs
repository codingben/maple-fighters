using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CommonCommunicationInterfaces;
using CommonTools.Coroutines;
using CommonTools.Log;
using GameServerProvider.Client.Common;
using Scripts.Containers;
using Scripts.Services;
using Scripts.UI.Core;
using Scripts.UI.Windows;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Scripts.UI.Controllers
{
    public class GameServerSelectorController : MonoSingleton<GameServerSelectorController>
    {
        [SerializeField] private int loadSceneIndex;

        private bool isInitialized;
        private bool isRefreshing;
        private string gameServerName;

        private readonly ExternalCoroutinesExecutor coroutinesExecutor = new ExternalCoroutinesExecutor();
        private readonly Dictionary<string, GameServerInformationParameters> gameServerInformations = new Dictionary<string, GameServerInformationParameters>();

        public void Initialize()
        {
            isInitialized = true;
            CreateGameServerSelectorWindow();

            GameServerSelectorConnectionProvider.Instance.Connect(() => coroutinesExecutor.StartTask(ProvideGameServerList));
        }

        private void Update()
        {
            coroutinesExecutor.Update();
        }

        private void OnDestroy()
        {
            coroutinesExecutor.Dispose();

            if (isInitialized)
            {
                RemoveGameServerSelectorWindow();
            }
        }

        private void CreateGameServerSelectorWindow()
        {
            var gameServerSelectorWindow = UserInterfaceContainer.Instance.Add<GameServerSelectorWindow>();
            gameServerSelectorWindow.JoinButtonClicked += OnJoinButtonClicked;
            gameServerSelectorWindow.RefreshButtonClicked += OnRefreshButtonClicked;
            gameServerSelectorWindow.GameServerButtonClicked += OnGameServerButtonClicked;
        }

        private void RemoveGameServerSelectorWindow()
        {
            var gameServerSelectorWindow = UserInterfaceContainer.Instance.Get<GameServerSelectorWindow>().AssertNotNull();
            gameServerSelectorWindow.JoinButtonClicked -= OnJoinButtonClicked;
            gameServerSelectorWindow.RefreshButtonClicked -= OnRefreshButtonClicked;
            gameServerSelectorWindow.GameServerButtonClicked -= OnGameServerButtonClicked;
        }

        private void OnJoinButtonClicked()
        {
            if (!gameServerInformations.ContainsKey(gameServerName))
            {
                LogUtils.Log(MessageBuilder.Trace($"A game server with name {gameServerName} does not exist."));
                return;
            }

            Action onHide = delegate 
            {
                var gameServerInfo = gameServerInformations[gameServerName];
                GameConnectionProvider.Instance.Connect(gameServerInfo.Name, OnGameConnected, new PeerConnectionInformation(gameServerInfo.IP, gameServerInfo.Port));
            };

            var gameServerSelectorWindow = UserInterfaceContainer.Instance.Get<GameServerSelectorWindow>().AssertNotNull();
            gameServerSelectorWindow.Hide(onHide);
        }

        private void OnRefreshButtonClicked()
        {
            if (isRefreshing)
            {
                return;
            }

            if (gameServerInformations.Count != 0)
            {
                gameServerInformations.Clear();
            }

            coroutinesExecutor.StartTask(ProvideGameServerList);

            isRefreshing = true;
        }

        private void OnGameServerButtonClicked(string serverName)
        {
            gameServerName = serverName;

            LogUtils.Log($"Selected a server with name {serverName}");
        }

        private void OnGameConnected()
        {
            RemoveGameServerSelectorWindow();

            SceneManager.LoadScene(loadSceneIndex, LoadSceneMode.Single);
        }

        private async Task ProvideGameServerList(IYield yield)
        {
            isRefreshing = true;

            var gameServerProviderService = ServiceContainer.GameServerProviderService.GetPeerLogic<IGameServerProviderServiceAPI>().AssertNotNull();
            var responseParameters = await gameServerProviderService.ProvideGameServers(yield);
            foreach (var gameServerInformation in responseParameters.GameServerInformations)
            {
                gameServerInformations.Add(gameServerInformation.Name, gameServerInformation);
            }

            SetGameServerList();
        }

        private void SetGameServerList()
        {
            var gameServerList = gameServerInformations.Select(gameServerInformation => gameServerInformation.Key).ToArray();
            var gameServerSelectorWindow = UserInterfaceContainer.Instance.Get<GameServerSelectorWindow>().AssertNotNull();
            gameServerSelectorWindow.ShowMessage = false;
            gameServerSelectorWindow.CreateGameServerList(gameServerList);

            isRefreshing = false;
        }
    }
}
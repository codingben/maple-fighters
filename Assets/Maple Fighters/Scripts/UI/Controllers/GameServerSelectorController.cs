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
        private string gameServerName;

        private readonly ExternalCoroutinesExecutor coroutinesExecutor = new ExternalCoroutinesExecutor();
        private readonly Dictionary<string, GameServerInformationParameters> gameServerInformations = new Dictionary<string, GameServerInformationParameters>();

        public void Initialize()
        {
            CreateGameServerSelectorWindow();
            CreateGameServerSelectorRefreshImage();

            GameServerSelectorConnectionProvider.Instance.Connect(OnRefreshButtonClicked);
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
            isInitialized = true;

            var gameServerSelectorWindow = UserInterfaceContainer.Instance.Add<GameServerSelectorWindow>();
            gameServerSelectorWindow.JoinButtonClicked += OnJoinButtonClicked;
            gameServerSelectorWindow.RefreshButtonClicked += OnRefreshButtonClicked;
            gameServerSelectorWindow.GameServerButtonClicked += OnGameServerButtonClicked;
        }

        private void RemoveGameServerSelectorWindow()
        {
            isInitialized = false;

            var gameServerSelectorWindow = UserInterfaceContainer.Instance.Get<GameServerSelectorWindow>().AssertNotNull();
            gameServerSelectorWindow.JoinButtonClicked -= OnJoinButtonClicked;
            gameServerSelectorWindow.RefreshButtonClicked -= OnRefreshButtonClicked;
            gameServerSelectorWindow.GameServerButtonClicked -= OnGameServerButtonClicked;
        }

        private void CreateGameServerSelectorRefreshImage()
        {
            var gameServerSelectorWindow = UserInterfaceContainer.Instance.Get<GameServerSelectorWindow>().AssertNotNull();
            var gameServerSelectorRefreshImage = UserInterfaceContainer.Instance.Add<GameServerSelectorRefreshImage>(ViewType.Foreground, Index.Last, gameServerSelectorWindow.transform);
            gameServerSelectorRefreshImage.Show();
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
            if (gameServerInformations.Count != 0)
            {
                gameServerInformations.Clear();
            }

            var isRefreshImageExists = UserInterfaceContainer.Instance.Get<GameServerSelectorRefreshImage>();
            if(!isRefreshImageExists)
            {
                var gameServerSelectorWindow = UserInterfaceContainer.Instance.Get<GameServerSelectorWindow>().AssertNotNull();
                var gameServerSelectorRefreshImage = UserInterfaceContainer.Instance.Add<GameServerSelectorRefreshImage>(ViewType.Foreground, Index.Last, gameServerSelectorWindow.transform);
                gameServerSelectorRefreshImage.Show(() => coroutinesExecutor.StartTask(ProvideGameServerList));
            }
            else
            {
                coroutinesExecutor.StartTask(ProvideGameServerList);
            }
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
            var gameServerProviderService = ServiceContainer.GameServerProviderService.GetPeerLogic<IGameServerProviderServiceAPI>().AssertNotNull();
            var responseParameters = await gameServerProviderService.ProvideGameServers(yield);
            foreach (var gameServerInformation in responseParameters.GameServerInformations)
            {
                var gameServerName = gameServerInformation.Name;
                if (gameServerInformations.ContainsKey(gameServerName))
                {
                    LogUtils.Log(MessageBuilder.Trace($"Duplication of the {gameServerName} game server. Can not add this one."));
                    continue;
                }

                gameServerInformations.Add(gameServerName, gameServerInformation);
            }
            
            ShowGameServerList();
        }

        private void ShowGameServerList()
        {
            var gameServerSelectorRefreshImage = UserInterfaceContainer.Instance.Get<GameServerSelectorRefreshImage>().AssertNotNull();
            gameServerSelectorRefreshImage.Hide();

            var gameServerSelectorWindow = UserInterfaceContainer.Instance.Get<GameServerSelectorWindow>().AssertNotNull();
            gameServerSelectorWindow.CreateGameServerList(gameServerInformations.Values.ToArray());
        }
    }
}
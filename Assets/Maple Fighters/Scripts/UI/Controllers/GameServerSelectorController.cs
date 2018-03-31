﻿using System;
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
    using WaitForSeconds = CommonTools.Coroutines.WaitForSeconds;

    public class GameServerSelectorController : MonoSingleton<GameServerSelectorController>
    {
        [SerializeField] private int loadSceneIndex;

        private bool isInitialized;
        private string gameServerName;
        private GameServerSelectorWindow gameServerSelectorWindow;

        private readonly ExternalCoroutinesExecutor coroutinesExecutor = new ExternalCoroutinesExecutor();
        private readonly Dictionary<string, GameServerInformationParameters> gameServerInformations = new Dictionary<string, GameServerInformationParameters>();

        public void ShowGameServerSelectorUI()
        {
            if (!IsGameServerSelectorWindowExists())
            {
                gameServerSelectorWindow = CreateGameServerSelectorWindow();
            }

            if (!isInitialized)
            {
                isInitialized = true;
                gameServerSelectorWindow.Show(RefreshGameServerList);
            }
            else
            {
                gameServerSelectorWindow.Show();
            }
        }

        private void Update()
        {
            coroutinesExecutor.Update();
        }

        protected override void OnDestroyed()
        {
            base.OnDestroyed();

            coroutinesExecutor.Dispose();

            if (IsGameServerSelectorWindowExists())
            {
                RemoveGameServerSelectorWindow();
            }
        }

        private GameServerSelectorWindow CreateGameServerSelectorWindow()
        {
            gameServerSelectorWindow = UserInterfaceContainer.Instance.Add<GameServerSelectorWindow>();
            gameServerSelectorWindow.JoinButtonClicked += OnJoinButtonClicked;
            gameServerSelectorWindow.RefreshButtonClicked += OnRefreshButtonClicked;
            gameServerSelectorWindow.GameServerButtonClicked += OnGameServerButtonClicked;
            return gameServerSelectorWindow;
        }

        private void RemoveGameServerSelectorWindow()
        {
            gameServerSelectorWindow.JoinButtonClicked -= OnJoinButtonClicked;
            gameServerSelectorWindow.RefreshButtonClicked -= OnRefreshButtonClicked;
            gameServerSelectorWindow.GameServerButtonClicked -= OnGameServerButtonClicked;

            UserInterfaceContainer.Instance?.Remove(gameServerSelectorWindow);
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

            gameServerSelectorWindow.Hide(onHide);
        }

        private void OnRefreshButtonClicked()
        {
            RefreshGameServerList();
        }

        private void OnGameServerButtonClicked(string serverName)
        {
            gameServerName = serverName;

            LogUtils.Log(MessageBuilder.Trace($"Selected a server with name {serverName}"));
        }

        private void RefreshGameServerList()
        {
            if (GameServerSelectorConnectionProvider.Instance.IsConnected())
            {
                if (gameServerInformations.Count != 0)
                {
                    gameServerInformations.Clear();
                }

                gameServerSelectorWindow.OnRefreshBegan();
                gameServerSelectorWindow.GameServerSelectorRefreshImage.Message = "Getting server list...";

                Action provideGameServerList = () => coroutinesExecutor.StartTask(ProvideGameServerList, exception => Utils.ShowExceptionNotice());
                if (!gameServerSelectorWindow.GameServerSelectorRefreshImage.IsShowed)
                {
                    gameServerSelectorWindow.GameServerSelectorRefreshImage.Show(provideGameServerList);
                }
                else
                {
                    provideGameServerList.Invoke();
                }
            }
            else
            {
                LogUtils.Log(MessageBuilder.Trace("There is no connection to game server provider."));
            }
        }

        private void OnGameConnected()
        {
            RemoveGameServerSelectorWindow();

            SceneManager.LoadScene(loadSceneIndex, LoadSceneMode.Single);
        }

        private async Task ProvideGameServerList(IYield yield)
        {
            await yield.Return(new WaitForSeconds(0.5f));

            var gameServerProviderService = ServiceContainer.GameServerProviderService.GetPeerLogic<IGameServerProviderPeerLogicAPI>().AssertNotNull();
            var responseParameters = await gameServerProviderService.ProvideGameServers(yield);
            foreach (var gameServerInformation in responseParameters.GameServerInformations)
            {
                var gameServerName = gameServerInformation.Name;
                if (gameServerInformations.ContainsKey(gameServerName))
                {
                    LogUtils.Log(MessageBuilder.Trace($"Duplication of the {gameServerName} game server. Can not add more than one."));
                    continue;
                }

                gameServerInformations.Add(gameServerName, gameServerInformation);
            }

            gameServerSelectorWindow.EnableAllButtons();

            if (gameServerInformations.Count != 0)
            {
                ShowGameServerList();
            }
            else
            {
                gameServerSelectorWindow.GameServerSelectorRefreshImage.Message = "No servers found.";
            }
        }

        private void ShowGameServerList()
        {
            gameServerSelectorWindow.GameServerSelectorRefreshImage.Hide();
            gameServerSelectorWindow.CreateGameServerList(gameServerInformations.Values.ToArray());
        }

        private bool IsGameServerSelectorWindowExists() => gameServerSelectorWindow != null;
    }
}
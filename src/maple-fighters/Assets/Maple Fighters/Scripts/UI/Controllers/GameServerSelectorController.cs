using System;
using System.Collections.Generic;
using Scripts.UI.Windows;
using UI.Manager;
using UnityEngine;

namespace Scripts.UI.Controllers
{
    public class GameServerSelectorController : MonoBehaviour
    {
        public event Action JoinGameServer;

        public event Action RefreshGameServerList;

        public event Action<string> GameServerSelected;

        private GameServerSelectorWindow gameServerSelectorWindow;
        private Dictionary<string, GameServerButton> gameServerButtons;

        private const string RefreshImageMessage = "Getting server list...";

        private void Awake()
        {
            gameServerButtons = new Dictionary<string, GameServerButton>();
        }

        private void OnDestroy()
        {
            RemoveGameServerButtons();
            RemoveGameServerSelectorWindow();
        }

        public void CreateGameServerSelectorWindow()
        {
            gameServerSelectorWindow = UIElementsCreator.GetInstance()
                .Create<GameServerSelectorWindow>();
            gameServerSelectorWindow.JoinButtonClicked +=
                OnJoinButtonClicked;
            gameServerSelectorWindow.RefreshButtonClicked +=
                OnRefreshButtonClicked;
            gameServerSelectorWindow.Show();
        }

        public void RemoveGameServerSelectorWindow()
        {
            if (gameServerSelectorWindow != null)
            {
                gameServerSelectorWindow.JoinButtonClicked -=
                    OnJoinButtonClicked;
                gameServerSelectorWindow.RefreshButtonClicked -=
                    OnRefreshButtonClicked;

                Destroy(gameServerSelectorWindow);
            }
        }

        public void CreateGameServerButtons(
            IEnumerable<UIGameServerButtonData> gameServerButtonDatas)
        {
            foreach (var gameServerButtonData in gameServerButtonDatas)
            {
                var gameServerButton = UIElementsCreator.GetInstance()
                    .Create<GameServerButton>(
                        UILayer.Foreground,
                        UIIndex.End,
                        gameServerSelectorWindow.GameServerList);
                gameServerButton
                    .SetUiGameServerButtonData(gameServerButtonData);
                gameServerButton.ButtonClicked += OnGameServerButtonClicked;

                gameServerButtons.Add(
                    gameServerButtonData.ServerName,
                    gameServerButton);
            }

            ShowGameServerList();
        }

        public void RemoveGameServerButtons()
        {
            foreach (var gameServerButton in gameServerButtons.Values)
            {
                gameServerButton.ButtonClicked -= OnGameServerButtonClicked;

                Destroy(gameServerButton.gameObject);
            }

            gameServerButtons.Clear();
        }

        private void OnGameServerButtonClicked(string serverName)
        {
            if (gameServerSelectorWindow != null)
            {
                gameServerSelectorWindow.EnableJoinButton();
            }

            GameServerSelected?.Invoke(serverName);
        }

        private void OnJoinButtonClicked()
        {
            if (gameServerSelectorWindow != null)
            {
                gameServerSelectorWindow.DisableAllButtons();
                gameServerSelectorWindow.Hide();
            }

            JoinGameServer?.Invoke();
        }

        private void OnRefreshButtonClicked()
        {
            ShowRefreshingGameServerList();

            RefreshGameServerList?.Invoke();
        }
        
        private void ShowGameServerList()
        {
            if (gameServerSelectorWindow != null)
            {
                if (gameServerSelectorWindow.RefreshImage != null)
                {
                    gameServerSelectorWindow.RefreshImage.Hide();
                }

                gameServerSelectorWindow.DisableAllButtons();
                gameServerSelectorWindow.EnableRefreshButton();
            }
        }

        private void ShowRefreshingGameServerList()
        {
            if (gameServerSelectorWindow != null)
            {
                gameServerSelectorWindow.DisableAllButtons();

                if (gameServerSelectorWindow.RefreshImage != null)
                {
                    gameServerSelectorWindow.RefreshImage.Message =
                        RefreshImageMessage;
                    gameServerSelectorWindow.RefreshImage.Show();
                }
            }
        }
    }
}
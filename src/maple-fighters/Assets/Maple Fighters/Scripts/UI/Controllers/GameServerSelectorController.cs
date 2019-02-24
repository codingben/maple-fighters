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

        private void Awake()
        {
            gameServerButtons = new Dictionary<string, GameServerButton>();

            CreateGameServerSelectorWindow();
        }

        public void ShowGameServerSelectorWindow()
        {
            if (gameServerSelectorWindow != null)
            {
                gameServerSelectorWindow.Show();
            }
        }

        private void CreateGameServerSelectorWindow()
        {
            gameServerSelectorWindow = UIElementsCreator.GetInstance()
                .Create<GameServerSelectorWindow>();
            gameServerSelectorWindow.JoinButtonClicked +=
                OnJoinButtonClicked;
            gameServerSelectorWindow.RefreshButtonClicked +=
                OnRefreshButtonClicked;
        }

        private void OnDestroy()
        {
            DestroyGameServerButtons();
            DestroyGameServerSelectorWindow();
        }

        private void DestroyGameServerSelectorWindow()
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

        private void DestroyGameServerButtons()
        {
            var gameServerButtonDatas = gameServerButtons.Values;

            foreach (var gameServerButton in gameServerButtonDatas)
            {
                if (gameServerButton != null)
                {
                    gameServerButton.ButtonClicked -= OnGameServerButtonClicked;

                    Destroy(gameServerButton.gameObject);
                }
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
            HideGameServerSelectorWindow();

            JoinGameServer?.Invoke();
        }

        private void HideGameServerSelectorWindow()
        {
            if (gameServerSelectorWindow != null)
            {
                gameServerSelectorWindow.DisableAllButtons();
                gameServerSelectorWindow.Hide();
            }
        }

        private void OnRefreshButtonClicked()
        {
            ShowRefreshingGameServerList();

            RefreshGameServerList?.Invoke();
        }
        
        private void ShowGameServerList()
        {
            HideRefreshImage();

            if (gameServerSelectorWindow != null)
            {
                gameServerSelectorWindow.DisableAllButtons();
                gameServerSelectorWindow.EnableRefreshButton();
            }
        }

        private void HideRefreshImage()
        {
            if (gameServerSelectorWindow != null)
            {
                if (gameServerSelectorWindow.RefreshImage != null)
                {
                    gameServerSelectorWindow.RefreshImage.Hide();
                }
            }
        }

        private void ShowRefreshingGameServerList()
        {
            ShowRefreshImage();

            if (gameServerSelectorWindow != null)
            {
                gameServerSelectorWindow.DisableAllButtons();
            }
        }

        private void ShowRefreshImage()
        {
            if (gameServerSelectorWindow != null)
            {
                if (gameServerSelectorWindow.RefreshImage != null)
                {
                    gameServerSelectorWindow.RefreshImage.Show();
                }
            }
        }
    }
}
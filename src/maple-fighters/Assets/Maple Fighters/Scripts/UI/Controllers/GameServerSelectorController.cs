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

        private Dictionary<string, GameServerButton> gameServerButtons;

        private IGameServerSelectorView gameServerSelectorView;

        private void Awake()
        {
            gameServerButtons = new Dictionary<string, GameServerButton>();

            CreateGameServerSelectorWindow();
        }

        public void ShowGameServerSelectorWindow()
        {
            if (gameServerSelectorView != null)
            {
                gameServerSelectorView.Show();
            }
        }

        private void CreateGameServerSelectorWindow()
        {
            gameServerSelectorView = UIElementsCreator.GetInstance()
                .Create<GameServerSelectorWindow>();
            gameServerSelectorView.JoinButtonClicked +=
                OnJoinButtonClicked;
            gameServerSelectorView.RefreshButtonClicked +=
                OnRefreshButtonClicked;
        }

        private void OnDestroy()
        {
            DestroyGameServerButtons();
            DestroyGameServerSelectorWindow();
        }

        private void DestroyGameServerSelectorWindow()
        {
            if (gameServerSelectorView != null)
            {
                gameServerSelectorView.JoinButtonClicked -=
                    OnJoinButtonClicked;
                gameServerSelectorView.RefreshButtonClicked -=
                    OnRefreshButtonClicked;
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
                        gameServerSelectorView.GameServerList);
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
            if (gameServerSelectorView != null)
            {
                gameServerSelectorView.EnableJoinButton();
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
            if (gameServerSelectorView != null)
            {
                gameServerSelectorView.DisableAllButtons();
                gameServerSelectorView.Hide();
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

            if (gameServerSelectorView != null)
            {
                gameServerSelectorView.DisableAllButtons();
                gameServerSelectorView.EnableRefreshButton();
            }
        }

        private void HideRefreshImage()
        {
            if (gameServerSelectorView != null)
            {
                if (gameServerSelectorView.RefreshImage != null)
                {
                    gameServerSelectorView.RefreshImage.Hide();
                }
            }
        }

        private void ShowRefreshingGameServerList()
        {
            ShowRefreshImage();

            if (gameServerSelectorView != null)
            {
                gameServerSelectorView.DisableAllButtons();
            }
        }

        private void ShowRefreshImage()
        {
            if (gameServerSelectorView != null)
            {
                if (gameServerSelectorView.RefreshImage != null)
                {
                    gameServerSelectorView.RefreshImage.Show();
                }
            }
        }
    }
}
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

        private Dictionary<string, IGameServerView> gameServerViews;
        private IGameServerSelectorView gameServerSelectorView;

        private void Awake()
        {
            gameServerViews = new Dictionary<string, IGameServerView>();

            CreateAndSubscribeToGameServerSelectorWindow();
        }

        public void ShowGameServerSelectorWindow()
        {
            if (gameServerSelectorView != null)
            {
                gameServerSelectorView.Show();
            }
        }

        private void CreateAndSubscribeToGameServerSelectorWindow()
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
            if (gameServerViews.Count != 0)
            {
                UnsubscribeFromGameServerViews();
            }

            UnsubscribeFromGameServerSelectorWindow();

            gameServerViews.Clear();
        }

        private void UnsubscribeFromGameServerSelectorWindow()
        {
            if (gameServerSelectorView != null)
            {
                gameServerSelectorView.JoinButtonClicked -=
                    OnJoinButtonClicked;
                gameServerSelectorView.RefreshButtonClicked -=
                    OnRefreshButtonClicked;
            }
        }

        public void CreateGameServerViews(IEnumerable<UIGameServerButtonData> datas)
        {
            foreach (var gameServerButtonData in datas)
            {
                var gameServerButton = UIElementsCreator.GetInstance()
                    .Create<GameServerButton>(
                        UILayer.Foreground,
                        UIIndex.End,
                        gameServerSelectorView.GameServerList);
                gameServerButton
                    .SetGameServerButtonData(gameServerButtonData);
                gameServerButton.ButtonClicked += OnGameServerButtonClicked;

                gameServerViews.Add(gameServerButtonData.ServerName, gameServerButton);
            }

            ShowGameServerList();
        }

        private void UnsubscribeFromGameServerViews()
        {
            var gameServerViewes = gameServerViews.Values;
            foreach (var gameServerView in gameServerViewes)
            {
                if (gameServerView != null)
                {
                    gameServerView.ButtonClicked -= OnGameServerButtonClicked;
                }
            }
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
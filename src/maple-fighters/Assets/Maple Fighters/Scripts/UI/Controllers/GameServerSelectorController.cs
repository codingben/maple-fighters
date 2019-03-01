using System.Collections.Generic;
using Scripts.UI.Windows;
using UI.Manager;
using UnityEngine;

namespace Scripts.UI.Controllers
{
    [RequireComponent(typeof(GameServerSelectorInteractor))]
    public class GameServerSelectorController : MonoBehaviour, IOnGameServerReceivedListener
    {
        private Dictionary<string, IGameServerView> gameServerViews;
        private IGameServerSelectorView gameServerSelectorView;

        private GameServerSelectorInteractor gameServerSelectorInteractor;

        private void Awake()
        {
            gameServerViews = new Dictionary<string, IGameServerView>();
            gameServerSelectorInteractor =
                GetComponent<GameServerSelectorInteractor>();

            CreateAndSubscribeToGameServerSelectorWindow();
        }

        public void ShowGameServerSelectorWindow()
        {
            gameServerSelectorView?.Show();
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
        
        public void OnGameServerReceived(IEnumerable<UIGameServerButtonData> datas)
        {
            gameServerViews.Clear();

            foreach (var gameServerButtonData in datas)
            {
                var gameServerButton = UIElementsCreator.GetInstance()
                    .Create<GameServerButton>(
                        UILayer.Foreground,
                        UIIndex.End,
                        gameServerSelectorView.GameServerList);
                gameServerButton.SetGameServerButtonData(
                    gameServerButtonData);
                gameServerButton.ButtonClicked += OnGameServerButtonClicked;

                var serverName = gameServerButtonData.ServerName;
                gameServerViews.Add(serverName, gameServerButton);
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
            gameServerSelectorView?.EnableJoinButton();

            // TODO: GameServerSelected(serverName)
        }

        private void OnJoinButtonClicked()
        {
            HideGameServerSelectorWindow();

            // TODO: JoinGameServer()
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

            gameServerSelectorInteractor.ProvideGameServers();
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
            gameServerSelectorView?.RefreshImage?.Hide();
        }

        private void ShowRefreshingGameServerList()
        {
            ShowRefreshImage();

            gameServerSelectorView?.DisableAllButtons();
        }

        private void ShowRefreshImage()
        {
            gameServerSelectorView?.RefreshImage?.Show();
        }
    }
}
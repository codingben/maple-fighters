using System.Collections.Generic;
using System.Linq;
using Scripts.UI.MenuBackground;
using Scripts.UI.ScreenFade;
using UI;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Scripts.UI.GameServerBrowser
{
    [RequireComponent(typeof(GameServerBrowserInteractor))]
    public class GameServerBrowserController : MonoBehaviour,
                                               IOnGameServerReceivedListener
    {
        private IGameServerBrowserView gameServerBrowserView;

        private GameServerViewCollection? gameServerViewCollection;
        private GameServerBrowserInteractor gameServerBrowserInteractor;
        private ScreenFadeController screenFadeController;

        private void Awake()
        {
            gameServerBrowserInteractor =
                GetComponent<GameServerBrowserInteractor>();
            screenFadeController =
                FindObjectOfType<ScreenFadeController>();
        }

        private void Start()
        {
            CreateGameServerBrowserWindow();

            SubscribeToGameServerBrowserWindow();
            SubscribeToBackgroundClicked();

            RefreshGameServers();
        }

        private void OnDestroy()
        {
            UnsubscribeFromGameServerViews();
            UnsubscribeFromGameServerBrowserWindow();
            UnsubscribeFromBackgroundClicked();
        }

        private void CreateGameServerBrowserWindow()
        {
            gameServerBrowserView = UICreator
                .GetInstance()
                .Create<GameServerBrowserWindow>();
        }

        public void ShowGameServerBrowserWindow()
        {
            if (gameServerBrowserView != null)
            {
                gameServerBrowserView.Show();
            }
        }

        public void HideGameServerBrowserWindow()
        {
            if (gameServerBrowserView != null
                && gameServerBrowserView.IsShown)
            {
                gameServerBrowserView.Hide();
            }
        }

        private void SubscribeToGameServerBrowserWindow()
        {
            if (gameServerBrowserView != null)
            {
                gameServerBrowserView.JoinGameButtonClicked +=
                    OnJoinGameButtonClicked;
                gameServerBrowserView.RefreshButtonClicked +=
                    OnRefreshButtonClicked;
            }
        }

        private void UnsubscribeFromGameServerBrowserWindow()
        {
            if (gameServerBrowserView != null)
            {
                gameServerBrowserView.JoinGameButtonClicked -=
                    OnJoinGameButtonClicked;
                gameServerBrowserView.RefreshButtonClicked -=
                    OnRefreshButtonClicked;
            }
        }

        private void SubscribeToBackgroundClicked()
        {
            var backgroundController =
                FindObjectOfType<MenuBackgroundController>();
            if (backgroundController != null)
            {
                backgroundController.BackgroundClicked +=
                    OnBackgroundClicked;
            }
        }

        private void UnsubscribeFromBackgroundClicked()
        {
            var backgroundController =
                FindObjectOfType<MenuBackgroundController>();
            if (backgroundController != null)
            {
                backgroundController.BackgroundClicked -=
                    OnBackgroundClicked;
            }
        }

        private void OnBackgroundClicked()
        {
            HideGameServerBrowserWindow();
        }

        public void OnGameServerReceived(IEnumerable<UIGameServerButtonData> gameServer)
        {
            UnsubscribeFromGameServerViews();
            DestroyGameServerViews();

            var index = 0;
            var gameServerArray = gameServer.ToArray();

            gameServerViewCollection =
                new GameServerViewCollection(gameServerArray.Length);

            foreach (var data in gameServerArray)
            {
                var view = CreateAndSubscribeToGameServerButton();

                view.SetGameServerData(data);
                view.SetGameServerName(data.Name);

                gameServerViewCollection?.Set(index, view);

                index++;
            }

            HideRefreshImage();
        }

        private IGameServerView CreateAndSubscribeToGameServerButton()
        {
            var gameServerButtonList = gameServerBrowserView.GameServerList;
            var gameServerButton = UICreator
                .GetInstance()
                .Create<GameServerButton>(UICanvasLayer.Foreground, UIIndex.End, gameServerButtonList);

            if (gameServerButton != null)
            {
                gameServerButton.ButtonClicked += OnGameServerButtonClicked;
            }

            return gameServerButton;
        }

        private void DestroyGameServerViews()
        {
            var gameServerViewes = gameServerViewCollection?.GetAll();
            if (gameServerViewes != null)
            {
                foreach (var gameServerView in gameServerViewes)
                {
                    if (gameServerView != null)
                    {
                        var view = gameServerView.GameObject;
                        if (view != null)
                        {
                            Destroy(view);
                        }
                    }
                }
            }
        }

        private void UnsubscribeFromGameServerViews()
        {
            var gameServerViewes = gameServerViewCollection?.GetAll();
            if (gameServerViewes != null)
            {
                foreach (var gameServerView in gameServerViewes)
                {
                    if (gameServerView != null)
                    {
                        gameServerView.ButtonClicked -= OnGameServerButtonClicked;
                    }
                }
            }
        }

        private void OnGameServerButtonClicked(UIGameServerButtonData gameServerData)
        {
            gameServerBrowserInteractor.SetGameServerInfo(gameServerData);
            gameServerBrowserView?.EnableJoinGameButton();
        }

        private void OnJoinGameButtonClicked()
        {
            if (screenFadeController != null)
            {
                screenFadeController.Show();
                screenFadeController.FadeInCompleted += OnFadeInCompleted;
            }
        }

        private void OnFadeInCompleted()
        {
            if (screenFadeController != null)
            {
                screenFadeController.FadeInCompleted -= OnFadeInCompleted;
            }

            var mapName = Constants.SceneNames.Maps.Lobby;

            SceneManager.LoadScene(sceneName: mapName);
        }

        private void OnRefreshButtonClicked()
        {
            RefreshGameServers();
        }

        private void RefreshGameServers()
        {
            ShowRefreshImage();

            gameServerBrowserInteractor.GetGames();
        }

        private void ShowRefreshImage()
        {
            gameServerBrowserView?.DisableJoinGameButton();
            gameServerBrowserView?.RefreshImage?.Show();
        }

        private void HideRefreshImage()
        {
            gameServerBrowserView?.DisableJoinGameButton();
            gameServerBrowserView?.RefreshImage?.Hide();
        }
    }
}
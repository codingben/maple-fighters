using System.Collections.Generic;
using System.Linq;
using Scripts.Constants;
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

        private void Awake()
        {
            gameServerBrowserInteractor =
                GetComponent<GameServerBrowserInteractor>();

            CreateAndShowGameServerBrowserWindow();
            SubscribeToGameServerBrowserWindow();
        }

        private void Start()
        {
            RefreshGameServers();
        }

        private void OnDestroy()
        {
            UnsubscribeFromGameServerViews();
            UnsubscribeFromGameServerBrowserWindow();
        }

        private void CreateAndShowGameServerBrowserWindow()
        {
            gameServerBrowserView = UICreator
                .GetInstance()
                .Create<GameServerBrowserWindow>();

            if (gameServerBrowserView != null)
            {
                gameServerBrowserView.Show();
            }
        }

        private void SubscribeToGameServerBrowserWindow()
        {
            if (gameServerBrowserView != null)
            {
                gameServerBrowserView.JoinButtonClicked +=
                    OnJoinButtonClicked;
                gameServerBrowserView.RefreshButtonClicked +=
                    OnRefreshButtonClicked;
            }
        }

        private void UnsubscribeFromGameServerBrowserWindow()
        {
            if (gameServerBrowserView != null)
            {
                gameServerBrowserView.JoinButtonClicked -=
                    OnJoinButtonClicked;
                gameServerBrowserView.RefreshButtonClicked -=
                    OnRefreshButtonClicked;
            }
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
            gameServerBrowserView?.EnableJoinButton();
        }

        private void OnJoinButtonClicked()
        {
            gameServerBrowserView?.DisableJoinButton();
            gameServerBrowserView?.DisableRefreshButton();
            gameServerBrowserView?.Hide();

            SceneManager.LoadScene(sceneName: SceneNames.Game);
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
            gameServerBrowserView?.DisableJoinButton();
            gameServerBrowserView?.RefreshImage?.Show();
        }

        private void HideRefreshImage()
        {
            gameServerBrowserView?.DisableJoinButton();
            gameServerBrowserView?.RefreshImage?.Hide();
        }
    }
}
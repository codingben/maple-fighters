using System.Collections.Generic;
using System.Linq;
using Scripts.Constants;
using Scripts.UI.Notice;
using UI.Manager;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Scripts.UI.GameServerBrowser
{
    [RequireComponent(typeof(GameServerBrowserInteractor))]
    public class GameServerBrowserController : MonoBehaviour,
                                               IOnConnectionFinishedListener,
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
            gameServerBrowserView = UIElementsCreator.GetInstance()
                .Create<GameServerBrowserWindow>();
            gameServerBrowserView.Show();
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

        public void OnConnectionFailed()
        {
            HideRefreshImage();

            NoticeUtils.ShowNotice(message: NoticeMessages.GameServerBrowserView.ConnectionFailed);
        }

        public void OnGameServerReceived(IEnumerable<UIGameServerButtonData> datas)
        {
            UnsubscribeFromGameServerViews();
            DestroyGameServerViews();

            var index = 0;
            var array = datas.ToArray();

            gameServerViewCollection = 
                new GameServerViewCollection(array.Length);

            foreach (var data in array)
            {
                IGameServerView view = CreateAndSubscribeToGameServerButton();
                view.SetGameServerData(data);
                view.SetGameServerName(data.ServerName);
                view.SetGameServerConnections(data.Connections, data.MaxConnections);

                gameServerViewCollection?.Set(index, view);

                index++;
            }

            HideRefreshImage();
        }

        private GameServerButton CreateAndSubscribeToGameServerButton()
        {
            var gameServerButton = UIElementsCreator.GetInstance()
                .Create<GameServerButton>(
                    UILayer.Foreground,
                    UIIndex.End,
                    gameServerBrowserView.GameServerList);
            gameServerButton.ButtonClicked += OnGameServerButtonClicked;

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
                        gameServerView.ButtonClicked -=
                            OnGameServerButtonClicked;
                    }
                }
            }
        }

        private void OnGameServerButtonClicked(UIGameServerButtonData gameServerData)
        {
            var ip = gameServerData.IP;
            var port = gameServerData.Port;

            gameServerBrowserInteractor.SetGameServerInfo(ip, port);
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

            gameServerBrowserInteractor.ProvideGameServers();
        }

        private void ShowRefreshImage()
        {
            gameServerBrowserView?.DisableJoinButton();
            gameServerBrowserView?.DisableRefreshButton();
            gameServerBrowserView?.RefreshImage?.Show();
        }

        private void HideRefreshImage()
        {
            gameServerBrowserView?.DisableJoinButton();
            gameServerBrowserView?.EnableRefreshButton();
            gameServerBrowserView?.RefreshImage?.Hide();
        }
    }
}
using System.Collections.Generic;
using Scripts.UI.Authenticator;
using UI.Manager;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Scripts.UI.GameServerBrowser
{
    [RequireComponent(typeof(GameServerBrowserInteractor))]
    public class GameServerBrowserController : MonoBehaviour,
                                               IOnGameServerReceivedListener
    {
        [SerializeField]
        private string sceneName;

        private Dictionary<string, IGameServerView> gameServerViews;
        private IGameServerBrowserView gameServerBrowserView;

        private GameServerBrowserInteractor gameServerBrowserInteractor;

        private void Awake()
        {
            gameServerViews = new Dictionary<string, IGameServerView>();
            gameServerBrowserInteractor =
                GetComponent<GameServerBrowserInteractor>();

            CreateAndSubscribeToGameServerBrowserWindow();
            SubscribeToAuthenticatorControllerEvents();
        }

        private void CreateAndSubscribeToGameServerBrowserWindow()
        {
            gameServerBrowserView = UIElementsCreator.GetInstance()
                .Create<GameServerBrowserWindow>();
            gameServerBrowserView.JoinButtonClicked +=
                OnJoinButtonClicked;
            gameServerBrowserView.RefreshButtonClicked +=
                OnRefreshButtonClicked;
        }

        private void SubscribeToAuthenticatorControllerEvents()
        {
            var authenticatorController =
                FindObjectOfType<AuthenticatorController>();
            if (authenticatorController != null)
            {
                authenticatorController.LoginSucceed +=
                    ShowGameServerBrowserWindow;
                authenticatorController.RegistrationSucceed +=
                    ShowGameServerBrowserWindow;
            }
        }

        private void OnDestroy()
        {
            if (gameServerViews.Count != 0)
            {
                UnsubscribeFromGameServerViews();
            }

            UnsubscribeFromGameServerBrowserWindow();
            UnsubscribeFromAuthenticatorControllerEvents();

            gameServerViews.Clear();
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

        private void UnsubscribeFromAuthenticatorControllerEvents()
        {
            var authenticatorController =
                FindObjectOfType<AuthenticatorController>();
            if (authenticatorController != null)
            {
                authenticatorController.LoginSucceed -=
                    ShowGameServerBrowserWindow;
                authenticatorController.RegistrationSucceed -=
                    ShowGameServerBrowserWindow;
            }
        }

        private void ShowGameServerBrowserWindow()
        {
            gameServerBrowserView?.Show();
        }

        public void OnGameServerReceived(IEnumerable<UIGameServerButtonData> datas)
        {
            DestroyGameServerViews();

            foreach (var gameServerButtonData in datas)
            {
                var gameServerButton = CreateAndSubscribeToGameServerButton();
                gameServerButton.SetGameServerButtonData(
                    gameServerButtonData);

                var serverName = gameServerButtonData.ServerName;
                gameServerViews.Add(serverName, gameServerButton);
            }

            ShowGameServerList();
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
            UnsubscribeFromGameServerViews();

            var gameServerViewes = gameServerViews.Values;
            foreach (var gameServerView in gameServerViewes)
            {
                Destroy(gameServerView.GameObject);
            }

            gameServerViews.Clear();
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
            gameServerBrowserView?.EnableJoinButton();

            // TODO: GameServerSelected(serverName)
        }

        private void OnJoinButtonClicked()
        {
            HideGameServerBrowserWindow();

            // TODO: Remove this from here
            SceneManager.LoadScene(sceneName, LoadSceneMode.Single);

            // TODO: JoinGameServer()
        }

        private void HideGameServerBrowserWindow()
        {
            gameServerBrowserView?.DisableAllButtons();
            gameServerBrowserView?.Hide();
        }

        private void OnRefreshButtonClicked()
        {
            ShowRefreshingGameServerList();

            gameServerBrowserInteractor.ProvideGameServers();
        }
        
        private void ShowGameServerList()
        {
            HideRefreshImage();

            gameServerBrowserView?.DisableAllButtons();
            gameServerBrowserView?.EnableRefreshButton();
        }

        private void HideRefreshImage()
        {
            gameServerBrowserView?.RefreshImage?.Hide();
        }

        private void ShowRefreshingGameServerList()
        {
            ShowRefreshImage();

            gameServerBrowserView?.DisableAllButtons();
        }

        private void ShowRefreshImage()
        {
            gameServerBrowserView?.RefreshImage?.Show();
        }
    }
}
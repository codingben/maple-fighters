using System.Collections.Generic;
using Scripts.UI.Windows;
using UI.Manager;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Scripts.UI.Controllers
{
    [RequireComponent(typeof(GameServerSelectorInteractor))]
    public class GameServerBrowserController : MonoBehaviour,
                                               IOnGameServerReceivedListener
    {
        [SerializeField]
        private string sceneName;

        private Dictionary<string, IGameServerView> gameServerViews;
        private IGameServerBrowserView gameServerSelectorView;

        private GameServerSelectorInteractor gameServerSelectorInteractor;

        private void Awake()
        {
            gameServerViews = new Dictionary<string, IGameServerView>();
            gameServerSelectorInteractor =
                GetComponent<GameServerSelectorInteractor>();

            CreateAndSubscribeToGameServerSelectorWindow();
            SubscribeToAuthenticatorControllerEvents();
        }

        private void CreateAndSubscribeToGameServerSelectorWindow()
        {
            gameServerSelectorView = UIElementsCreator.GetInstance()
                .Create<GameServerBrowserWindow>();
            gameServerSelectorView.JoinButtonClicked +=
                OnJoinButtonClicked;
            gameServerSelectorView.RefreshButtonClicked +=
                OnRefreshButtonClicked;
        }

        private void SubscribeToAuthenticatorControllerEvents()
        {
            // TODO: Use event bus system
            var authenticatorController =
                FindObjectOfType<AuthenticatorController>();
            if (authenticatorController != null)
            {
                authenticatorController.LoginSucceed +=
                    ShowGameServerSelectorWindow;
                authenticatorController.RegistrationSucceed +=
                    ShowGameServerSelectorWindow;
            }
        }

        private void OnDestroy()
        {
            if (gameServerViews.Count != 0)
            {
                UnsubscribeFromGameServerViews();
            }

            UnsubscribeFromGameServerSelectorWindow();
            UnsubscribeFromAuthenticatorControllerEvents();

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

        private void UnsubscribeFromAuthenticatorControllerEvents()
        {
            // TODO: Use event bus system
            var authenticatorController =
                FindObjectOfType<AuthenticatorController>();
            if (authenticatorController != null)
            {
                authenticatorController.LoginSucceed -=
                    ShowGameServerSelectorWindow;
                authenticatorController.RegistrationSucceed -=
                    ShowGameServerSelectorWindow;
            }
        }

        private void ShowGameServerSelectorWindow()
        {
            gameServerSelectorView?.Show();
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
                    gameServerSelectorView.GameServerList);
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
            gameServerSelectorView?.EnableJoinButton();

            // TODO: GameServerSelected(serverName)
        }

        private void OnJoinButtonClicked()
        {
            HideGameServerSelectorWindow();

            // TODO: Remove this from here
            SceneManager.LoadScene(sceneName, LoadSceneMode.Single);

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
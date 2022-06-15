using System;
using UI;
using UnityEngine;
using UnityEngine.UI;

namespace Scripts.UI.GameServerBrowser
{
    [RequireComponent(typeof(UIFadeAnimation))]
    public class GameServerBrowserWindow : UIElement, IGameServerBrowserView
    {
        public event Action JoinGameButtonClicked;

        public event Action RefreshButtonClicked;

        public IGameServerBrowserRefreshView RefreshImage => refreshImage;

        public Transform GameServerList => gameServerList;

        [Header("Transform")]
        [SerializeField]
        private Transform gameServerList;

        [Header("Buttons")]
        [SerializeField]
        private Button joinGameButton;

        [SerializeField]
        private Button refreshButton;

        [Header("Image")]
        [SerializeField]
        private GameServerBrowserRefreshImage refreshImage;

        private void Start()
        {
            joinGameButton?.onClick.AddListener(OnJoinGameButtonClicked);
            refreshButton?.onClick.AddListener(OnRefreshButtonClicked);
        }

        private void OnDestroy()
        {
            joinGameButton?.onClick.RemoveListener(OnJoinGameButtonClicked);
            refreshButton?.onClick.RemoveListener(OnRefreshButtonClicked);
        }

        private void OnJoinGameButtonClicked()
        {
            JoinGameButtonClicked?.Invoke();
        }

        private void OnRefreshButtonClicked()
        {
            RefreshButtonClicked?.Invoke();
        }

        public void EnableJoinGameButton()
        {
            if (joinGameButton != null)
            {
                joinGameButton.interactable = true;
            }
        }

        public void DisableJoinGameButton()
        {
            if (joinGameButton != null)
            {
                joinGameButton.interactable = false;
            }
        }

        public void EnableRefreshButton()
        {
            if (refreshButton != null)
            {
                refreshButton.interactable = true;
            }
        }

        public void DisableRefreshButton()
        {
            if (refreshButton != null)
            {
                refreshButton.interactable = false;
            }
        }
    }
}
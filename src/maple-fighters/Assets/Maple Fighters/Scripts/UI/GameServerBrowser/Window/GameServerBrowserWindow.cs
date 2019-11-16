using System;
using UI.Manager;
using UnityEngine;
using UnityEngine.UI;

namespace Scripts.UI.GameServerBrowser
{
    [RequireComponent(typeof(UIFadeAnimation))]
    public class GameServerBrowserWindow : UIElement, IGameServerBrowserView
    {
        public event Action JoinButtonClicked;

        public event Action RefreshButtonClicked;

        public IGameServerBrowserRefreshView RefreshImage => refreshImage;

        public Transform GameServerList => gameServerList;

        [Header("Transform")]
        [SerializeField]
        private Transform gameServerList;

        [Header("Buttons")]
        [SerializeField]
        private Button joinButton;

        [SerializeField]
        private Button refreshButton;

        [Header("Image")]
        [SerializeField]
        private GameServerBrowserRefreshImage refreshImage;
        
        private void Start()
        {
            joinButton?.onClick.AddListener(OnJoinButtonClicked);
            refreshButton?.onClick.AddListener(OnRefreshButtonClicked);
        }

        private void OnDestroy()
        {
            joinButton?.onClick.RemoveListener(OnJoinButtonClicked);
            refreshButton?.onClick.RemoveListener(OnRefreshButtonClicked);
        }

        private void OnJoinButtonClicked()
        {
            JoinButtonClicked?.Invoke();
        }

        private void OnRefreshButtonClicked()
        {
            RefreshButtonClicked?.Invoke();
        }

        public void EnableJoinButton()
        {
            if (joinButton != null)
            {
                joinButton.interactable = true;
            }
        }

        public void DisableJoinButton()
        {
            if (joinButton != null)
            {
                joinButton.interactable = false;
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
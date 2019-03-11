using System;
using UI.Manager;
using UnityEngine;
using UnityEngine.UI;

namespace Scripts.UI.Windows
{
    [RequireComponent(typeof(UIFadeAnimation))]
    public class GameServerBrowserWindow : UIElement, IGameServerBrowserView
    {
        public event Action JoinButtonClicked;

        public event Action RefreshButtonClicked;

        public IGameServerSelectorRefreshView RefreshImage => refreshImage;

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
        private GameServerSelectorRefreshImage refreshImage;
        
        private void Start()
        {
            if (joinButton != null)
            {
                joinButton.onClick.AddListener(OnJoinButtonClicked);
            }

            if (refreshButton != null)
            {
                refreshButton.onClick.AddListener(OnRefreshButtonClicked);
            }
        }

        private void OnDestroy()
        {
            if (joinButton != null)
            {
                joinButton.onClick.RemoveListener(OnJoinButtonClicked);
            }

            if (refreshButton != null)
            {
                refreshButton.onClick.RemoveListener(OnRefreshButtonClicked);
            }
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

        public void EnableRefreshButton()
        {
            if (refreshButton != null)
            {
                refreshButton.interactable = true;
            }
        }

        public void DisableAllButtons()
        {
            if (joinButton != null)
            {
                joinButton.interactable = false;
            }

            if (refreshButton != null)
            {
                refreshButton.interactable = false;
            }
        }
    }
}
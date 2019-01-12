using System;
using UI.Manager;
using UnityEngine;
using UnityEngine.UI;

namespace Scripts.UI.Windows
{
    [RequireComponent(typeof(UIFadeAnimation))]
    public class GameServerSelectorWindow : UIElement
    {
        public event Action JoinButtonClicked;

        public event Action RefreshButtonClicked;

        public GameServerSelectorRefreshImage RefreshImage => refreshImage;

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
            joinButton.onClick.AddListener(OnJoinButtonClicked);
            refreshButton.onClick.AddListener(OnRefreshButtonClicked);
        }

        private void OnDestroy()
        {
            joinButton.onClick.RemoveListener(OnJoinButtonClicked);
            refreshButton.onClick.RemoveListener(OnRefreshButtonClicked);
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
            joinButton.interactable = true;
        }

        public void EnableRefreshButton()
        {
            refreshButton.interactable = true;
        }

        public void DisableAllButtons()
        {
            joinButton.interactable = false;
            refreshButton.interactable = false;
        }
    }
}
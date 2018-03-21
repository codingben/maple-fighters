using System;
using System.Collections.Generic;
using Scripts.UI.Core;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Scripts.UI.Windows
{
    public class GameServerSelectorWindow : UserInterfaceBaseFadeEffect
    {
        public bool ShowMessage
        {
            set
            {
                messageText.gameObject.SetActive(value);
            }
        }

        public string Message
        {
            set
            {
                messageText.text = value;
            }
        }

        public event Action JoinButtonClicked;
        public event Action RefreshButtonClicked;
        public event Action<string> GameServerButtonClicked;

        [Header("Text")]
        [SerializeField] private TextMeshProUGUI messageText;

        [Header("Buttons")]
        [SerializeField] private Button joinButton;
        [SerializeField] private Button refreshButton;

        [Header("Parent")]
        [SerializeField] private Transform gameServerList;

        private readonly Dictionary<string, ClickableGameServerButton> gameServerButtons = new Dictionary<string, ClickableGameServerButton>();

        private void Start()
        {
            SubscribeToEvents();
        }

        private void OnDestroy()
        {
            UnsubscribeFromEvents();
        }

        private void SubscribeToEvents()
        {
            joinButton.onClick.AddListener(OnJoinButtonClicked);
            refreshButton.onClick.AddListener(OnRefreshButtonClicked);
        }

        private void UnsubscribeFromEvents()
        {
            joinButton.onClick.RemoveListener(OnJoinButtonClicked);
            refreshButton.onClick.RemoveListener(OnRefreshButtonClicked);
        }

        public void CreateGameServerList(string[] gameServers)
        {
            StopRefreshing();
            RemoveGameServerList();

            if (gameServers.Length == 0)
            {
                ShowMessage = true;
                Message = "No servers found.";
                return;
            }

            foreach (var gameServerName in gameServers)
            {
                var gameServerButton = UserInterfaceContainer.Instance.Add<ClickableGameServerButton>(ViewType.Foreground, Index.Last, gameServerList);
                gameServerButton.ServerName = gameServerName;
                gameServerButton.ServerButtonClicked += () => OnGameServerButtonClicked(gameServerName);
                gameServerButtons.Add(gameServerName, gameServerButton);
            }
        }

        private void RemoveGameServerList()
        {
            if (gameServerButtons.Count == 0)
            {
                return;
            }

            foreach (var gameServerButton in gameServerButtons)
            {
                var gameServerButtonGameObject = gameServerButton.Value.gameObject;
                Destroy(gameServerButtonGameObject);
            }

            gameServerButtons.Clear();
        }

        public void DeselectAllGameServerButtons()
        {
            foreach (var gameServerButton in gameServerButtons)
            {
                gameServerButton.Value.Selected = false;
            }
        }

        private void OnGameServerButtonClicked(string serverName)
        {
            var gameServerButton = gameServerButtons[serverName];
            var isSelected = gameServerButton.Selected;

            DeselectAllGameServerButtons();

            if (isSelected)
            {
                joinButton.interactable = false;
                return;
            }

            gameServerButton.Selected = true;
            joinButton.interactable = gameServerButton.Selected;

            GameServerButtonClicked?.Invoke(serverName);
        }

        private void OnJoinButtonClicked()
        {
            JoinButtonClicked?.Invoke();
        }

        private void OnRefreshButtonClicked()
        {
            StartRefreshing();

            RefreshButtonClicked?.Invoke();
        }

        private void StartRefreshing()
        {
            ShowMessage = true;
            refreshButton.interactable = false;
        }

        private void StopRefreshing()
        {
            ShowMessage = false;
            refreshButton.interactable = true;
        }
    }
}
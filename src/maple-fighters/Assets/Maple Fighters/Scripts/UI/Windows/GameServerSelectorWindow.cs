using System;
using UI.Manager;
using UnityEngine;
using UnityEngine.UI;

namespace Scripts.UI.Windows
{
    // TODO: Finish code refactoring
    public class GameServerSelectorWindow : UIElement
    {
        public GameServerSelectorRefreshImage GameServerSelectorRefreshImage
        {
            get
            {
                return gameServerSelectorRefreshImage;
            }
        }

        public event Action JoinButtonClicked;

        public event Action RefreshButtonClicked;

        public event Action<string> GameServerButtonClicked;

        [Header("Parent")]
        [SerializeField]
        private Transform gameServerList;

        [Header("Buttons")]
        [SerializeField]
        private Button joinButton;

        [SerializeField]
        private Button refreshButton;

        [Header("Image")]
        [SerializeField]
        private GameServerSelectorRefreshImage gameServerSelectorRefreshImage;

        /*private Dictionary<string, ClickableGameServerButton> gameServerButtons;

        private void Awake()
        {
            gameServerButtons = new Dictionary<string, ClickableGameServerButton>();
        }*/

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

        /*public void CreateGameServerList(IEnumerable<GameServerInformationParameters> gameServerList)
        {
            foreach (var gameServer in gameServerList)
            {
                var gameServerButton = UserInterfaceContainer.GetInstance().Add<ClickableGameServerButton>(ViewType.Foreground, Index.Last, this.gameServerList);
                gameServerButton.ServerName = gameServer.Name;
                gameServerButton.MaxConnections = gameServer.MaxConnections;
                gameServerButton.Connections = gameServer.Connections;
                gameServerButton.ServerButtonClicked += () => OnGameServerButtonClicked(gameServer.Name);

                gameServerButtons.Add(gameServer.Name, gameServerButton);
            }
        }*/

        public void OnRefreshBegan()
        {
            DisableAllButtons();

            // RemoveGameServerList();
        }

        /*private void RemoveGameServerList()
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
        }*/

        /*public void DeselectAllGameServerButtons()
        {
            foreach (var gameServerButton in gameServerButtons)
            {
                gameServerButton.Value.Selected = false;
            }
        }*/

        private void OnGameServerButtonClicked(string serverName)
        {
            /*var gameServerButton = gameServerButtons[serverName];
            var isSelected = gameServerButton.Selected;

            DeselectAllGameServerButtons();

            if (isSelected)
            {
                joinButton.interactable = false;
                return;
            }

            gameServerButton.Selected = true;

            joinButton.interactable = gameServerButton.Selected;*/

            GameServerButtonClicked?.Invoke(serverName);
        }

        private void OnJoinButtonClicked()
        {
            JoinButtonClicked?.Invoke();
        }

        private void OnRefreshButtonClicked()
        {
            RefreshButtonClicked?.Invoke();
        }

        private void DisableAllButtons()
        {
            joinButton.interactable = false;
            refreshButton.interactable = false;
        }

        public void EnableAllButtons()
        {
            joinButton.interactable = false;
            refreshButton.interactable = true;
        }
    }
}
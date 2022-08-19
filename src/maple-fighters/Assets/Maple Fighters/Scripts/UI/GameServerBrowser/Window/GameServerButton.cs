using System;
using UI;
using UnityEngine;
using UnityEngine.UI;

namespace Scripts.UI.GameServerBrowser
{
    [RequireComponent(typeof(Button), typeof(UICanvasGroup))]
    public class GameServerButton : UIElement, IGameServerView
    {
        public event Action<UIGameServerButtonData> ButtonClicked;

        public GameObject GameObject => gameObject;

        [Header("Text")]
        [SerializeField]
        private Text gameServerNameText;

        [Header("Image")]
        [SerializeField]
        private Image frame;

        private UIGameServerButtonData gameServerData;

        private void Start()
        {
            var button = GetComponent<Button>();
            button?.onClick.AddListener(OnButtonClicked);
        }

        private void OnDestroy()
        {
            var button = GetComponent<Button>();
            button?.onClick.RemoveListener(OnButtonClicked);
        }

        private void OnButtonClicked()
        {
            SelectButton();

            ButtonClicked?.Invoke(gameServerData);
        }

        public void SetGameServerData(UIGameServerButtonData gameServerData)
        {
            this.gameServerData = gameServerData;
        }

        public void SetGameServerName(string name)
        {
            if (gameServerNameText != null)
            {
                gameServerNameText.text = name;
            }
        }

        public void SetGameServerConnections(int min, int max)
        {
            // Unimplemented
        }

        public void SelectButton()
        {
            frame?.gameObject.SetActive(true);
        }

        public void DeselectButton()
        {
            frame?.gameObject.SetActive(false);
        }

        public bool IsButtonSelected()
        {
            return frame != null && frame.gameObject.activeSelf;
        }

        public UIGameServerButtonData GetGameServerData()
        {
            return gameServerData;
        }
    }
}
using System;
using TMPro;
using UI.Manager;
using UnityEngine;
using UnityEngine.UI;

namespace Scripts.UI.GameServerBrowser
{
    [RequireComponent(typeof(Button), typeof(UICanvasGroup))]
    public class GameServerButton : UIElement, IGameServerView
    {
        public event Action<int> ButtonClicked;

        public GameObject GameObject => gameObject;

        [Header("Text")]
        [SerializeField]
        private TextMeshProUGUI gameServerNameText;

        [Header("Slider")]
        [SerializeField]
        private Slider connectionsBar;

        [Header("Image")]
        [SerializeField]
        private Image frame;

        private int index;

        private void Start()
        {
            var button = GetComponent<Button>();
            button.onClick.AddListener(OnButtonClicked);
        }

        private void OnDestroy()
        {
            var button = GetComponent<Button>();
            button.onClick.RemoveListener(OnButtonClicked);
        }

        private void OnButtonClicked()
        {
            SelectButton();

            ButtonClicked?.Invoke(index);
        }

        public void SetGameServerButtonData(int index, UIGameServerButtonData data)
        {
            this.index = index;

            if (gameServerNameText != null)
            {
                gameServerNameText.text = data.ServerName;
            }

            if (connectionsBar != null)
            {
                connectionsBar.maxValue = data.MaxConnections;
                connectionsBar.value = data.Connections;
            }
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
    }
}
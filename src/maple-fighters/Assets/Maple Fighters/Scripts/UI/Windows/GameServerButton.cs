using System;
using TMPro;
using UI.Manager;
using UnityEngine;
using UnityEngine.UI;

namespace Scripts.UI
{
    [RequireComponent(typeof(Button), typeof(UICanvasGroup))]
    public class GameServerButton : UIElement
    {
        public event Action<string> ButtonClicked;

        [Header("Text")]
        [SerializeField]
        private TextMeshProUGUI gameServerNameText;

        [Header("Slider")]
        [SerializeField]
        private Slider connectionsBar;

        [Header("Image")]
        [SerializeField]
        private Image frame;

        private UiGameServerButtonData uiGameServerButtonData;

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

            ButtonClicked?.Invoke(uiGameServerButtonData.ServerName);
        }

        public void SetUiGameServerButtonData(UiGameServerButtonData data)
        {
            uiGameServerButtonData = data;

            if (gameServerNameText != null)
            {
                gameServerNameText.text = data.ServerName;
            }

            if (connectionsBar != null)
            {
                connectionsBar.value = data.Connections;
                connectionsBar.maxValue = data.MaxConnections;
            }
        }

        public void SelectButton()
        {
            if (frame != null)
            {
                frame.gameObject.SetActive(true);
            }
        }

        public void DeselectButton()
        {
            if (frame != null)
            {
                frame.gameObject.SetActive(false);
            }
        }

        public bool IsButtonSelected()
        {
            return frame != null && frame.gameObject.activeSelf;
        }
    }
}
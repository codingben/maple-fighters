using System;
using TMPro;
using UI.Manager;
using UnityEngine;
using UnityEngine.UI;

namespace Scripts.UI
{
    [RequireComponent(typeof(Button))]
    public class ClickableGameServerButton : UIElement
    {
        public event Action ServerButtonClicked;

        [Header("Text")]
        [SerializeField]
        private TextMeshProUGUI gameServerText;

        [Header("Slider")]
        [SerializeField]
        private Slider playersBarSlider;

        [Header("Image")]
        [SerializeField]
        private Image frame;

        private Button serverButton;

        private void Start()
        {
            serverButton = GetComponent<Button>();
            serverButton.onClick.AddListener(OnServerButtonClicked);
        }

        private void OnDestroy()
        {
            serverButton.onClick.RemoveListener(OnServerButtonClicked);
        }

        private void OnServerButtonClicked()
        {
            ServerButtonClicked?.Invoke();
        }

        public void SetGameServerButtonData(UiGameServerButtonData data)
        {
            gameServerText.text = data.ServerName;
            playersBarSlider.value = data.Connections;
            playersBarSlider.maxValue = data.MaxConnections;
        }

        public void Select()
        {
            frame.gameObject.SetActive(true);
        }

        public void Deselect()
        {
            frame.gameObject.SetActive(false);
        }

        public bool IsSelected()
        {
            return frame.gameObject.activeSelf;
        }
    }
}
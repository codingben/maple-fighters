using System;
using Scripts.UI.Core;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Scripts.UI
{
    public class ClickableGameServerButton : UserInterfaceBase
    {
        public string ServerName
        {
            set
            {
                gameServerText.text = value;
            }
        }

        public int Connections
        {
            set
            {
                playersBarSlider.value = value;
            }
        }

        public int MaxConnections
        {
            set
            {
                playersBarSlider.maxValue = value;
            }
        }

        public bool Selected
        {
            set
            {
                frame.gameObject.SetActive(value);
            }
            get
            {
                return frame.gameObject.activeSelf;
            }
        }

        public event Action ServerButtonClicked;

        [Header("Text")]
        [SerializeField] private TextMeshProUGUI gameServerText;

        [Header("Slider")]
        [SerializeField] private Slider playersBarSlider;

        [Header("Foreground")]
        [SerializeField] private Image frame;

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
    }
}
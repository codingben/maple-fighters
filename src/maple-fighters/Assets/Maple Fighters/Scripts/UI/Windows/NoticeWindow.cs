using System;
using TMPro;
using UI.Manager;
using UnityEngine;
using UnityEngine.UI;

namespace Scripts.UI.Windows
{
    [RequireComponent(typeof(UIFadeAnimation))]
    public class NoticeWindow : UIElement
    {
        public event Action OkButtonClicked;

        public string Message
        {
            set
            {
                if (messageText != null)
                {
                    messageText.text = value;
                }
            }
        }

        [Header("Image")]
        [SerializeField]
        private Image backgroundImage;

        [Header("Text")]
        [SerializeField]
        private TextMeshProUGUI messageText;

        [Header("Button")]
        [SerializeField]
        private Button okButton;

        private void Start()
        {
            if (okButton != null)
            {
                okButton.onClick.AddListener(OnOkButtonClicked);
            }
        }

        private void OnDestroy()
        {
            if (okButton != null)
            {
                okButton.onClick.RemoveListener(OnOkButtonClicked);
            }
        }

        private void OnOkButtonClicked()
        {
            Hide();

            OkButtonClicked?.Invoke();
        }

        public void UseBackground()
        {
            if (backgroundImage != null)
            {
                backgroundImage.gameObject.SetActive(true);
            }
        }
    }
}
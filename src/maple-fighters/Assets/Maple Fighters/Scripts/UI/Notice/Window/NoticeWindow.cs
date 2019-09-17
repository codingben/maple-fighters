using System;
using TMPro;
using UI.Manager;
using UnityEngine;
using UnityEngine.UI;

namespace Scripts.UI.Windows
{
    [RequireComponent(typeof(UIFadeAnimation))]
    public class NoticeWindow : UIElement, INoticeView
    {
        public event Action FadeOutCompleted;

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

            SubscribeToUIFadeAnimation();
        }

        private void SubscribeToUIFadeAnimation()
        {
            var uiFadeAnimation = GetComponent<UIFadeAnimation>();
            uiFadeAnimation.FadeOutCompleted += OnFadeOutCompleted;
        }

        private void OnDestroy()
        {
            if (okButton != null)
            {
                okButton.onClick.RemoveListener(OnOkButtonClicked);
            }

            UnsubscribeFromUIFadeAnimation();
        }

        private void UnsubscribeFromUIFadeAnimation()
        {
            var uiFadeAnimation = GetComponent<UIFadeAnimation>();
            uiFadeAnimation.FadeOutCompleted -= OnFadeOutCompleted;
        }

        private void OnFadeOutCompleted()
        {
            FadeOutCompleted?.Invoke();
        }

        private void OnOkButtonClicked()
        {
            OkButtonClicked?.Invoke();
        }

        public void ShowBackground()
        {
            if (backgroundImage != null)
            {
                backgroundImage.gameObject.SetActive(true);
            }
        }

        public void HideBackground()
        {
            if (backgroundImage != null)
            {
                backgroundImage.gameObject.SetActive(false);
            }
        }
    }
}
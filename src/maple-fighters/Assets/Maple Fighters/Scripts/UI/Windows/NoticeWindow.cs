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
                messageText.text = value;
            }
        }

        [Header("Images")]
        [SerializeField]
        private Image backgroundImage;

        [Header("Texts")]
        [SerializeField]
        private TextMeshProUGUI messageText;

        [Header("Buttons")]
        [SerializeField]
        private Button okButton;

        private void Start()
        {
            okButton.onClick.AddListener(OnOkButtonClicked);
        }

        private void OnDestroy()
        {
            okButton.onClick.RemoveListener(OnOkButtonClicked);
        }

        private void OnOkButtonClicked()
        {
            Hide();

            OkButtonClicked?.Invoke();
        }

        public void UseBackground()
        {
            backgroundImage.gameObject.SetActive(true);
        }
    }
}
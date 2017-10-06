using System;
using Scripts.UI.Core;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Scripts.UI.Windows
{
    public class NoticeWindow : UserInterfaceBaseFadeEffect
    {
        public TextMeshProUGUI Message => messageText;
        public Button OkButton => okButton;
        public Action OkButtonClicked;

        [Header("Buttons")]
        [SerializeField] private Button okButton;
        [Header("Texts")]
        [SerializeField] private TextMeshProUGUI messageText;

        public void Initialize(string message, Action okButtonClicked)
        {
            messageText.text = message;
            OkButtonClicked = okButtonClicked;
        }

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

            UserInterfaceContainer.Instance.Remove(this);
        }
    }
}
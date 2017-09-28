using System;
using UnityEngine;
using UnityEngine.UI;

namespace Scripts.UI
{
    public class NoticeWindow : UserInterfaceBaseFadeEffect
    {
        private Action okButtonClicked;

        [Header("Buttons")]
        [SerializeField] private Button okButton;
        [Header("Texts")]
        [SerializeField] private Text messageText;

        public void Initialize(string message, Action okButtonClicked)
        {
            messageText.text = message;

            this.okButtonClicked = okButtonClicked;
        }

        private void Start()
        {
            okButton.onClick.AddListener(OnOkButtonClicked);
        }

        private void OnOkButtonClicked()
        {
            Hide();
            okButtonClicked?.Invoke();
            UserInterfaceContainer.Instance.Remove(this);
        }
    }
}
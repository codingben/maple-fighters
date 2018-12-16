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

        public Action OkButtonClickedAction;

        [Header("Images")]
        [SerializeField]
        private Image blackBackground;

        [Header("Buttons")]
        [SerializeField]
        private Button okButton;

        [Header("Texts")]
        [SerializeField]
        private TextMeshProUGUI messageText;

        public void Initialize(string message, Action okButtonClicked, bool background = false)
        {
            messageText.text = message;
            OkButtonClickedAction = okButtonClicked;

            blackBackground.gameObject.SetActive(background);
        }

        public override void Hide()
        {
            Hide(onFinished: () => UserInterfaceContainer.GetInstance()?.Remove(this));
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

            OkButtonClickedAction?.Invoke();
        }
    }
}
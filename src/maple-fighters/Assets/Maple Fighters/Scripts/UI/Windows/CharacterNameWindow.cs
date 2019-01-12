using System;
using TMPro;
using UI.Manager;
using UnityEngine;
using UnityEngine.UI;

namespace Scripts.UI.Windows
{
    [RequireComponent(typeof(UIFadeAnimation))]
    public class CharacterNameWindow : UIElement
    {
        public event Action<string> ConfirmButtonClicked;

        public event Action BackButtonClicked;

        [Header("Configuration")]
        [SerializeField]
        private int minCharactersName;

        [Header("Input Field")]
        [SerializeField]
        private TMP_InputField nameInputField;

        [Header("Buttons")]
        [SerializeField]
        private Button confirmButton;

        [SerializeField]
        private Button backButton;

        private void Start()
        {
            confirmButton.onClick.AddListener(OnConfirmClicked);
            backButton.onClick.AddListener(OnBackClicked);
            nameInputField.onValueChanged.AddListener(OnNameInputFieldChanged);
        }

        private void OnDestroy()
        {
            confirmButton.onClick.RemoveListener(OnConfirmClicked);
            backButton.onClick.RemoveListener(OnBackClicked);
            nameInputField.onValueChanged.RemoveListener(OnNameInputFieldChanged);
        }

        private void OnNameInputFieldChanged(string text)
        {
            confirmButton.interactable = text.Length >= minCharactersName;
        }

        private void OnConfirmClicked()
        {
            var characterName = nameInputField.text;
            ConfirmButtonClicked?.Invoke(characterName);

            Hide();
            ResetNameInputField();
        }

        private void OnBackClicked()
        {
            Hide();
            ResetNameInputField();

            BackButtonClicked?.Invoke();
        }

        private void ResetNameInputField()
        {
            nameInputField.text = string.Empty;
        }
    }
}
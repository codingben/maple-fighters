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
            if (confirmButton != null)
            {
                confirmButton.onClick.AddListener(OnConfirmButtonClicked);
            }

            if (backButton != null)
            {
                backButton.onClick.AddListener(OnBackButtonClicked);
            }

            if (nameInputField != null)
            {
                nameInputField.onValueChanged.AddListener(
                    OnNameInputFieldChanged);
            }
        }

        private void OnDestroy()
        {
            if (confirmButton != null)
            {
                confirmButton.onClick.RemoveListener(OnConfirmButtonClicked);
            }

            if (backButton != null)
            {
                backButton.onClick.RemoveListener(OnBackButtonClicked);
            }

            if (nameInputField != null)
            {
                nameInputField.onValueChanged.RemoveListener(
                    OnNameInputFieldChanged);
            }
        }

        private void OnNameInputFieldChanged(string text)
        {
            if (confirmButton != null)
            {
                confirmButton.interactable = text.Length >= minCharactersName;
            }
        }

        private void OnConfirmButtonClicked()
        {
            if (nameInputField != null)
            {
                var characterName = nameInputField.text;
                ConfirmButtonClicked?.Invoke(characterName);
            }

            Hide();

            ResetNameInputField();
        }

        private void OnBackButtonClicked()
        {
            Hide();

            ResetNameInputField();

            BackButtonClicked?.Invoke();
        }

        private void ResetNameInputField()
        {
            if (nameInputField != null)
            {
                nameInputField.text = string.Empty;
            }
        }
    }
}
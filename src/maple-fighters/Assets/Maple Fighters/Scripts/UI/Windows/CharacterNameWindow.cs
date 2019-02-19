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

        public event Action<string> NameInputFieldChanged;

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

        public void EnableConfirmButton()
        {
            if (confirmButton != null)
            {
                confirmButton.interactable = true;
            }
        }

        public void DisableConfirmButton()
        {
            if (confirmButton != null)
            {
                confirmButton.interactable = false;
            }
        }

        private void OnNameInputFieldChanged(string characterName)
        {
            NameInputFieldChanged?.Invoke(characterName);
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
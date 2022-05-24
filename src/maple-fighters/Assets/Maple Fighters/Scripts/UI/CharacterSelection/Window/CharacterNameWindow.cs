using System;
using UI;
using UnityEngine;
using UnityEngine.UI;
using Random = System.Random;

namespace Scripts.UI.CharacterSelection
{
    [RequireComponent(typeof(UIFadeAnimation))]
    public class CharacterNameWindow : UIElement, ICharacterNameView
    {
        public event Action<string> ConfirmButtonClicked;

        public event Action BackButtonClicked;

        public event Action<string> NameInputFieldChanged;

        [Header("Input Field")]
        [SerializeField]
        private InputField nameInputField;

        [Header("Buttons")]
        [SerializeField]
        private Button confirmButton;

        [SerializeField]
        private Button backButton;

        private void Start()
        {
            confirmButton?.onClick.AddListener(OnConfirmButtonClicked);
            backButton?.onClick.AddListener(OnBackButtonClicked);
            nameInputField?.onValueChanged.AddListener(OnNameInputFieldChanged);

            SetRandomCharacterName();
        }

        private void OnDestroy()
        {
            confirmButton?.onClick.RemoveListener(OnConfirmButtonClicked);
            backButton?.onClick.RemoveListener(OnBackButtonClicked);
            nameInputField?.onValueChanged.RemoveListener(OnNameInputFieldChanged);
        }

        private void SetRandomCharacterName()
        {
            if (nameInputField != null)
            {
                nameInputField.text = "player" + new Random().Next(1000, 9999);
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

            ResetNameInputField();
            DisableConfirmButton();
        }

        private void OnBackButtonClicked()
        {
            BackButtonClicked?.Invoke();
        }

        public void ResetNameInputField()
        {
            if (nameInputField != null)
            {
                nameInputField.text = string.Empty;
            }
        }
    }
}
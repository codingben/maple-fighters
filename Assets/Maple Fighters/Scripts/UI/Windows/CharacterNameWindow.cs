using System;
using Scripts.UI.Core;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Scripts.UI.Windows
{
    public class CharacterNameWindow : UserInterfaceWindowFadeEffect
    {
        public event Action<string> ConfirmClicked;
        public event Action BackClicked;

        [Header("Configuration")]
        [SerializeField] private int minCharactersName;
        [Header("Input Field")]
        [SerializeField] private TMP_InputField nameInputField;
        [Header("Buttons")]
        [SerializeField] private Button confirmButton;
        [SerializeField] private Button backButton;

        private void Start()
        {
            confirmButton.onClick.AddListener(OnConfirmClicked);
            backButton.onClick.AddListener(OnBackClicked);

            nameInputField.onValueChanged.AddListener(IsEnoughCharactersForInteractableConfirmButton);
        }

        private void OnDestroy()
        {
            confirmButton.onClick.RemoveListener(OnConfirmClicked);
            backButton.onClick.RemoveListener(OnBackClicked);

            nameInputField.onValueChanged.RemoveListener(IsEnoughCharactersForInteractableConfirmButton);
        }

        private void IsEnoughCharactersForInteractableConfirmButton(string text)
        {
            confirmButton.interactable = text.Length >= minCharactersName;
        }

        private void OnConfirmClicked()
        {
            Hide();

            var characterName = nameInputField.text;
            ConfirmClicked?.Invoke(characterName);

            ResetNameInputField();
        }

        private void OnBackClicked()
        {
            Hide();
            ResetNameInputField();

            BackClicked?.Invoke();
        }

        private void ResetNameInputField()
        {
            nameInputField.text = string.Empty;
        }
    }
}
using System;
using UI.Manager;
using UnityEngine;
using UnityEngine.UI;

namespace Scripts.UI.Windows
{
    [RequireComponent(typeof(UIFadeAnimation))]
    public class CharacterSelectionOptionsWindow : UIElement
    {
        public event Action StartButtonClicked;

        public event Action CreateCharacterButtonClicked;

        public event Action DeleteCharacterButtonClicked;

        [SerializeField]
        private Button startButton;

        [SerializeField]
        private Button createCharacterButton;

        [SerializeField]
        private Button deleteCharacterButton;

        private void Start()
        {
            startButton.onClick.AddListener(OnStartButtonClicked);
            createCharacterButton.onClick.AddListener(OnCreateCharacterButtonClicked);
            deleteCharacterButton.onClick.AddListener(OnDeleteCharacterButtonClicked);
        }

        private void OnDestroy()
        {
            startButton.onClick.RemoveListener(OnStartButtonClicked);
            createCharacterButton.onClick.RemoveListener(OnCreateCharacterButtonClicked);
            deleteCharacterButton.onClick.RemoveListener(OnDeleteCharacterButtonClicked);
        }
        
        private void OnStartButtonClicked()
        {
            Hide();

            StartButtonClicked?.Invoke();
        }

        private void OnCreateCharacterButtonClicked()
        {
            Hide();

            CreateCharacterButtonClicked?.Invoke();
        }

        private void OnDeleteCharacterButtonClicked()
        {
            Hide();

            DeleteCharacterButtonClicked?.Invoke();
        }

        public void StartButtonInteraction(bool interctable)
        {
            startButton.interactable = interctable;
        }

        public void CreateCharacterButtonInteraction(bool interctable)
        {
            createCharacterButton.interactable = interctable;
        }

        public void DeleteCharacterButtonInteraction(bool interctable)
        {
            deleteCharacterButton.interactable = interctable;
        }
    }
}
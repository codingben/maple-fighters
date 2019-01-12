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
            startButton.onClick.AddListener(OnStartClicked);
            createCharacterButton.onClick.AddListener(OnCreateCharacterClicked);
            deleteCharacterButton.onClick.AddListener(OnDeleteCharacterClicked);
        }

        private void OnDestroy()
        {
            startButton.onClick.RemoveListener(OnStartClicked);
            createCharacterButton.onClick.RemoveListener(OnCreateCharacterClicked);
            deleteCharacterButton.onClick.RemoveListener(OnDeleteCharacterClicked);
        }
        
        private void OnStartClicked()
        {
            Hide();

            StartButtonClicked?.Invoke();
        }

        private void OnCreateCharacterClicked()
        {
            Hide();

            CreateCharacterButtonClicked?.Invoke();
        }

        private void OnDeleteCharacterClicked()
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
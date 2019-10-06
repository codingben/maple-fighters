using System;
using UI.Manager;
using UnityEngine;
using UnityEngine.UI;

namespace Scripts.UI.CharacterSelection
{
    [RequireComponent(typeof(UIFadeAnimation))]
    public class CharacterSelectionOptionsWindow : UIElement,
                                                   ICharacterSelectionOptionsView
    {
        public event Action StartButtonClicked;

        public event Action CreateCharacterButtonClicked;

        public event Action DeleteCharacterButtonClicked;

        [Header("Buttons")]
        [SerializeField]
        private Button startButton;

        [SerializeField]
        private Button createCharacterButton;

        [SerializeField]
        private Button deleteCharacterButton;

        private void Start()
        {
            startButton?.onClick.AddListener(OnStartButtonClicked);
            createCharacterButton?.onClick.AddListener(OnCreateCharacterButtonClicked);
            deleteCharacterButton?.onClick.AddListener(OnDeleteCharacterButtonClicked);
        }

        private void OnDestroy()
        {
            startButton?.onClick.RemoveListener(OnStartButtonClicked);
            createCharacterButton?.onClick.RemoveListener(OnCreateCharacterButtonClicked);
            deleteCharacterButton?.onClick.RemoveListener(OnDeleteCharacterButtonClicked);
        }
        
        private void OnStartButtonClicked()
        {
            StartButtonClicked?.Invoke();
        }

        private void OnCreateCharacterButtonClicked()
        {
            CreateCharacterButtonClicked?.Invoke();
        }

        private void OnDeleteCharacterButtonClicked()
        {
            DeleteCharacterButtonClicked?.Invoke();
        }

        public void EnableOrDisableStartButton(bool interactable)
        {
            if (startButton != null)
            {
                startButton.interactable = interactable;
            }
        }

        public void EnableOrDisableCreateCharacterButton(bool interactable)
        {
            if (createCharacterButton != null)
            {
                createCharacterButton.interactable = interactable;
            }
        }

        public void EnableOrDisableDeleteCharacterButton(bool interactable)
        {
            if (deleteCharacterButton != null)
            {
                deleteCharacterButton.interactable = interactable;
            }
        }
    }
}
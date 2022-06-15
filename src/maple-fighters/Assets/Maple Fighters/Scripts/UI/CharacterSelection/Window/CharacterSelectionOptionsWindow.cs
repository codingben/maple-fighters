using System;
using UI;
using UnityEngine;
using UnityEngine.UI;

namespace Scripts.UI.CharacterSelection
{
    [RequireComponent(typeof(UIFadeAnimation))]
    public class CharacterSelectionOptionsWindow : UIElement,
                                                   ICharacterSelectionOptionsView
    {
        public event Action JoinGameButtonClicked;

        public event Action CreateCharacterButtonClicked;

        public event Action DeleteCharacterButtonClicked;

        [Header("Buttons")]
        [SerializeField]
        private Button joinGameButton;

        [SerializeField]
        private Button createCharacterButton;

        [SerializeField]
        private Button deleteCharacterButton;

        private void Start()
        {
            joinGameButton?.onClick.AddListener(OnJoinGameButtonClicked);
            createCharacterButton?.onClick.AddListener(OnCreateCharacterButtonClicked);
            deleteCharacterButton?.onClick.AddListener(OnDeleteCharacterButtonClicked);
        }

        private void OnDestroy()
        {
            joinGameButton?.onClick.RemoveListener(OnJoinGameButtonClicked);
            createCharacterButton?.onClick.RemoveListener(OnCreateCharacterButtonClicked);
            deleteCharacterButton?.onClick.RemoveListener(OnDeleteCharacterButtonClicked);
        }

        private void OnJoinGameButtonClicked()
        {
            JoinGameButtonClicked?.Invoke();
        }

        private void OnCreateCharacterButtonClicked()
        {
            CreateCharacterButtonClicked?.Invoke();
        }

        private void OnDeleteCharacterButtonClicked()
        {
            DeleteCharacterButtonClicked?.Invoke();
        }

        public void EnableOrDisableJoinGameButton(bool interactable)
        {
            if (joinGameButton != null)
            {
                joinGameButton.interactable = interactable;
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
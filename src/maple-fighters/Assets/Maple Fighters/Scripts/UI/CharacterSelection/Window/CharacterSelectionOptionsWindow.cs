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
        public event Action ChooseCharacterButtonClicked;

        public event Action CreateCharacterButtonClicked;

        public event Action DeleteCharacterButtonClicked;

        [Header("Buttons")]
        [SerializeField]
        private Button chooseCharacterButton;

        [SerializeField]
        private Button createCharacterButton;

        [SerializeField]
        private Button deleteCharacterButton;

        private void Start()
        {
            chooseCharacterButton?.onClick.AddListener(OnChooseCharacterButtonClicked);
            createCharacterButton?.onClick.AddListener(OnCreateCharacterButtonClicked);
            deleteCharacterButton?.onClick.AddListener(OnDeleteCharacterButtonClicked);
        }

        private void OnDestroy()
        {
            chooseCharacterButton?.onClick.RemoveListener(OnChooseCharacterButtonClicked);
            createCharacterButton?.onClick.RemoveListener(OnCreateCharacterButtonClicked);
            deleteCharacterButton?.onClick.RemoveListener(OnDeleteCharacterButtonClicked);
        }

        private void OnChooseCharacterButtonClicked()
        {
            ChooseCharacterButtonClicked?.Invoke();
        }

        private void OnCreateCharacterButtonClicked()
        {
            CreateCharacterButtonClicked?.Invoke();
        }

        private void OnDeleteCharacterButtonClicked()
        {
            DeleteCharacterButtonClicked?.Invoke();
        }

        public void EnableOrDisableChooseCharacterButton(bool interactable)
        {
            if (chooseCharacterButton != null)
            {
                chooseCharacterButton.interactable = interactable;
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
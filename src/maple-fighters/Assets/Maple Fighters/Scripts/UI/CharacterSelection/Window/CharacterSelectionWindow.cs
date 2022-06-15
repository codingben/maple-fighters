using System;
using UI;
using UnityEngine;
using UnityEngine.UI;

namespace Scripts.UI.CharacterSelection
{
    [RequireComponent(typeof(UIFadeAnimation))]
    public class CharacterSelectionWindow : UIElement, ICharacterSelectionView
    {
        public event Action<UICharacterClass> CharacterSelected;

        public event Action ChooseButtonClicked;

        public event Action CancelButtonClicked;

        [Header("Buttons")]
        [SerializeField]
        private Button cancelButton;

        [SerializeField]
        private Button chooseButton;

        [SerializeField]
        private Button knightButton;

        [SerializeField]
        private Button archerButton;

        [SerializeField]
        private Button wizardButton;

        private void Start()
        {
            chooseButton?.onClick.AddListener(OnChooseButtonClicked);
            cancelButton?.onClick.AddListener(OnCancelButtonClicked);
            knightButton?.onClick.AddListener(OnKnightSelected);
            archerButton?.onClick.AddListener(OnArcherSelected);
            wizardButton?.onClick.AddListener(OnWizardSelected);
        }

        private void OnDestroy()
        {
            chooseButton?.onClick.RemoveListener(OnChooseButtonClicked);
            cancelButton?.onClick.RemoveListener(OnCancelButtonClicked);
            knightButton?.onClick.RemoveListener(OnKnightSelected);
            archerButton?.onClick.RemoveListener(OnArcherSelected);
            wizardButton?.onClick.RemoveListener(OnWizardSelected);
        }

        private void OnChooseButtonClicked()
        {
            ChooseButtonClicked?.Invoke();
        }

        private void OnCancelButtonClicked()
        {
            CancelButtonClicked?.Invoke();
        }

        private void OnKnightSelected()
        {
            OnCharacterSelected(UICharacterClass.Knight);
        }

        private void OnArcherSelected()
        {
            OnCharacterSelected(UICharacterClass.Archer);
        }

        private void OnWizardSelected()
        {
            OnCharacterSelected(UICharacterClass.Wizard);
        }

        private void OnCharacterSelected(UICharacterClass uiCharacterClass)
        {
            CharacterSelected?.Invoke(uiCharacterClass);
        }

        public void EnableChooseButton()
        {
            if (chooseButton != null)
            {
                chooseButton.interactable = true;
            }
        }

        public void DisableChooseButton()
        {
            if (chooseButton != null)
            {
                chooseButton.interactable = false;
            }
        }
    }
}
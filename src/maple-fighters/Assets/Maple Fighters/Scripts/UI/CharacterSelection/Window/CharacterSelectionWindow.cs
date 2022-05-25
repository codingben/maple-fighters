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

        [Header("Images")]
        [SerializeField]
        private GameObject knightSelectedImage;

        [SerializeField]
        private GameObject archerSelectedImage;

        [SerializeField]
        private GameObject wizardSelectedImage;

        [Header("Texts")]
        [SerializeField]
        private Text knightName;

        [SerializeField]
        private Text archerName;

        [SerializeField]
        private Text wizardName;

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

        public void SelectCharacterClass(UICharacterClass uiCharacterClass)
        {
            switch (uiCharacterClass)
            {
                case UICharacterClass.Knight:
                {
                    SelectKnightClass();
                    break;
                }

                case UICharacterClass.Archer:
                {
                    SelectArcherClass();
                    break;
                }

                case UICharacterClass.Wizard:
                {
                    SelectWizardClass();
                    break;
                }
            }
        }

        public void ResetSelection()
        {
            DeselectKnightClass();
            DeselectArcherClass();
            DeselectWizardClass();
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

        private void SelectKnightClass()
        {
            if (knightName != null)
            {
                knightName.fontStyle = FontStyle.Bold;
            }

            knightSelectedImage?.SetActive(true);
        }

        private void DeselectKnightClass()
        {
            if (knightName != null)
            {
                knightName.fontStyle = FontStyle.Normal;
            }

            knightSelectedImage?.SetActive(false);
        }

        private void SelectArcherClass()
        {
            if (archerName != null)
            {
                archerName.fontStyle = FontStyle.Bold;
            }

            archerSelectedImage?.SetActive(true);
        }

        private void DeselectArcherClass()
        {
            if (archerName != null)
            {
                archerName.fontStyle = FontStyle.Normal;
            }

            archerSelectedImage?.SetActive(false);
        }

        private void SelectWizardClass()
        {
            if (wizardName != null)
            {
                wizardName.fontStyle = FontStyle.Bold;
            }

            wizardSelectedImage?.SetActive(true);
        }

        private void DeselectWizardClass()
        {
            if (wizardName != null)
            {
                wizardName.fontStyle = FontStyle.Normal;
            }

            wizardSelectedImage?.SetActive(false);
        }
    }
}
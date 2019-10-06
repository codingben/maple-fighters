using System;
using TMPro;
using UI.Manager;
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
        private Button arrowButton;

        [SerializeField]
        private Button wizardButton;

        [Header("Images")]
        [SerializeField]
        private GameObject knightSelectedImage;

        [SerializeField]
        private GameObject arrowSelectedImage;

        [SerializeField]
        private GameObject wizardSelectedImage;

        [Header("Texts")]
        [SerializeField]
        private TextMeshProUGUI knightName;

        [SerializeField]
        private TextMeshProUGUI arrowName;

        [SerializeField]
        private TextMeshProUGUI wizardName;

        private void Start()
        {
            chooseButton?.onClick.AddListener(OnChooseButtonClicked);
            cancelButton?.onClick.AddListener(OnCancelButtonClicked);
            knightButton?.onClick.AddListener(OnKnightSelected);
            arrowButton?.onClick.AddListener(OnArrowSelected);
            wizardButton?.onClick.AddListener(OnWizardSelected);
        }

        private void OnDestroy()
        {
            chooseButton?.onClick.RemoveListener(OnChooseButtonClicked);
            cancelButton?.onClick.RemoveListener(OnCancelButtonClicked);
            knightButton?.onClick.RemoveListener(OnKnightSelected);
            arrowButton?.onClick.RemoveListener(OnArrowSelected);
            wizardButton?.onClick.RemoveListener(OnWizardSelected);
        }

        private void OnChooseButtonClicked()
        {
            DisableChooseButton();

            DeselectAllCharacterClasses();

            ChooseButtonClicked?.Invoke();
        }

        private void OnCancelButtonClicked()
        {
            DisableChooseButton();

            DeselectAllCharacterClasses();

            CancelButtonClicked?.Invoke();
        }

        private void OnKnightSelected()
        {
            OnCharacterSelected(UICharacterClass.Knight);
        }

        private void OnArrowSelected()
        {
            OnCharacterSelected(UICharacterClass.Arrow);
        }

        private void OnWizardSelected()
        {
            OnCharacterSelected(UICharacterClass.Wizard);
        }

        private void OnCharacterSelected(UICharacterClass uiCharacterClass)
        {
            EnableChooseButton();

            DeselectAllCharacterClasses();

            SelectCharacterClass(uiCharacterClass);

            CharacterSelected?.Invoke(uiCharacterClass);
        }

        private void SelectCharacterClass(UICharacterClass uiCharacterClass)
        {
            switch (uiCharacterClass)
            {
                case UICharacterClass.Knight:
                {
                    SelectKnightClass();
                    break;
                }

                case UICharacterClass.Arrow:
                {
                    SelectArrowClass();
                    break;
                }

                case UICharacterClass.Wizard:
                {
                    SelectWizardClass();
                    break;
                }
            }
        }

        private void DeselectAllCharacterClasses()
        {
            DeselectKnightClass();
            DeselectArrowClass();
            DeselectWizardClass();
        }

        private void EnableChooseButton()
        {
            if (chooseButton != null)
            {
                chooseButton.interactable = true;
            }
        }

        private void DisableChooseButton()
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
                knightName.fontStyle = FontStyles.Bold;
            }

            knightSelectedImage?.SetActive(true);
        }

        private void DeselectKnightClass()
        {
            if (knightName != null)
            {
                knightName.fontStyle = FontStyles.Normal;
            }

            knightSelectedImage?.SetActive(false);
        }

        private void SelectArrowClass()
        {
            if (arrowName != null)
            {
                arrowName.fontStyle = FontStyles.Bold;
            }

            arrowSelectedImage?.SetActive(true);
        }

        private void DeselectArrowClass()
        {
            if (arrowName != null)
            {
                arrowName.fontStyle = FontStyles.Normal;
            }

            arrowSelectedImage?.SetActive(false);
        }

        private void SelectWizardClass()
        {
            if (wizardName != null)
            {
                wizardName.fontStyle = FontStyles.Bold;
            }

            wizardSelectedImage?.SetActive(true);
        }

        private void DeselectWizardClass()
        {
            if (wizardName != null)
            {
                wizardName.fontStyle = FontStyles.Normal;
            }

            wizardSelectedImage?.SetActive(false);
        }
    }
}
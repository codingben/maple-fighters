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
            if (chooseButton != null)
            {
                chooseButton.onClick.AddListener(OnChooseButtonClicked);
            }

            if (cancelButton != null)
            {
                cancelButton.onClick.AddListener(OnCancelButtonClicked);
            }

            if (knightButton != null)
            {
                knightButton.onClick.AddListener(OnKnightSelected);
            }

            if (arrowButton != null)
            {
                arrowButton.onClick.AddListener(OnArrowSelected);
            }

            if (wizardButton != null)
            {
                wizardButton.onClick.AddListener(OnWizardSelected);
            }
        }

        private void OnDestroy()
        {
            if (chooseButton != null)
            {
                chooseButton.onClick.RemoveListener(OnChooseButtonClicked);
            }

            if (cancelButton != null)
            {
                cancelButton.onClick.RemoveListener(OnCancelButtonClicked);
            }

            if (knightButton != null)
            {
                knightButton.onClick.RemoveListener(OnKnightSelected);
            }

            if (arrowButton != null)
            {
                arrowButton.onClick.RemoveListener(OnArrowSelected);
            }

            if (wizardButton != null)
            {
                wizardButton.onClick.RemoveListener(OnWizardSelected);
            }
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

            if (knightSelectedImage != null)
            {
                knightSelectedImage.SetActive(true);
            }
        }

        private void DeselectKnightClass()
        {
            if (knightName != null)
            {
                knightName.fontStyle = FontStyles.Normal;
            }

            if (knightSelectedImage != null)
            {
                knightSelectedImage.SetActive(false);
            }
        }

        private void SelectArrowClass()
        {
            if (arrowName != null)
            {
                arrowName.fontStyle = FontStyles.Bold;
            }

            if (arrowSelectedImage != null)
            {
                arrowSelectedImage.SetActive(true);
            }
        }

        private void DeselectArrowClass()
        {
            if (arrowName != null)
            {
                arrowName.fontStyle = FontStyles.Normal;
            }

            if (arrowSelectedImage != null)
            {
                arrowSelectedImage.SetActive(false);
            }
        }

        private void SelectWizardClass()
        {
            if (wizardName != null)
            {
                wizardName.fontStyle = FontStyles.Bold;
            }

            if (wizardSelectedImage != null)
            {
                wizardSelectedImage.SetActive(true);
            }
        }

        private void DeselectWizardClass()
        {
            if (wizardName != null)
            {
                wizardName.fontStyle = FontStyles.Normal;
            }

            if (wizardSelectedImage != null)
            {
                wizardSelectedImage.SetActive(false);
            }
        }
    }
}
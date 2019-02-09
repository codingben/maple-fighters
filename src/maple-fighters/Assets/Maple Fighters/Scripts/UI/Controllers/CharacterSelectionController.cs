using System;
using Scripts.UI.Windows;
using UI.Manager;
using UnityEngine;

namespace Scripts.UI.Controllers
{
    public class CharacterSelectionController : MonoBehaviour
    {
        public event Action CharacterChosen;

        public event Action CharacterCancelled;

        private UICharacterDetails uiCharacterDetails;
        private CharacterSelectionWindow characterSelectionWindow;

        public void SetCharacterDetails(UICharacterDetails uiCharacterDetails)
        {
            this.uiCharacterDetails = uiCharacterDetails;
        }

        public void ShowCharacterSelectionWindow()
        {
            if (characterSelectionWindow == null)
            {
                characterSelectionWindow = UIElementsCreator.GetInstance()
                    .Create<CharacterSelectionWindow>();
                characterSelectionWindow.ChooseButtonClicked +=
                    OnChooseButtonClicked;
                characterSelectionWindow.CancelButtonClicked +=
                    OnCancelButtonClicked;
                characterSelectionWindow.KnightSelected += OnKnightSelected;
                characterSelectionWindow.ArrowSelected += OnArrowSelected;
                characterSelectionWindow.WizardSelected += OnWizardSelected;
            }

            characterSelectionWindow.Show();
        }

        private void Awake()
        {
            SubscribeToCharacterNameEvents();
        }

        private void OnDestroy()
        {
            if (characterSelectionWindow != null)
            {
                characterSelectionWindow.ChooseButtonClicked -=
                    OnChooseButtonClicked;
                characterSelectionWindow.CancelButtonClicked -=
                    OnCancelButtonClicked;
                characterSelectionWindow.KnightSelected -= OnKnightSelected;
                characterSelectionWindow.ArrowSelected -= OnArrowSelected;
                characterSelectionWindow.WizardSelected -= OnWizardSelected;

                Destroy(characterSelectionWindow.gameObject);
            }

            UnsubscribeToCharacterNameEvents();
        }

        private void SubscribeToCharacterNameEvents()
        {
            // TODO: Use event bus system
            var characterNameController =
                FindObjectOfType<CharacterNameController>();
            if (characterNameController != null)
            {
                characterNameController.ConfirmButtonClicked +=
                    OnConfirmButtonClicked;
                characterNameController.BackButtonClicked +=
                    OnBackButtonClicked;
            }
        }

        private void UnsubscribeToCharacterNameEvents()
        {
            // TODO: Use event bus system
            var characterNameController =
                FindObjectOfType<CharacterNameController>();
            if (characterNameController != null)
            {
                characterNameController.ConfirmButtonClicked -=
                    OnConfirmButtonClicked;
                characterNameController.BackButtonClicked -=
                    OnBackButtonClicked;
            }
        }

        private void OnConfirmButtonClicked(string characterName)
        {
            uiCharacterDetails.SetCharacterName(characterName);

            CharacterChosen?.Invoke();
        }

        private void OnBackButtonClicked()
        {
            if (characterSelectionWindow != null)
            {
                characterSelectionWindow.Show();
            }
        }

        private void OnChooseButtonClicked()
        {
            // TODO: Use event bus system
            var characterNameController =
                FindObjectOfType<CharacterNameController>();
            if (characterNameController != null)
            {
                characterNameController.ShowCharacterNameWindow();
            }
        }

        private void OnCancelButtonClicked()
        {
            CharacterCancelled?.Invoke();
        }

        private void OnKnightSelected()
        {
            uiCharacterDetails.SetCharacterClass(UICharacterClass.Knight);
        }

        private void OnArrowSelected()
        {
            uiCharacterDetails.SetCharacterClass(UICharacterClass.Arrow);
        }

        private void OnWizardSelected()
        {
            uiCharacterDetails.SetCharacterClass(UICharacterClass.Wizard);
        }

        public UICharacterDetails GetCharacterDetails()
        {
            return uiCharacterDetails;
        }
    }
}
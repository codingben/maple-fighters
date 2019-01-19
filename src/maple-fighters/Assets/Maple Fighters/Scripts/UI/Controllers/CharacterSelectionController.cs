using System;
using Scripts.UI.Windows;
using Scripts.Utils;
using UI.Manager;

namespace Scripts.UI.Controllers
{
    public class CharacterSelectionController : MonoSingleton<CharacterSelectionController>
    {
        public event Action CharacterChosen;

        public event Action CharacterCancelled;

        private UiCharacterDetails uiCharacterDetails;
        private CharacterSelectionWindow characterSelectionWindow;

        protected override void OnAwake()
        {
            base.OnAwake();
            
            SubscribeToCharacterNameEvents();
        }

        protected override void OnDestroying()
        {
            base.OnDestroying();

            if (characterSelectionWindow != null)
            {
                characterSelectionWindow.ChooseButtonClicked -= OnChooseButtonClicked;
                characterSelectionWindow.CancelButtonClicked -= OnCancelButtonClicked;
                characterSelectionWindow.KnightSelected -= OnKnightSelected;
                characterSelectionWindow.ArrowSelected -= OnArrowSelected;
                characterSelectionWindow.WizardSelected -= OnWizardSelected;

                Destroy(characterSelectionWindow.gameObject);
            }

            UnsubscribeToCharacterNameEvents();
        }

        private void SubscribeToCharacterNameEvents()
        {
            CharacterNameController.GetInstance().ConfirmButtonClicked +=
                OnConfirmButtonClicked;
            CharacterNameController.GetInstance().BackButtonClicked +=
                OnBackButtonClicked;
        }

        private void UnsubscribeToCharacterNameEvents()
        {
            CharacterNameController.GetInstance().ConfirmButtonClicked -=
                OnConfirmButtonClicked;
            CharacterNameController.GetInstance().BackButtonClicked -=
                OnBackButtonClicked;
        }

        public void SetCharacterDetails(UiCharacterDetails uiCharacterDetails)
        {
            this.uiCharacterDetails = uiCharacterDetails;
        }

        public void ShowCharacterSelectionWindow()
        {
            if (characterSelectionWindow == null)
            {
                characterSelectionWindow = UIElementsCreator.GetInstance()
                    .Create<CharacterSelectionWindow>();
                characterSelectionWindow.ChooseButtonClicked += OnChooseButtonClicked;
                characterSelectionWindow.CancelButtonClicked += OnCancelButtonClicked;
                characterSelectionWindow.KnightSelected += OnKnightSelected;
                characterSelectionWindow.ArrowSelected += OnArrowSelected;
                characterSelectionWindow.WizardSelected += OnWizardSelected;
            }

            characterSelectionWindow.Show();
        }

        private void OnConfirmButtonClicked(string characterName)
        {
            uiCharacterDetails.SetCharacterName(characterName);

            CharacterChosen?.Invoke();
        }

        private void OnBackButtonClicked()
        {
            characterSelectionWindow.Show();
        }

        private void OnChooseButtonClicked()
        {
            CharacterNameController.GetInstance().ShowCharacterNameWindow();
        }

        private void OnCancelButtonClicked()
        {
            CharacterCancelled?.Invoke();
        }

        private void OnKnightSelected()
        {
            uiCharacterDetails.SetCharacterClass(UiCharacterClass.Knight);
        }

        private void OnArrowSelected()
        {
            uiCharacterDetails.SetCharacterClass(UiCharacterClass.Arrow);
        }

        private void OnWizardSelected()
        {
            uiCharacterDetails.SetCharacterClass(UiCharacterClass.Wizard);
        }

        public UiCharacterDetails GetCharacterDetails()
        {
            return uiCharacterDetails;
        }
    }
}
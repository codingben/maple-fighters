using System;
using Game.Common;
using Scripts.UI.Windows;
using Scripts.Utils;
using UI.Manager;

namespace Scripts.UI.Controllers
{
    public class CharacterSelectionController : MonoSingleton<CharacterSelectionController>
    {
        public event Action<UiCharacterDetails> CharacterChosen;

        private UiCharacterDetails characterDetails;

        private CharacterSelectionWindow characterSelectionWindow;
        private ClickableCharacterImageController clickableCharacterController;

        protected override void OnAwake()
        {
            base.OnAwake();

            clickableCharacterController = new ClickableCharacterImageController();

            SubscribeToCharacterNameEvents();
        }

        protected override void OnDestroying()
        {
            base.OnDestroying();

            if (characterSelectionWindow != null)
            {
                characterSelectionWindow.ChooseButtonClicked += OnChooseButtonClicked;
                characterSelectionWindow.CancelButtonClicked += OnCancelButtonClicked;
                characterSelectionWindow.KnightSelected += OnKnightSelected;
                characterSelectionWindow.ArrowSelected += OnArrowSelected;
                characterSelectionWindow.WizardSelected += OnWizardSelected;

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

        public void SetClickableCharacterDetails(ClickableCharacterImage characterImage, int characterIndex)
        {
            clickableCharacterController.SetCharacterImage(characterImage);
            clickableCharacterController.CharacterSelected();

            characterDetails.SetCharacterIndex((CharacterIndex)characterIndex);
        }

        public void CreateOrShowCharacterSelectionWindow()
        {
            if (characterSelectionWindow != null)
            {
                characterSelectionWindow.Show();
            }
            else
            {
                characterSelectionWindow = UIElementsCreator.GetInstance()
                    .Create<CharacterSelectionWindow>();
                characterSelectionWindow.ChooseButtonClicked += OnChooseButtonClicked;
                characterSelectionWindow.CancelButtonClicked += OnCancelButtonClicked;
                characterSelectionWindow.KnightSelected += OnKnightSelected;
                characterSelectionWindow.ArrowSelected += OnArrowSelected;
                characterSelectionWindow.WizardSelected += OnWizardSelected;
                characterSelectionWindow.Show();
            }
        }

        private void OnConfirmButtonClicked(string characterName)
        {
            characterDetails.SetCharacterName(characterName);

            CharacterChosen?.Invoke(characterDetails);
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
            clickableCharacterController.CharacterDeselected();
        }

        private void OnKnightSelected()
        {
            characterDetails.SetCharacterClass(CharacterClasses.Knight);
        }

        private void OnArrowSelected()
        {
            characterDetails.SetCharacterClass(CharacterClasses.Arrow);
        }

        private void OnWizardSelected()
        {
            characterDetails.SetCharacterClass(CharacterClasses.Wizard);
        }

        public UiCharacterDetails GetCharacterDetails()
        {
            return characterDetails;
        }
    }
}
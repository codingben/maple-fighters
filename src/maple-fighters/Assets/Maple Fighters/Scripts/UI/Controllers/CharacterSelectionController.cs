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

        [Header("Configuration")]
        [SerializeField]
        private int characterNameLength;

        private UICharacterDetails uiCharacterDetails;

        private CharacterSelectionWindow characterSelectionWindow;
        private CharacterNameWindow characterNameWindow;

        public void SetCharacterDetails(UICharacterDetails uiCharacterDetails)
        {
            this.uiCharacterDetails = uiCharacterDetails;
        }

        public UICharacterDetails GetCharacterDetails()
        {
            return uiCharacterDetails;
        }

        private void Awake()
        {
            CreateCharacterSelectionWindow();
            CreateCharacterNameWindow();
        }

        private void CreateCharacterSelectionWindow()
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

        private void CreateCharacterNameWindow()
        {
            characterNameWindow = UIElementsCreator.GetInstance()
                .Create<CharacterNameWindow>();
            characterNameWindow.ConfirmButtonClicked +=
                OnConfirmButtonClicked;
            characterNameWindow.BackButtonClicked += OnBackButtonClicked;
            characterNameWindow.NameInputFieldChanged +=
                OnNameInputFieldChanged;
        }

        private void OnDestroy()
        {
            DestroyCharacterSelectionWindow();
            DestroyCharacterNameWindow();
        }

        private void DestroyCharacterSelectionWindow()
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
        }

        private void DestroyCharacterNameWindow()
        {
            if (characterNameWindow != null)
            {
                characterNameWindow.ConfirmButtonClicked -=
                    OnConfirmButtonClicked;
                characterNameWindow.BackButtonClicked -= OnBackButtonClicked;
                characterNameWindow.NameInputFieldChanged -=
                    OnNameInputFieldChanged;

                Destroy(characterNameWindow.gameObject);
            }
        }

        private void OnNameInputFieldChanged(string characterName)
        {
            if (characterName.Length >= characterNameLength)
            {
                if (characterNameWindow != null)
                {
                    characterNameWindow.EnableConfirmButton();
                }
            }
            else
            {
                if (characterNameWindow != null)
                {
                    characterNameWindow.DisableConfirmButton();
                }
            }
        }

        private void OnConfirmButtonClicked(string characterName)
        {
            HideCharacterNameWindow();

            uiCharacterDetails.SetCharacterName(characterName);

            CharacterChosen?.Invoke();
        }

        private void OnBackButtonClicked()
        {
            HideCharacterNameWindow();
            ShowCharacterSelectionWindow();
        }

        private void OnChooseButtonClicked()
        {
            HideCharacterSelectionWindow();
            ShowCharacterNameWindow();
        }

        private void OnCancelButtonClicked()
        {
            HideCharacterSelectionWindow();

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

        private void ShowCharacterNameWindow()
        {
            if (characterNameWindow != null)
            {
                characterNameWindow.Show();
            }
        }

        private void HideCharacterNameWindow()
        {
            if (characterNameWindow != null)
            {
                characterNameWindow.Hide();
            }
        }

        public void ShowCharacterSelectionWindow()
        {
            if (characterSelectionWindow != null)
            {
                characterSelectionWindow.Show();
            }
        }

        private void HideCharacterSelectionWindow()
        {
            if (characterSelectionWindow != null)
            {
                characterSelectionWindow.Hide();
            }
        }
    }
}
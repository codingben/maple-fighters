using Scripts.UI.Models;
using Scripts.UI.Windows;
using UI.Manager;
using UnityEngine;

namespace Scripts.UI.Controllers
{
    public class CharacterSelectionController : MonoBehaviour
    {
        [Header("Configuration"), SerializeField]
        private int characterNameLength;

        private ICharacterSelectionView characterSelectionView;
        private ICharacterNameView characterNameView;

        private void Awake()
        {
            CreateAndSubscribeToCharacterSelectionWindow();
            CreateAndSubscribeToCharacterNameWindow();
        }

        private void CreateAndSubscribeToCharacterSelectionWindow()
        {
            characterSelectionView = UIElementsCreator.GetInstance()
                .Create<CharacterSelectionWindow>();
            characterSelectionView.ChooseButtonClicked +=
                OnChooseButtonClicked;
            characterSelectionView.CancelButtonClicked +=
                OnCancelButtonClicked;
            characterSelectionView.CharacterSelected += 
                OnCharacterSelected;
        }

        private void CreateAndSubscribeToCharacterNameWindow()
        {
            characterNameView = UIElementsCreator.GetInstance()
                .Create<CharacterNameWindow>();
            characterNameView.ConfirmButtonClicked +=
                OnConfirmButtonClicked;
            characterNameView.BackButtonClicked += 
                OnBackButtonClicked;
            characterNameView.NameInputFieldChanged +=
                OnNameInputFieldChanged;
        }

        private void OnDestroy()
        {
            UnsubscribeFromCharacterSelectionWindow();
            UnsubscribeFromCharacterNameWindow();
        }

        private void UnsubscribeFromCharacterSelectionWindow()
        {
            if (characterSelectionView != null)
            {
                characterSelectionView.ChooseButtonClicked -=
                    OnChooseButtonClicked;
                characterSelectionView.CancelButtonClicked -=
                    OnCancelButtonClicked;
                characterSelectionView.CharacterSelected -=
                    OnCharacterSelected;
            }
        }

        private void UnsubscribeFromCharacterNameWindow()
        {
            if (characterNameView != null)
            {
                characterNameView.ConfirmButtonClicked -=
                    OnConfirmButtonClicked;
                characterNameView.BackButtonClicked -= 
                    OnBackButtonClicked;
                characterNameView.NameInputFieldChanged -=
                    OnNameInputFieldChanged;
            }
        }

        private void OnNameInputFieldChanged(string characterName)
        {
            if (characterName.Length >= characterNameLength)
            {
                characterNameView?.EnableConfirmButton();
            }
            else
            {
                characterNameView?.DisableConfirmButton();
            }
        }

        private void OnConfirmButtonClicked(string characterName)
        {
            CharacterSelectionDetails.GetInstance().GetCharacterDetails()
                .SetCharacterName(characterName);

            // TODO: CharacterChosen
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

            // TODO: CharacterCancelled
        }

        private void OnCharacterSelected(UICharacterClass uiCharacterClass)
        {
            CharacterSelectionDetails.GetInstance().GetCharacterDetails()
                .SetCharacterClass(uiCharacterClass);
        }

        private void ShowCharacterNameWindow()
        {
            characterNameView?.Show();
        }

        private void HideCharacterNameWindow()
        {
            characterNameView?.Hide();
        }

        public void ShowCharacterSelectionWindow()
        {
            characterSelectionView?.Show();
        }

        private void HideCharacterSelectionWindow()
        {
            characterSelectionView?.Hide();
        }
    }
}
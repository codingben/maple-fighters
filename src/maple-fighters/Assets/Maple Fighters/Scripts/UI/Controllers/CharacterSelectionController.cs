using System;
using Scripts.UI.Models;
using Scripts.UI.Windows;
using UI.Manager;
using UnityEngine;

namespace Scripts.UI.Controllers
{
    public class CharacterSelectionController : MonoBehaviour, ICharacterSelectionListener
    {
        public event Action CharacterChosen;

        public event Action CharacterCancelled;

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
                if (characterNameView != null)
                {
                    characterNameView.EnableConfirmButton();
                }
            }
            else
            {
                if (characterNameView != null)
                {
                    characterNameView.DisableConfirmButton();
                }
            }
        }

        private void OnConfirmButtonClicked(string characterName)
        {
            CharacterDetails.GetInstance().SetCharacterName(characterName);

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

        private void OnCharacterSelected(UICharacterClass uiCharacterClass)
        {
            CharacterDetails.GetInstance().SetCharacterClass(uiCharacterClass);
        }

        private void ShowCharacterNameWindow()
        {
            if (characterNameView != null)
            {
                characterNameView.Show();
            }
        }

        public void HideCharacterNameWindow()
        {
            if (characterNameView != null)
            {
                characterNameView.Hide();
            }
        }

        public void ShowCharacterSelectionWindow()
        {
            if (characterSelectionView != null)
            {
                characterSelectionView.Show();
            }
        }

        private void HideCharacterSelectionWindow()
        {
            if (characterSelectionView != null)
            {
                characterSelectionView.Hide();
            }
        }
    }
}
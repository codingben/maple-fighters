using System;
using Scripts.UI.Models;
using Scripts.UI.Windows;
using UI.Manager;
using UnityEngine;

namespace Scripts.UI.Controllers
{
    public class CharacterSelectionController : MonoBehaviour
    {
        public event Action CharacterChosen;

        public event Action CharacterCancelled;

        [Header("Configuration"), SerializeField]
        private int characterNameLength;

        private ICharacterSelectionView characterSelectionView;
        private CharacterNameWindow characterNameWindow;

        private void Awake()
        {
            CreateCharacterSelectionWindow();
            CreateCharacterNameWindow();
        }

        private void CreateCharacterSelectionWindow()
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

        private void CreateCharacterNameWindow()
        {
            characterNameWindow = UIElementsCreator.GetInstance()
                .Create<CharacterNameWindow>();
            characterNameWindow.ConfirmButtonClicked +=
                OnConfirmButtonClicked;
            characterNameWindow.BackButtonClicked += 
                OnBackButtonClicked;
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

        private void DestroyCharacterNameWindow()
        {
            if (characterNameWindow != null)
            {
                characterNameWindow.ConfirmButtonClicked -=
                    OnConfirmButtonClicked;
                characterNameWindow.BackButtonClicked -= 
                    OnBackButtonClicked;
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
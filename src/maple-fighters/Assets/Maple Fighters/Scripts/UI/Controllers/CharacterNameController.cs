using System;
using Scripts.UI.Windows;
using UI.Manager;
using UnityEngine;

namespace Scripts.UI.Controllers
{
    public class CharacterNameController : MonoBehaviour
    {
        public event Action<string> ConfirmButtonClicked;

        public event Action BackButtonClicked;

        [Header("Configuration")]
        [SerializeField]
        private int characterNameLength;

        private CharacterNameWindow characterNameWindow;

        public void ShowCharacterNameWindow()
        {
            if (characterNameWindow != null)
            {
                characterNameWindow.Show();
            }
            else
            {
                characterNameWindow = UIElementsCreator.GetInstance()
                    .Create<CharacterNameWindow>();
                characterNameWindow.ConfirmButtonClicked +=
                    OnConfirmButtonClicked;
                characterNameWindow.BackButtonClicked += OnBackButtonClicked;
                characterNameWindow.NameInputFieldChanged +=
                    OnNameInputFieldChanged;
                characterNameWindow.Show();
            }
        }

        private void OnDestroy()
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

        private void OnConfirmButtonClicked(string characterName)
        {
            ConfirmButtonClicked?.Invoke(characterName);
        }

        private void OnBackButtonClicked()
        {
            if (characterNameWindow != null)
            {
                characterNameWindow.Hide();
            }

            BackButtonClicked?.Invoke();
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
    }
}
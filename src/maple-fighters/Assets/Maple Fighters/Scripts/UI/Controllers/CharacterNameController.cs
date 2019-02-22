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
        }

        public void HideCharacterNameWindow()
        {
            if (characterNameWindow != null)
            {
                characterNameWindow.Hide();
            }
        }

        private void Awake()
        {
            CreateCharacterNameWindow();
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
            DestroyCharacterNameWindow();
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

        private void OnConfirmButtonClicked(string characterName)
        {
            ConfirmButtonClicked?.Invoke(characterName);
        }

        private void OnBackButtonClicked()
        {
            HideCharacterNameWindow();

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
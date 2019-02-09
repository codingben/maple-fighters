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
    }
}
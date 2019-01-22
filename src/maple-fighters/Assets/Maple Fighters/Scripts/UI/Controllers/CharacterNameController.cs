using System;
using Scripts.UI.Windows;
using Scripts.Utils;
using UI.Manager;

namespace Scripts.UI.Controllers
{
    public class CharacterNameController : MonoSingleton<CharacterNameController>
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

        protected override void OnDestroying()
        {
            base.OnDestroying();

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
            characterNameWindow.Hide();

            BackButtonClicked?.Invoke();
        }
    }
}
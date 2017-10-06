using CommonTools.Log;
using Scripts.UI.Core;
using Scripts.UI.Windows;

namespace Scripts.UI.Controllers
{
    public enum CharacterClasses
    {
        Knight,
        Arrow,
        Wizard
    }

    public class CharactersSelectionController : MonoSingleton<CharactersSelectionController>
    {
        private CharactersSelectionWindow charactersSelectionWindow;
        private CharacterNameWindow characterNameWindow;

        private CharacterClasses chosenCharacterClass;

        public void ShowCharactersSelectionWindow()
        {
            if (charactersSelectionWindow)
            {
                charactersSelectionWindow.Show();
                return;
            }

            charactersSelectionWindow = UserInterfaceContainer.Instance.Add<CharactersSelectionWindow>();
            charactersSelectionWindow.Show();

            SubscribeToCharactersSelectionWindowEvents();
        }

        private void ShowCharacterNamwWindow()
        {
            if (characterNameWindow)
            {
                characterNameWindow.Show();
                return;
            }

            characterNameWindow = UserInterfaceContainer.Instance.Add<CharacterNameWindow>();
            characterNameWindow.Show();

            SubscribeToCharacterNameWindow();
        }

        private void OnDestroy()
        {
            if (charactersSelectionWindow != null)
            {
                UnsubscribeFromCharactersSelectionWindowEvents();
            }

            if (characterNameWindow != null)
            {
                UnsubscribeFromCharacterNameWindow();
            }
        }

        private void SubscribeToCharactersSelectionWindowEvents()
        {
            charactersSelectionWindow.CancelClicked += OnCancelClicked;
            charactersSelectionWindow.ChoosedClicked += OnChoosedClass;
            charactersSelectionWindow.KnightSelected += OnKnightSelected;
            charactersSelectionWindow.ArrowSelected += OnArrowSelected;
            charactersSelectionWindow.WizardSelected += OnWizardSelected;
            charactersSelectionWindow.Deselected += OnDeselected;
        }

        private void UnsubscribeFromCharactersSelectionWindowEvents()
        {
            charactersSelectionWindow.CancelClicked -= OnCancelClicked;
            charactersSelectionWindow.ChoosedClicked -= OnChoosedClass;
            charactersSelectionWindow.KnightSelected -= OnKnightSelected;
            charactersSelectionWindow.ArrowSelected -= OnArrowSelected;
            charactersSelectionWindow.WizardSelected -= OnWizardSelected;
            charactersSelectionWindow.Deselected -= OnDeselected;
        }

        private void SubscribeToCharacterNameWindow()
        {
            characterNameWindow.ConfirmClicked += OnConfirmClicked;
            characterNameWindow.BackClicked += OnBackClicked;
        }

        private void UnsubscribeFromCharacterNameWindow()
        {
            characterNameWindow.ConfirmClicked -= OnConfirmClicked;
            characterNameWindow.BackClicked -= OnBackClicked;
        }

        private void OnConfirmClicked(string characterName)
        {
            // TODO: Implement

            LogUtils.Log(MessageBuilder.Trace(characterName));
        }

        private void OnBackClicked()
        {
            characterNameWindow.Hide();
            charactersSelectionWindow.Show();
        }

        private void OnCancelClicked()
        {
            charactersSelectionWindow.Hide();
        }

        private void OnChoosedClass()
        {
            charactersSelectionWindow.Hide();

            ShowCharacterNamwWindow();
        }

        private void OnKnightSelected()
        {
            chosenCharacterClass = CharacterClasses.Knight;
        }

        private void OnArrowSelected()
        {
            chosenCharacterClass = CharacterClasses.Arrow;
        }

        private void OnWizardSelected()
        {
            chosenCharacterClass = CharacterClasses.Wizard;
        }

        private void OnDeselected()
        {
            // Left blank intentionally
        }
    }
}
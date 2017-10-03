using UnityEngine;

namespace Scripts.UI
{
    public enum CharacterClasses
    {
        Knight,
        Arrow,
        Wizard
    }

    public class CharactersSelectionController : MonoBehaviour
    {
        private CharactersSelectionWindow charactersSelectionWindow;
        private CharacterClasses chosenCharacterClass;

        private void Start()
        {
            charactersSelectionWindow = UserInterfaceContainer.Instance.Add<CharactersSelectionWindow>();
            charactersSelectionWindow.Show();

            SubscribeToCharactersSelectionWindowEvents();
        }

        private void OnDestroy()
        {
            UnsubscribeFromCharactersSelectionWindowEvents();
        }

        private void SubscribeToCharactersSelectionWindowEvents()
        {
            charactersSelectionWindow.Choosed += OnChoosedClass;
            charactersSelectionWindow.KnightSelected += OnKnightSelected;
            charactersSelectionWindow.ArrowSelected += OnArrowSelected;
            charactersSelectionWindow.WizardSelected += OnWizardSelected;
            charactersSelectionWindow.Deselected += OnDeselected;
        }

        private void UnsubscribeFromCharactersSelectionWindowEvents()
        {
            charactersSelectionWindow.KnightSelected -= OnKnightSelected;
            charactersSelectionWindow.ArrowSelected -= OnArrowSelected;
            charactersSelectionWindow.WizardSelected -= OnWizardSelected;
            charactersSelectionWindow.Deselected -= OnDeselected;
        }

        private void OnChoosedClass()
        {
            charactersSelectionWindow.Hide();
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
using System.Threading.Tasks;
using CommonTools.Coroutines;
using Scripts.Containers;
using Scripts.UI.Core;
using Scripts.UI.Windows;
using Shared.Game.Common;

namespace Scripts.UI.Controllers
{
    public class CharactersSelectionController : MonoSingleton<CharactersSelectionController>
    {
        private ClickableCharacter clickedCharacter;

        private CharactersSelectionWindow charactersSelectionWindow;
        private CharacterNameWindow characterNameWindow;

        private CreateCharacterRequestParameters characterRequestParameters;

        private readonly ExternalCoroutinesExecutor coroutinesExecutor = new ExternalCoroutinesExecutor();

        private void Update()
        {
            coroutinesExecutor.Update();
        }

        public void ShowCharactersSelectionWindow(ClickableCharacter clickableCharacter, int characterIndex)
        {
            clickedCharacter = clickableCharacter;
            clickedCharacter.PlayWalkAnimationAction.Invoke();

            characterRequestParameters.Index = (CharacterIndex)characterIndex;

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
                UserInterfaceContainer.Instance.Remove(charactersSelectionWindow);
            }

            if (characterNameWindow != null)
            {
                UnsubscribeFromCharacterNameWindow();
                UserInterfaceContainer.Instance.Remove(characterNameWindow);
            }
        }

        private void SubscribeToCharactersSelectionWindowEvents()
        {
            charactersSelectionWindow.ChoosedClicked += OnChoosedClass;
            charactersSelectionWindow.CancelClicked += OnCancelClicked;
            charactersSelectionWindow.KnightSelected += OnKnightSelected;
            charactersSelectionWindow.ArrowSelected += OnArrowSelected;
            charactersSelectionWindow.WizardSelected += OnWizardSelected;
            charactersSelectionWindow.Deselected += OnDeselected;
        }

        private void UnsubscribeFromCharactersSelectionWindowEvents()
        {
            charactersSelectionWindow.ChoosedClicked -= OnChoosedClass;
            charactersSelectionWindow.CancelClicked -= OnCancelClicked;
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
            characterRequestParameters.Name = characterName;

            coroutinesExecutor.StartTask(CreateCharacter);
        }

        private async Task CreateCharacter(IYield yield)
        {
            var noticeWindow = Utils.ShowNotice("Creating a new character... Please wait.", ShowCharacterNamwWindow, true);
            noticeWindow.OkButton.interactable = false;

            var responseParameters = await ServiceContainer.GameService.CreateCharacter(yield, characterRequestParameters);

            switch (responseParameters.Status)
            {
                case CharacterCreationStatus.Succeed:
                {
                    CharactersController.Instance.RecreateCharacter(GetLastCreatedCharacter());

                    noticeWindow.Message.text = "Character created successfully.";
                    noticeWindow.OkButtonClickedAction = charactersSelectionWindow.DeactiveAll;
                    noticeWindow.OkButton.interactable = true;
                    break;
                }
                case CharacterCreationStatus.Failed:
                {
                    noticeWindow.Message.text = "Failed to create a new character, please try again.";
                    noticeWindow.OkButton.interactable = true;
                    break;
                }
                case CharacterCreationStatus.NameUsed:
                {
                    noticeWindow.Message.text = "The name is already in use, choose another name.";
                    noticeWindow.OkButton.interactable = true;
                    break;
                }
                default:
                {
                    noticeWindow.Message.text = "Something went wrong, please try again.";
                    noticeWindow.OkButton.interactable = true;
                    break;
                }
            }
        }

        private void OnBackClicked()
        {
            charactersSelectionWindow.Show();
        }

        private void OnChoosedClass()
        {
            ShowCharacterNamwWindow();
        }

        private void OnCancelClicked()
        {
            clickedCharacter.PlayIdleAnimationAction.Invoke();
        }

        private void OnKnightSelected()
        {
            characterRequestParameters.CharacterClass = CharacterClasses.Knight;
        }

        private void OnArrowSelected()
        {
            characterRequestParameters.CharacterClass = CharacterClasses.Arrow;
        }

        private void OnWizardSelected()
        {
            characterRequestParameters.CharacterClass = CharacterClasses.Wizard;
        }

        private void OnDeselected()
        {
            // Left blank intentionally
        }

        private CharacterFromDatabase GetLastCreatedCharacter()
        {
            return new CharacterFromDatabase(characterRequestParameters.Name, characterRequestParameters.CharacterClass, characterRequestParameters.Index);
        }
    }
}
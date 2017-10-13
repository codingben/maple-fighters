using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CommonTools.Coroutines;
using CommonTools.Log;
using Scripts.Containers;
using Scripts.UI.Core;
using Scripts.UI.Windows;
using Shared.Game.Common;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using GameObject = UnityEngine.GameObject;

namespace Scripts.UI.Controllers
{
    public class CharactersController : MonoSingleton<CharactersController>
    {
        private const string CHARACTERS_PATH = "Characters/{0}";
        private const int MAXIMUM_CHARACTERS = 3;

        [SerializeField] private int loadSceneIndex;

        private Transform charactersParent;

        private ChooseFighterText chooseFighterText;
        private CharacterSelectionOptionsWindow characterSelectionOptionsWindow;

        private ClickableCharacter clickedCharacter;

        private readonly ClickableCharacter[] characters = { null, null, null };
        private readonly ExternalCoroutinesExecutor coroutinesExecutor = new ExternalCoroutinesExecutor();

        private void Start()
        {
            charactersParent = UserInterfaceContainer.Instance.Get<BackgroundCharactersParent>().AssertNotNull().GameObject.transform;

            ServiceContainer.GameService.Authenticated += () => coroutinesExecutor.StartTask(FetchCharactersTask);
        }

        private void OnDestroy()
        {
            RemoveCharacters();

            if (chooseFighterText != null)
            {
                UserInterfaceContainer.Instance.Remove(chooseFighterText);
            }
        }

        private void RemoveCharacters()
        {
            foreach (var clickableCharacter in characters)
            {
                if (clickableCharacter != null)
                {
                    Destroy(clickableCharacter.gameObject);
                }
            }
        }

        private void Update()
        {
            coroutinesExecutor?.Update();
        }

        private async Task FetchCharactersTask(IYield yield)
        {
            var parameters = await ServiceContainer.GameService.FetchCharacters(yield);

            if (parameters.Characters.Length == 0)
            {
                Utils.ShowNotice("Could not connect to a server.", Application.Quit, true);
                return;
            }

            var characters = new List<Character>(MAXIMUM_CHARACTERS);
            characters.AddRange(parameters.Characters.ToList());

            OnReceivedCharacters(characters);
        }

        private void OnReceivedCharacters(IReadOnlyCollection<Character> characters)
        {
            if (characters.Count > MAXIMUM_CHARACTERS)
            {
                LogUtils.Log($"There can not be more than {MAXIMUM_CHARACTERS} characters.", LogMessageType.Error);
                return;
            }

            ShowChooseFighterText();

            foreach (var character in characters)
            {
                if (character.Index == CharacterIndex.Zero)
                {
                    LogUtils.Log(MessageBuilder.Trace($"Character with name {character.Name} can not be with index Zero."));
                    continue;
                }

                CreateCharacter(character);
            }
        }

        private void CreateCharacter(Character character)
        {
            var index = (int)character.Index;
            var characterName = character.HasCharacter ? $"{character.CharacterType} {index}" : $"Sample {index}";
            var characterType = Resources.Load<GameObject>(string.Format(CHARACTERS_PATH, characterName));
            var anchoredPosition = characterType.GetComponent<RectTransform>().anchoredPosition;

            var characterGameObject = Instantiate(characterType, Vector3.zero, Quaternion.identity, charactersParent);
            characterGameObject.transform.SetAsFirstSibling();
            characterGameObject.GetComponent<RectTransform>().anchoredPosition = anchoredPosition;

            var characterComponent = characterGameObject.GetComponent<ClickableCharacter>().AssertNotNull();
            characterComponent.SetCharacter(index, character);
            characterComponent.CharacterClicked += (clickedCharacter, characterIndex) 
                => OnCharacterClicked(characterComponent, clickedCharacter, characterIndex);

            characters[(int)character.Index] = characterComponent;

            if (!character.HasCharacter)
            {
                return;
            }

            var characterNameComponent = characterGameObject.transform.GetChild(0).GetComponent<TextMeshProUGUI>().AssertNotNull();
            if (characterNameComponent != null)
            {
                characterNameComponent.text = character.Name;
            }
        }

        public void RecreateCharacter(Character character)
        {
            var index = (int)character.Index;
            characters[index].Hide();

            CreateCharacter(character);
        }

        private void OnCharacterClicked(ClickableCharacter clickableCharacter, Character character, int index)
        {
            HideChooseFighterText();
            RemoveCharacterSelectionOptionsWindowIfExists();
            ShowCharacterSelectionOptionsWindow(clickableCharacter, character, index);
        }

        private void ShowChooseFighterText()
        {
            chooseFighterText = UserInterfaceContainer.Instance.Add<ChooseFighterText>();
            chooseFighterText.Show();
        }

        private void HideChooseFighterText()
        {
            chooseFighterText.Hide();
        }

        private void RemoveCharacterSelectionOptionsWindowIfExists()
        {
            if (characterSelectionOptionsWindow != null)
            {
                characterSelectionOptionsWindow.Hide();
            }
        }

        private void ShowCharacterSelectionOptionsWindow(ClickableCharacter clickableCharacter, Character character, int index)
        {
            characterSelectionOptionsWindow = UserInterfaceContainer.Instance.Add<CharacterSelectionOptionsWindow>();
            characterSelectionOptionsWindow.StartButtonClicked += () => OnStartButtonClicked(index);
            characterSelectionOptionsWindow.CreateCharacterButtonClicked += () => OnCreateCharacterButtonClicked(clickableCharacter, index);
            characterSelectionOptionsWindow.DeleteCharacterButtonClicked += () => OnDeleteCharacterButtonClicked(index);
            characterSelectionOptionsWindow.PlayCharacterIdleAnimation += clickableCharacter.PlayIdleAnimationAction;
            characterSelectionOptionsWindow.StartButtonInteraction(character.HasCharacter);
            characterSelectionOptionsWindow.CreateCharacteButtonInteraction(!character.HasCharacter);
            characterSelectionOptionsWindow.DeleteCharacterButtonInteraction(character.HasCharacter);
            characterSelectionOptionsWindow.Show();

            clickedCharacter = clickableCharacter;
        }

        private void OnStartButtonClicked(int characterIndex)
        {
            clickedCharacter.PlayWalkAnimationAction();

            var parameters = new ValidateCharacterRequestParameters(characterIndex);
            coroutinesExecutor.StartTask((y) => ValidateCharacter(y, parameters));
        }

        private async Task ValidateCharacter(IYield yield, ValidateCharacterRequestParameters parameters)
        {
            var noticeWindow = Utils.ShowNotice("Entering to the world... Please wait.", null, true);
            noticeWindow.OkButton.interactable = false;

            var response = await ServiceContainer.GameService.ValidateCharacter(yield, parameters);

            switch (response)
            {
                case ValidateCharacterStatus.Ok:
                {
                    EnteredWorld(noticeWindow);
                    break;
                }
                case ValidateCharacterStatus.Wrong:
                {
                    noticeWindow.Message.text = "Cannot enter to the world with this character. Please try again.";
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

            if (response != ValidateCharacterStatus.Ok)
            {
                clickedCharacter.PlayIdleAnimationAction();
            }
        }

        private void EnteredWorld(NoticeWindow noticeWindow)
        {
            var screenFade = UserInterfaceContainer.Instance.Get<ScreenFade>().AssertNotNull();
            screenFade.Show(() => OnEnteredWorld(noticeWindow));
        }

        private void OnEnteredWorld(NoticeWindow noticeWindow)
        {
            UserInterfaceContainer.Instance.Remove(noticeWindow);

            SceneManager.LoadScene(loadSceneIndex, LoadSceneMode.Single);

            ServiceContainer.GameService.EnterWorld();
        }

        private void OnCreateCharacterButtonClicked(ClickableCharacter clickableCharacter, int characterIndex)
        {
            CharactersSelectionController.Instance.ShowCharactersSelectionWindow(clickableCharacter, characterIndex);
        }

        private void OnDeleteCharacterButtonClicked(int characterIndex)
        {
            var parameters = new RemoveCharacterRequestParameters(characterIndex);
            coroutinesExecutor.StartTask((y) => DeleteCharacter(y, parameters));
        }

        private async Task DeleteCharacter(IYield yield, RemoveCharacterRequestParameters parameters)
        {
            var noticeWindow = Utils.ShowNotice("Deleting a character... Please wait.", null, true);
            noticeWindow.OkButton.interactable = false;

            var responseParameters = await ServiceContainer.GameService.RemoveCharacter(yield, parameters);

            switch (responseParameters.Status)
            {
                case RemoveCharacterStatus.Succeed:
                {
                    RecreateCharacter(new Character
                    {
                        HasCharacter = false,
                        Index = (CharacterIndex)parameters.CharacterIndex
                    });

                    noticeWindow.Message.text = "Character deleted successfully.";
                    noticeWindow.OkButton.interactable = true;
                    break;
                }
                case RemoveCharacterStatus.Failed:
                {
                    noticeWindow.Message.text = "Could not remove a character. Please try again.";
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
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Character.Client.Common;
using CommonTools.Coroutines;
using CommonTools.Log;
using Scripts.Containers;
using Scripts.Services;
using Scripts.UI.Core;
using Scripts.UI.Windows;
using Scripts.World;
using Game.Common;
using Scripts.Utils;
using TMPro;
using UnityEngine;
using GameObject = UnityEngine.GameObject;

namespace Scripts.UI.Controllers
{
    public class CharactersController : MonoSingleton<CharactersController>
    {
        private const string CHARACTERS_PATH = "Characters/{0}";
        private const int MAXIMUM_CHARACTERS = 3;

        private CharacterSelectionOptionsWindow characterSelectionOptionsWindow;
        private ClickableCharacter clickedCharacter;
        private readonly ClickableCharacter[] characters = { null, null, null };
        private readonly ExternalCoroutinesExecutor coroutinesExecutor = new ExternalCoroutinesExecutor();

        private void Start()
        {
            var chooseFighterText = UserInterfaceContainer.Instance.Add<ChooseFighterText>();
            chooseFighterText.Show();

            Action getCharactersAction = () =>
            {
                coroutinesExecutor.StartTask(GetCharacters);
            };

            if (CharacterConnectionProvider.Instance.IsConnected())
            {
                getCharactersAction.Invoke();
            }
            else
            {
                CharacterConnectionProvider.Instance.Connect(onAuthorized: getCharactersAction);
            }
        }

        private void OnDestroy()
        {
            var chooseFighterText = UserInterfaceContainer.Instance.Get<ChooseFighterText>().AssertNotNull();
            UserInterfaceContainer.Instance.Remove(chooseFighterText);

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
            coroutinesExecutor.Update();
        }

        private async Task GetCharacters(IYield yield)
        {
            var parameters = await ServiceContainer.CharacterService.GetCharacters(yield);
            if (parameters.Characters.Length == 0)
            {
                Utils.ShowNotice("Failed to get characters, unfortunately.", LoadedObjects.DestroyAll);
                return;
            }

            var characters = new List<CharacterFromDatabaseParameters>(MAXIMUM_CHARACTERS);
            characters.AddRange(parameters.Characters.ToList());

            OnReceivedCharacters(characters);
        }

        private void OnReceivedCharacters(IEnumerable<CharacterFromDatabaseParameters> characters)
        {
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

        private void CreateCharacter(CharacterFromDatabaseParameters character)
        {
            var index = (int)character.Index;
            var characterName = character.HasCharacter ? $"{character.CharacterType} {index}" : $"Sample {index}";
            var characterType = Resources.Load<GameObject>(string.Format(CHARACTERS_PATH, characterName));
            var anchoredPosition = characterType.GetComponent<RectTransform>().anchoredPosition;

            var charactersParent = UserInterfaceContainer.Instance.Get<BackgroundCharactersParent>().AssertNotNull();
            var characterGameObject = Instantiate(characterType, Vector3.zero, Quaternion.identity, charactersParent.gameObject.transform);
            characterGameObject.transform.SetAsFirstSibling();
            characterGameObject.GetComponent<RectTransform>().anchoredPosition = anchoredPosition;

            var characterComponent = characterGameObject.GetComponent<ClickableCharacter>().AssertNotNull();
            characterComponent.SetCharacter(index, character);
            characterComponent.CharacterClicked += (clickedCharacter, characterIndex) => OnCharacterClicked(characterComponent, clickedCharacter, characterIndex);

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

        public void RecreateCharacter(CharacterFromDatabaseParameters character)
        {
            var index = (int)character.Index;
            characters[index].Hide();

            CreateCharacter(character);
        }

        private void OnCharacterClicked(ClickableCharacter clickableCharacter, CharacterFromDatabaseParameters character, int index)
        {
            var chooseFighterText = UserInterfaceContainer.Instance.Get<ChooseFighterText>().AssertNotNull();
            chooseFighterText.Hide();

            if (characterSelectionOptionsWindow != null)
            {
                characterSelectionOptionsWindow.Hide();
            }

            ShowCharacterSelectionOptionsWindow(clickableCharacter, character, index);
        }

        private void ShowCharacterSelectionOptionsWindow(ClickableCharacter clickableCharacter, CharacterFromDatabaseParameters character, int index)
        {
            characterSelectionOptionsWindow = UserInterfaceContainer.Instance.Add<CharacterSelectionOptionsWindow>();
            characterSelectionOptionsWindow.StartButtonClicked += () => OnStartButtonClicked(index);
            characterSelectionOptionsWindow.CreateCharacterButtonClicked += () => OnCreateCharacterButtonClicked(clickableCharacter, index);
            characterSelectionOptionsWindow.DeleteCharacterButtonClicked += () => OnDeleteCharacterButtonClicked(index);
            characterSelectionOptionsWindow.PlayCharacterIdleAnimation += clickableCharacter.PlayIdleAnimationAction;
            characterSelectionOptionsWindow.StartButtonInteraction(character.HasCharacter);
            characterSelectionOptionsWindow.CreateCharacterButtonInteraction(!character.HasCharacter);
            characterSelectionOptionsWindow.DeleteCharacterButtonInteraction(character.HasCharacter);
            characterSelectionOptionsWindow.Show();

            clickedCharacter = clickableCharacter;
            clickedCharacter.PlayWalkAnimationAction();
        }

        private void OnStartButtonClicked(int characterIndex)
        {
            clickedCharacter.PlayWalkAnimationAction();

            var noticeWindow = Utils.ShowNotice("Entering to the world... Please wait.", null, true);
            noticeWindow.OkButton.interactable = false;

            GameConnectionProvider.Instance.Connect(onAuthorized: () => 
            {
                var parameters = new ValidateCharacterRequestParameters(characterIndex);
                coroutinesExecutor.StartTask((y) => ValidateCharacter(y, parameters));
            });
        }

        private async Task ValidateCharacter(IYield yield, ValidateCharacterRequestParameters parameters)
        {
            var response = await ServiceContainer.GameService.ValidateCharacter(yield, parameters);
            switch (response)
            {
                case CharacterValidationStatus.Ok:
                {
                    EnteredWorld();
                    break;
                }
                case CharacterValidationStatus.Wrong:
                {
                    var noticeWindow = UserInterfaceContainer.Instance.Get<NoticeWindow>().AssertNotNull();
                    noticeWindow.Message.text = "Can not enter to the world with this character. Please try again.";
                    noticeWindow.OkButton.interactable = true;
                    break;
                }
                default:
                {
                    var noticeWindow = UserInterfaceContainer.Instance.Get<NoticeWindow>().AssertNotNull();
                    noticeWindow.Message.text = "Something went wrong, please try again.";
                    noticeWindow.OkButton.interactable = true;
                    break;
                }
            }

            if (response != CharacterValidationStatus.Ok)
            {
                clickedCharacter.PlayIdleAnimationAction();
            }
        }

        private void EnteredWorld()
        {
            var screenFade = UserInterfaceContainer.Instance.Get<ScreenFade>().AssertNotNull();
            screenFade.Show(OnEnteredWorld);
        }

        private void OnEnteredWorld()
        {
            CharacterConnectionProvider.Instance.Dispose();

            var noticeWindow = UserInterfaceContainer.Instance.Get<NoticeWindow>().AssertNotNull();
            UserInterfaceContainer.Instance.Remove(noticeWindow);

            GameScenesController.Instance.LoadScene(Maps.Map_1);
        }

        private void OnCreateCharacterButtonClicked(ClickableCharacter clickableCharacter, int characterIndex)
        {
            CharactersSelectionController.Instance.ShowCharactersSelectionWindow(clickableCharacter, characterIndex);
        }

        private void OnDeleteCharacterButtonClicked(int characterIndex)
        {
            Action deleteCharacterAction = () =>
            {
                var parameters = new RemoveCharacterRequestParameters(characterIndex);
                coroutinesExecutor.StartTask((y) => DeleteCharacter(y, parameters));
            };

            if (CharacterConnectionProvider.Instance.IsConnected())
            {
                deleteCharacterAction.Invoke();
            }
            else
            {
                CharacterConnectionProvider.Instance.Connect(onAuthorized: deleteCharacterAction);
            }
        }

        private async Task DeleteCharacter(IYield yield, RemoveCharacterRequestParameters parameters)
        {
            var noticeWindow = Utils.ShowNotice("Deleting a character... Please wait.", null, true);
            noticeWindow.OkButton.interactable = false;

            var responseParameters = await ServiceContainer.CharacterService.RemoveCharacter(yield, parameters);
            switch (responseParameters.Status)
            {
                case RemoveCharacterStatus.Succeed:
                {
                    RecreateCharacter(new CharacterFromDatabaseParameters
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
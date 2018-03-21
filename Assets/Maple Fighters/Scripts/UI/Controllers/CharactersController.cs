using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CommonTools.Coroutines;
using CommonTools.Log;
using CommunicationHelper;
using Scripts.Containers;
using Scripts.UI.Core;
using Scripts.UI.Windows;
using Game.Common;
using Scripts.Utils;
using Scripts.World;
using TMPro;
using UnityEngine;

namespace Scripts.UI.Controllers
{
    public class CharactersController : MonoSingleton<CharactersController>
    {
        private readonly ClickableCharacter[] characters = { null, null, null };
        private readonly ExternalCoroutinesExecutor coroutinesExecutor = new ExternalCoroutinesExecutor();

        private void Start()
        {
            var chooseFighterText = UserInterfaceContainer.Instance.Add<ChooseFighterText>();
            chooseFighterText.Show();

            coroutinesExecutor.StartTask(GetCharacters);
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

            coroutinesExecutor.Dispose();
        }

        private void Update()
        {
            coroutinesExecutor.Update();
        }

        private async Task GetCharacters(IYield yield)
        {
            ServiceContainer.GameService.SetServerPeerHandler<CharacterOperations, EmptyEventCode>();

            var parameters = await ServiceContainer.CharacterService.GetCharacters(yield);
            if (parameters.Characters == null)
            {
                LogUtils.Log(MessageBuilder.Trace("Failed to get characters."));
                LoadedObjects.DestroyAll();
                return;
            }

            OnReceivedCharacters(parameters);
        }

        private void OnReceivedCharacters(GetCharactersResponseParameters parameters)
        {
            const int CHARACTERS_COUNT = 3;

            var characters = new List<CharacterParameters>(CHARACTERS_COUNT);
            characters.AddRange(parameters.Characters);

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

        private void CreateCharacter(CharacterParameters character)
        {
            const string CHARACTERS_RESOURCES_PATH = "Characters/{0}";

            var index = (int)character.Index;
            var characterName = character.HasCharacter ? $"{character.CharacterType} {index}" : $"Sample {index}";
            var characterType = Resources.Load<GameObject>(string.Format(CHARACTERS_RESOURCES_PATH, characterName));
            var anchoredPosition = characterType.GetComponent<RectTransform>().anchoredPosition;

            var charactersParent = UserInterfaceContainer.Instance.Get<BackgroundCharactersParent>().AssertNotNull();
            var characterGameObject = Instantiate(characterType, Vector3.zero, Quaternion.identity, charactersParent.gameObject.transform);
            characterGameObject.transform.SetAsFirstSibling();
            characterGameObject.GetComponent<RectTransform>().anchoredPosition = anchoredPosition;

            // TODO: Refactor. Add<ClickableCharacter>

            var characterComponent = characterGameObject.GetComponent<ClickableCharacter>().AssertNotNull();
            characterComponent.SetCharacter(index, character);
            characterComponent.CharacterClicked += OnCharacterClicked;

            characters[(int)character.Index] = characterComponent;

            if (character.HasCharacter)
            {
                var characterNameComponent = characterGameObject.transform.GetChild(0)?.GetComponent<TextMeshProUGUI>().AssertNotNull();
                if (characterNameComponent != null)
                {
                    characterNameComponent.text = character.Name;
                }
            }
        }

        public void RecreateCharacter(CharacterParameters character)
        {
            var index = (int)character.Index;
            characters[index].Hide();

            CreateCharacter(character);
        }

        private void OnCharacterClicked(CharacterParameters character, int characterIndex)
        {
            var chooseFighterText = UserInterfaceContainer.Instance.Get<ChooseFighterText>().AssertNotNull();
            chooseFighterText.Hide();

            var characterSelectionOptionsWindow = UserInterfaceContainer.Instance.Get<CharacterSelectionOptionsWindow>();
            if (characterSelectionOptionsWindow != null)
            {
                Action onCharacterSelectionOptionsWindowDisappeared = delegate
                {
                    characterSelectionOptionsWindow.Hide();

                    var clickableCharacter = characters[characterIndex];
                    ShowCharacterSelectionOptionsWindow(clickableCharacter, character, characterIndex);
                };

                characterSelectionOptionsWindow.Hide(onCharacterSelectionOptionsWindowDisappeared);
            }
            else
            {
                var clickableCharacter = characters[characterIndex];
                ShowCharacterSelectionOptionsWindow(clickableCharacter, character, characterIndex);
            }
            
            PlayWalkCharacterAnimation(characterIndex);
        }

        private void ShowCharacterSelectionOptionsWindow(ClickableCharacter clickableCharacter, CharacterParameters character, int characterIndex)
        {
            var characterSelectionOptionsWindow = UserInterfaceContainer.Instance.Add<CharacterSelectionOptionsWindow>().AssertNotNull();
            characterSelectionOptionsWindow.StartButtonClicked += () => OnStartButtonClicked(characterIndex);
            characterSelectionOptionsWindow.CreateCharacterButtonClicked += () => OnCreateCharacterButtonClicked(clickableCharacter, characterIndex);
            characterSelectionOptionsWindow.DeleteCharacterButtonClicked += () => OnDeleteCharacterButtonClicked(characterIndex);
            characterSelectionOptionsWindow.PlayCharacterIdleAnimation += () =>  PlayIdleCharacterAnimation(characterIndex);
            characterSelectionOptionsWindow.StartButtonInteraction(character.HasCharacter);
            characterSelectionOptionsWindow.CreateCharacterButtonInteraction(!character.HasCharacter);
            characterSelectionOptionsWindow.DeleteCharacterButtonInteraction(character.HasCharacter);
            characterSelectionOptionsWindow.Show();
        }

        private void PlayIdleCharacterAnimation(int characterIndex)
        {
            var clickedCharacter = characters[characterIndex];
            clickedCharacter.PlayIdleAnimationAction();
        }

        private void PlayWalkCharacterAnimation(int characterIndex)
        {
            var clickedCharacter = characters[characterIndex];
            clickedCharacter.PlayWalkAnimationAction();
        }

        private void OnStartButtonClicked(int characterIndex)
        {
            var clickedCharacter = characters[characterIndex];
            clickedCharacter.PlayWalkAnimationAction();

            var noticeWindow = Utils.ShowNotice("Entering to the world... Please wait.", null, true);
            noticeWindow.OkButton.interactable = false;

            var parameters = new ValidateCharacterRequestParameters(characterIndex);
            coroutinesExecutor.StartTask((y) => ValidateCharacter(y, parameters));
        }

        private async Task ValidateCharacter(IYield yield, ValidateCharacterRequestParameters parameters)
        {
            var responseParameters = await ServiceContainer.CharacterService.ValidateCharacter(yield, parameters);
            switch (responseParameters)
            {
                case CharacterValidationStatus.Ok:
                {
                    OnCharacterValidated();
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

            if (responseParameters != CharacterValidationStatus.Ok)
            {
                var index = parameters.CharacterIndex;
                PlayIdleCharacterAnimation(index);
            }
        }

        private void OnCharacterValidated()
        {
            Action enterScene = delegate 
            {
                var noticeWindow = UserInterfaceContainer.Instance.Get<NoticeWindow>().AssertNotNull();
                UserInterfaceContainer.Instance.Remove(noticeWindow);

                GameScenesController.Instance.LoadScene(Maps.Map_1);
            };

            var screenFade = UserInterfaceContainer.Instance.Get<ScreenFade>().AssertNotNull();
            screenFade.Show(onFinished: enterScene);
        }

        private void OnCreateCharacterButtonClicked(ClickableCharacter clickableCharacter, int characterIndex)
        {
            CharactersSelectionController.Instance.ShowCharactersSelectionWindow(clickableCharacter, characterIndex);
        }

        private void OnDeleteCharacterButtonClicked(int characterIndex)
        {
            var noticeWindow = Utils.ShowNotice("Deleting a character... Please wait.", null, true);
            noticeWindow.OkButton.interactable = false;

            var parameters = new RemoveCharacterRequestParameters(characterIndex);
            coroutinesExecutor.StartTask((y) => DeleteCharacter(y, parameters));
        }

        private async Task DeleteCharacter(IYield yield, RemoveCharacterRequestParameters parameters)
        {
            var responseParameters = await ServiceContainer.CharacterService.RemoveCharacter(yield, parameters);
            switch (responseParameters.Status)
            {
                case RemoveCharacterStatus.Succeed:
                {
                    var characterParameters = new CharacterParameters{HasCharacter = false, Index = (CharacterIndex)parameters.CharacterIndex};
                    RecreateCharacter(characterParameters);

                    var noticeWindow = UserInterfaceContainer.Instance.Get<NoticeWindow>().AssertNotNull();
                    noticeWindow.Message.text = "Character deleted successfully.";
                    noticeWindow.OkButton.interactable = true;
                    break;
                }
                case RemoveCharacterStatus.Failed:
                {
                    var noticeWindow = UserInterfaceContainer.Instance.Get<NoticeWindow>().AssertNotNull();
                    noticeWindow.Message.text = "Could not remove a character. Please try again.";
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
        }
    }
}
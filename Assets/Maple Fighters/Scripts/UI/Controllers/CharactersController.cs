using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CommonTools.Coroutines;
using CommonTools.Log;
using Game.Common;
using Scripts.Containers;
using Scripts.Services;
using Scripts.UI.Core;
using Scripts.UI.Windows;
using Scripts.Utils;
using Scripts.World;
using TMPro;
using UnityEngine;

namespace Scripts.UI.Controllers
{
    public class CharactersController : MonoSingleton<CharactersController>
    {
        private readonly ClickableCharacter[] characters = { null, null, null };
        private ExternalCoroutinesExecutor coroutinesExecutor;

        protected override void OnAwake()
        {
            base.OnAwake();

            coroutinesExecutor = new ExternalCoroutinesExecutor();
        }

        private void Start()
        {
            CreateChooseFighterTextUI();

            coroutinesExecutor.StartTask(GetCharacters, exception => ServiceConnectionProviderUtils.OnOperationFailed());
        }

        protected override void OnDestroying()
        {
            base.OnDestroying();

            coroutinesExecutor.Dispose();

            RemoveChooseFighterTextUI();
            RemoveAllClickableCharacters();
        }

        private void CreateChooseFighterTextUI()
        {
            var chooseFighterText = UserInterfaceContainer.GetInstance().Add<ChooseFighterText>();
            chooseFighterText.Show();
        }

        private void RemoveChooseFighterTextUI()
        {
            var chooseFighterText = UserInterfaceContainer.GetInstance()?.Get<ChooseFighterText>().AssertNotNull();
            UserInterfaceContainer.GetInstance()?.Remove(chooseFighterText);
        }

        private void RemoveAllClickableCharacters()
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
            coroutinesExecutor.Update();
        }

        private async Task GetCharacters(IYield yield)
        {
            var characterService = ServiceContainer.GameService.GetPeerLogic<ICharacterPeerLogicAPI>().AssertNotNull();
            var parameters = await characterService.GetCharacters(yield);
            if (parameters.Characters == null)
            {
                throw new Exception("Failed to get characters.");
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

                LogUtils.Log($"Character Name: {character.Name} Character Index: {character.Index} Has Character: {character.HasCharacter}", LogMessageType.Warning);

                CreateCharacter(character);
            }
        }

        private void CreateCharacter(CharacterParameters character)
        {
            var index = (int)character.Index;
            var characterGameObject = CreateCharacterGameObject(character);
            var characterComponent = characterGameObject.GetComponent<ClickableCharacter>().AssertNotNull();
            characterComponent.SetCharacter(index, character);
            characterComponent.CharacterClicked += OnCharacterClicked;

            characters[index] = characterComponent;
        }

        public void RecreateCharacter(CharacterParameters character)
        {
            var index = (int)character.Index;
            characters[index].Hide();

            CreateCharacter(character);
        }

        private void OnCharacterClicked(CharacterParameters character, int characterIndex)
        {
            var chooseFighterText = UserInterfaceContainer.GetInstance().Get<ChooseFighterText>().AssertNotNull();
            chooseFighterText.Hide();

            var characterSelectionOptionsWindow = UserInterfaceContainer.GetInstance().Get<CharacterSelectionOptionsWindow>();
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
            var characterSelectionOptionsWindow = UserInterfaceContainer.GetInstance().Add<CharacterSelectionOptionsWindow>().AssertNotNull();
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
            coroutinesExecutor.StartTask((y) => ValidateCharacter(y, parameters), exception => ServiceConnectionProviderUtils.OnOperationFailed());
        }

        private async Task ValidateCharacter(IYield yield, ValidateCharacterRequestParameters parameters)
        {
            var characterPeerLogic = ServiceContainer.GameService.GetPeerLogic<ICharacterPeerLogicAPI>().AssertNotNull();
            var responseParameters = await characterPeerLogic.ValidateCharacter(yield, parameters);
            switch (responseParameters.Status)
            {
                case CharacterValidationStatus.Ok:
                {
                    var map = responseParameters.Map;
                    OnCharacterValidated(map);
                    break;
                }
                case CharacterValidationStatus.Wrong:
                {
                    var noticeWindow = UserInterfaceContainer.GetInstance().Get<NoticeWindow>().AssertNotNull();
                    noticeWindow.Message.text = "Can not enter to the world with this character. Please try again.";
                    noticeWindow.OkButton.interactable = true;
                    break;
                }
                default:
                {
                    var noticeWindow = UserInterfaceContainer.GetInstance().Get<NoticeWindow>().AssertNotNull();
                    noticeWindow.Message.text = "Something went wrong, please try again.";
                    noticeWindow.OkButton.interactable = true;
                    break;
                }
            }

            if (responseParameters.Status != CharacterValidationStatus.Ok)
            {
                var index = parameters.CharacterIndex;
                PlayIdleCharacterAnimation(index);
            }
        }

        private void OnCharacterValidated(Maps map)
        {
            Action enterScene = delegate 
            {
                var noticeWindow = UserInterfaceContainer.GetInstance().Get<NoticeWindow>().AssertNotNull();
                UserInterfaceContainer.GetInstance()?.Remove(noticeWindow);

                GameScenesController.GetInstance().LoadScene(map);
            };

            var screenFade = UserInterfaceContainer.GetInstance().Get<ScreenFade>().AssertNotNull();
            screenFade.Show(onFinished: enterScene);
        }

        private void OnCreateCharacterButtonClicked(ClickableCharacter clickableCharacter, int characterIndex)
        {
            CharactersSelectionController.GetInstance().ShowCharactersSelectionWindow(clickableCharacter, characterIndex);
        }

        private void OnDeleteCharacterButtonClicked(int characterIndex)
        {
            var noticeWindow = Utils.ShowNotice("Deleting a character... Please wait.", null, true);
            noticeWindow.OkButton.interactable = false;

            var parameters = new RemoveCharacterRequestParameters(characterIndex);
            coroutinesExecutor.StartTask((y) => DeleteCharacter(y, parameters), exception => ServiceConnectionProviderUtils.OnOperationFailed());
        }

        private async Task DeleteCharacter(IYield yield, RemoveCharacterRequestParameters parameters)
        {
            var characterPeerLogic = ServiceContainer.GameService.GetPeerLogic<ICharacterPeerLogicAPI>().AssertNotNull();
            var responseParameters = await characterPeerLogic.RemoveCharacter(yield, parameters);
            switch (responseParameters.Status)
            {
                case RemoveCharacterStatus.Succeed:
                {
                    var characterParameters = new CharacterParameters{HasCharacter = false, Index = (CharacterIndex)parameters.CharacterIndex};
                    RecreateCharacter(characterParameters);

                    var noticeWindow = UserInterfaceContainer.GetInstance().Get<NoticeWindow>().AssertNotNull();
                    noticeWindow.Message.text = "Character deleted successfully.";
                    noticeWindow.OkButton.interactable = true;
                    break;
                }
                case RemoveCharacterStatus.Failed:
                {
                    var noticeWindow = UserInterfaceContainer.GetInstance().Get<NoticeWindow>().AssertNotNull();
                    noticeWindow.Message.text = "Could not remove a character. Please try again.";
                    noticeWindow.OkButton.interactable = true;
                    break;
                }
                default:
                {
                    var noticeWindow = UserInterfaceContainer.GetInstance().Get<NoticeWindow>().AssertNotNull();
                    noticeWindow.Message.text = "Something went wrong, please try again.";
                    noticeWindow.OkButton.interactable = true;
                    break;
                }
            }
        }

        private GameObject CreateCharacterGameObject(CharacterParameters character)
        {
            const string CHARACTERS_RESOURCES_PATH = "Characters/{0}";

            var index = (int)character.Index;
            var characterName = character.HasCharacter ? $"{character.CharacterType} {index}" : $"Sample {index}";
            var characterType = Resources.Load<GameObject>(string.Format(CHARACTERS_RESOURCES_PATH, characterName));
            var anchoredPosition = characterType.GetComponent<RectTransform>().anchoredPosition;

            var charactersParent = UserInterfaceContainer.GetInstance().Get<BackgroundCharacters>().AssertNotNull();
            var characterGameObject = Instantiate(characterType, Vector3.zero, Quaternion.identity, charactersParent.gameObject.transform);
            characterGameObject.transform.SetAsFirstSibling();
            characterGameObject.GetComponent<RectTransform>().anchoredPosition = anchoredPosition;

            if (character.HasCharacter)
            {
                var characterNameComponent = characterGameObject.transform.GetChild(0).AssertNotNull("Could not find character name transform.")
                    .GetComponent<TextMeshProUGUI>().AssertNotNull();
                if (characterNameComponent != null)
                {
                    characterNameComponent.text = character.Name;
                }
            }
            return characterGameObject;
        }
    }
}
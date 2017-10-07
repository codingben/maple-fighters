using System;
using System.Collections.Generic;
using CommonTools.Log;
using Scripts.UI.Core;
using Scripts.UI.Windows;
using TMPro;
using UnityEngine;

namespace Scripts.UI.Controllers
{
    public class CharactersCreationController : MonoBehaviour
    {
        private const string CHARACTERS_PATH = "Characters/{0}";

        private Transform charactersParent;
        private CharacterSelectionOptionsWindow characterSelectionOptionsWindow;

        private void Start()
        {
            // TODO: Subscribe to an event to get a data from the server.

            charactersParent = UserInterfaceContainer.Instance.Get<BackgroundCharactersParent>().AssertNotNull().GameObject.transform;

            var characters = new [] { new Character(CharacterClasses.Arrow, "Stephen"), new Character(CharacterClasses.Knight, "Stephen"), new Character(CharacterClasses.Wizard, "Ronald") };
            OnReceivedCharacters(characters);
        }

        private void OnReceivedCharacters(IList<Character> characters)
        {
            if (characters.Count > 3)
            {
                throw new Exception("There can not be more than 3 characters.");
            }

            for (var i = 0; i < characters.Count; i++)
            {
                var character = characters[i];
                CreateCharacter(characters[i] != null ? character : null, i);
            }
        }

        private void CreateCharacter(Character character = null, int index = 0)
        {
            var characterName = character?.CharacterType.ToString() ?? $"Sample {index}";
            var characterType = Resources.Load<GameObject>(string.Format(CHARACTERS_PATH, characterName));
            var anchoredPosition = characterType.GetComponent<RectTransform>().anchoredPosition;

            var characterGameObject = Instantiate(characterType, Vector3.zero, Quaternion.identity, charactersParent);
            characterGameObject.transform.SetAsFirstSibling();
            characterGameObject.GetComponent<RectTransform>().anchoredPosition = anchoredPosition;

            var characterComponent = characterGameObject.GetComponent<ClickableCharacter>().AssertNotNull();
            characterComponent.SetCharacter(index, character);
            characterComponent.CharacterClicked += OnCharacterClicked;

            if (character == null)
            {
                return;
            }

            var characterNameComponent = characterGameObject.transform.GetChild(0).GetComponent<TextMeshProUGUI>().AssertNotNull();
            if (characterNameComponent != null)
            {
                characterNameComponent.text = character.Name;
            }
        }

        private void OnCharacterClicked(Character character, int index)
        {
            RemoveCharacterSelectionOptionsWindowIfExists();
            ShowCharacterSelectionOptionsWindow(character, index);
        }

        private void RemoveCharacterSelectionOptionsWindowIfExists()
        {
            if (characterSelectionOptionsWindow != null)
            {
                UserInterfaceContainer.Instance.Remove(characterSelectionOptionsWindow);
            }
        }

        private void ShowCharacterSelectionOptionsWindow(Character character, int index)
        {
            characterSelectionOptionsWindow = UserInterfaceContainer.Instance.Add<CharacterSelectionOptionsWindow>();
            characterSelectionOptionsWindow.StartButtonClicked += () => OnStartButtonClicked(index);
            characterSelectionOptionsWindow.CreateCharacterButtonClicked += () => OnCreateCharacterButtonClicked(index);
            characterSelectionOptionsWindow.DeleteCharacterButtonClicked += () => DeleteCharacterButtonClicked(index);
            characterSelectionOptionsWindow.StartButtonInteraction(character != null);
            characterSelectionOptionsWindow.CreateCharacteButtonInteraction(character == null);
            characterSelectionOptionsWindow.DeleteCharacterButtonInteraction(character != null);
            characterSelectionOptionsWindow.Show();
        }

        private void OnStartButtonClicked(int characterIndex)
        {
            // TODO: Implement
        }

        private void OnCreateCharacterButtonClicked(int characterIndex)
        {
            // TODO: Implement

            characterSelectionOptionsWindow.Hide(RemoveCharacterSelectionOptionsWindowIfExists);

            CharactersSelectionController.Instance.ShowCharactersSelectionWindow();
        }

        private void DeleteCharacterButtonClicked(int characterIndex)
        {
            // TODO: Implement
        }
    }

    public class Character
    {
        public readonly CharacterClasses CharacterType;
        public readonly string Name;

        public Character(CharacterClasses characterType, string name)
        {
            CharacterType = characterType;
            Name = name;
        }
    }
}
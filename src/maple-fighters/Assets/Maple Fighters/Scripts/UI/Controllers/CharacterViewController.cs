using System;
using Scripts.UI.Models;
using Scripts.UI.Windows;
using UI.Manager;
using UnityEngine;
using UIManagerUtils = UI.Manager.Utils;

namespace Scripts.UI.Controllers
{
    public class CharacterViewController : MonoBehaviour, ICharacterViewListener
    {
        public event Action<int> CharacterStarted;

        public event Action<IClickableCharacterView> CharacterSelected;

        public event Action<int> CharacterRemoved;

        private ClickableCharacterImageCollection characterImageCollection;
        
        private ICharacterView characterView;
        private ICharacterSelectionOptionsView characterSelectionOptionsView;

        private void Awake()
        {
            characterImageCollection = new ClickableCharacterImageCollection();

            CreateCharacterView();
            CreateAndSubscribeToCharacterSelectionOptionsWindow();
        }

        private void CreateCharacterView()
        {
            characterView = UIElementsCreator.GetInstance()
                .Create<CharacterView>(UILayer.Background, UIIndex.End);
        }

        private void CreateAndSubscribeToCharacterSelectionOptionsWindow()
        {
            characterSelectionOptionsView = UIElementsCreator.GetInstance()
                .Create<CharacterSelectionOptionsWindow>();
            characterSelectionOptionsView.StartButtonClicked +=
                OnStartButtonClicked;
            characterSelectionOptionsView.CreateCharacterButtonClicked +=
                OnCreateCharacterButtonClicked;
            characterSelectionOptionsView.DeleteCharacterButtonClicked +=
                OnDeleteCharacterButtonClicked;
        }

        private void OnDestroy()
        {
            UnsubscribeFromCharacterImages();
            UnsubscribeFromCharacterSelectionOptionsWindow();
        }

        private void UnsubscribeFromCharacterImages()
        {
            var characterImages =
                characterImageCollection.GetClickableCharacterViews();
            foreach (var characterImage in characterImages)
            {
                if (characterImage != null)
                {
                    characterImage.CharacterClicked -= OnCharacterClicked;
                }
            }
        }

        private void UnsubscribeFromCharacterSelectionOptionsWindow()
        {
            if (characterSelectionOptionsView != null)
            {
                characterSelectionOptionsView.StartButtonClicked -=
                    OnStartButtonClicked;
                characterSelectionOptionsView.CreateCharacterButtonClicked -=
                    OnCreateCharacterButtonClicked;
                characterSelectionOptionsView.DeleteCharacterButtonClicked -=
                    OnDeleteCharacterButtonClicked;
            }
        }

        public void CreateCharacter()
        {
            var characterGameObject = 
                UIManagerUtils.LoadAndCreateGameObject(GetCharacterPath());
            if (characterGameObject != null)
            {
                var clickableCharacterView =
                    GetClickableCharacterView(characterGameObject);
                var characterIndex =
                    CharacterSelectionDetails.GetInstance().GetCharacterIndex();

                characterImageCollection
                    .SetCharacterView(characterIndex, clickableCharacterView);

                AttachCharacterToView(characterGameObject);
            }
        }

        private IClickableCharacterView GetClickableCharacterView(
            GameObject characterGameObject)
        {
            var characterImage =
                characterGameObject.GetComponent<ClickableCharacterImage>();
            var characterName =
                CharacterSelectionDetails.GetInstance().GetCharacterName();
            var characterClass =
                CharacterSelectionDetails.GetInstance().GetCharacterClass();

            characterImage.CharacterClass = characterClass;
            characterImage.CharacterName = characterName;
            characterImage.CharacterClicked += OnCharacterClicked;

            return characterImage;
        }

        private void AttachCharacterToView(GameObject characterGameObject)
        {
            if (characterView != null)
            {
                characterGameObject.transform.SetParent(
                    characterView.Transform);
                characterGameObject.transform.SetAsFirstSibling();
            }
        }

        private void ShowCharacterSelectionOptionsWindow()
        {
            if (characterSelectionOptionsView != null)
            {
                var hasCharacter =
                    CharacterSelectionDetails.GetInstance().HasCharacter();

                characterSelectionOptionsView
                    .EnableOrDisableStartButton(hasCharacter);

                characterSelectionOptionsView
                    .EnableOrDisableCreateCharacterButton(!hasCharacter);

                characterSelectionOptionsView
                    .EnableOrDisableDeleteCharacterButton(hasCharacter);

                characterSelectionOptionsView.Show();
            }
        }

        private void OnCharacterClicked(UICharacterClass uiCharacterClass)
        {
            CharacterSelectionDetails.GetInstance().SetCharacterClass(uiCharacterClass);

            ShowCharacterSelectionOptionsWindow();

            var characterIndex =
                CharacterSelectionDetails.GetInstance().GetCharacterIndex();
            var characterImage =
                characterImageCollection.GetCharacterView(characterIndex);

            CharacterSelected?.Invoke(characterImage);
        }
        
        private void OnStartButtonClicked()
        {
            var characterIndex =
                CharacterSelectionDetails.GetInstance().GetCharacterIndex();

            CharacterStarted?.Invoke((int)characterIndex);
        }

        private void OnCreateCharacterButtonClicked()
        {
            // TODO: Use event bus system
            var characterSelectionController =
                FindObjectOfType<CharacterSelectionController>();
            if (characterSelectionController != null)
            {
                characterSelectionController.ShowCharacterSelectionWindow();
            }
        }

        private void OnDeleteCharacterButtonClicked()
        {
            var characterIndex =
                CharacterSelectionDetails.GetInstance().GetCharacterIndex();

            CharacterRemoved?.Invoke((int)characterIndex);
        }

        private string GetCharacterPath()
        {
            const string CharactersPath = "Characters/{0}";

            var characterIndex =
                CharacterSelectionDetails.GetInstance().GetCharacterIndex();
            var characterClass =
                CharacterSelectionDetails.GetInstance().GetCharacterClass();
            var hasCharacter = 
                CharacterSelectionDetails.GetInstance().HasCharacter();
            var name = 
                hasCharacter
                    ? $"{characterClass} {(int)characterIndex}"
                    : $"Sample {characterIndex}";

            return string.Format(CharactersPath, name);
        }
    }
}
using System;
using Scripts.UI.Models;
using Scripts.UI.Windows;
using UI.Manager;
using UnityEngine;
using UIManagerUtils = UI.Manager.Utils;

namespace Scripts.UI.Controllers
{
    public class CharacterViewController : MonoBehaviour
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
            CreateCharacterSelectionOptionsWindow();
        }

        private void CreateCharacterView()
        {
            characterView = UIElementsCreator.GetInstance()
                .Create<CharacterView>(UILayer.Background, UIIndex.End);
        }

        private void CreateCharacterSelectionOptionsWindow()
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
            DestroyCharacterImages();
            DestroyCharacterView();
            DestroyCharacterSelectionOptionsWindow();
        }

        private void DestroyCharacterImages()
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

        private void DestroyCharacterView()
        {
            // TODO: Implement
        }

        private void DestroyCharacterSelectionOptionsWindow()
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
                    CharacterDetails.GetInstance().GetCharacterIndex();

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
                CharacterDetails.GetInstance().GetCharacterName();
            var characterClass =
                CharacterDetails.GetInstance().GetCharacterClass();

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
                    CharacterDetails.GetInstance().HasCharacter();

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
            CharacterDetails.GetInstance().SetCharacterClass(uiCharacterClass);

            ShowCharacterSelectionOptionsWindow();

            var characterIndex =
                CharacterDetails.GetInstance().GetCharacterIndex();
            var characterImage =
                characterImageCollection.GetCharacterView(characterIndex);

            CharacterSelected?.Invoke(characterImage);
        }
        
        private void OnStartButtonClicked()
        {
            var characterIndex =
                CharacterDetails.GetInstance().GetCharacterIndex();

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
                CharacterDetails.GetInstance().GetCharacterIndex();

            CharacterRemoved?.Invoke((int)characterIndex);
        }

        private string GetCharacterPath()
        {
            const string CharactersPath = "Characters/{0}";

            var characterIndex =
                CharacterDetails.GetInstance().GetCharacterIndex();
            var characterClass =
                CharacterDetails.GetInstance().GetCharacterClass();
            var hasCharacter = 
                CharacterDetails.GetInstance().HasCharacter();
            var name = 
                hasCharacter
                    ? $"{characterClass} {(int)characterIndex}"
                    : $"Sample {characterIndex}";

            return string.Format(CharactersPath, name);
        }
    }
}
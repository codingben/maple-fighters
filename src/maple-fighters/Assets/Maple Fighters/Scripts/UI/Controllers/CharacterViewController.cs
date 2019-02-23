using System;
using Scripts.UI.Windows;
using UI.Manager;
using UnityEngine;
using UIManagerUtils = UI.Manager.Utils;

namespace Scripts.UI.Controllers
{
    public class CharacterViewController : MonoBehaviour
    {
        public event Action<int> CharacterStarted;

        public event Action<ClickableCharacterImage> CharacterSelected;

        public event Action<int> CharacterRemoved;

        private ClickableCharacterImageCollection characterImageCollection;

        private CharacterView characterView;
        private CharacterSelectionOptionsWindow characterSelectionOptionsWindow;

        private CharacterSelectionController characterSelectionController;

        private void Awake()
        {
            characterImageCollection = new ClickableCharacterImageCollection();

            // TODO: Use event bus system
            characterSelectionController =
                FindObjectOfType<CharacterSelectionController>();

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
            characterSelectionOptionsWindow = UIElementsCreator.GetInstance()
                .Create<CharacterSelectionOptionsWindow>();
            characterSelectionOptionsWindow.StartButtonClicked +=
                OnStartButtonClicked;
            characterSelectionOptionsWindow.CreateCharacterButtonClicked +=
                OnCreateCharacterButtonClicked;
            characterSelectionOptionsWindow.DeleteCharacterButtonClicked +=
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
                characterImageCollection.GetClickableCharacterImages();
            foreach (var characterImage in characterImages)
            {
                if (characterImage != null)
                {
                    characterImage.CharacterClicked -= OnCharacterClicked;

                    Destroy(characterImage.gameObject);
                }
            }
        }

        private void DestroyCharacterView()
        {
            if (characterView != null)
            {
                Destroy(characterView.gameObject);
            }
        }

        private void DestroyCharacterSelectionOptionsWindow()
        {
            if (characterSelectionOptionsWindow != null)
            {
                characterSelectionOptionsWindow.StartButtonClicked -=
                    OnStartButtonClicked;
                characterSelectionOptionsWindow.CreateCharacterButtonClicked -=
                    OnCreateCharacterButtonClicked;
                characterSelectionOptionsWindow.DeleteCharacterButtonClicked -=
                    OnDeleteCharacterButtonClicked;

                Destroy(characterSelectionOptionsWindow.gameObject);
            }
        }

        public void CreateCharacter(UICharacterDetails uiCharacterDetails)
        {
            var path = GetCharacterPath(uiCharacterDetails);
            var characterGameObject = 
                UIManagerUtils.LoadAndCreateGameObject(path);
            var characterImage = 
                characterGameObject.GetComponent<ClickableCharacterImage>();
            if (characterImage != null)
            {
                var characterName = uiCharacterDetails.GetCharacterName();

                characterImage.SetCharacterDetails(uiCharacterDetails);
                characterImage.SetCharacterName(characterName);
                characterImage.CharacterClicked += OnCharacterClicked;

                var characterIndex = uiCharacterDetails.GetCharacterIndex();
                characterImageCollection
                    .SetCharacterImage(characterIndex, characterImage);
            }
        }

        private void ShowCharacterSelectionOptionsWindow()
        {
            if (characterSelectionOptionsWindow != null)
            {
                var uiCharacterDetails =
                    characterSelectionController.GetCharacterDetails();
                var hasCharacter = uiCharacterDetails.HasCharacter();

                characterSelectionOptionsWindow
                    .EnableOrDisableStartButton(hasCharacter);
                characterSelectionOptionsWindow
                    .EnableOrDisableCreateCharacterButton(!hasCharacter);
                characterSelectionOptionsWindow
                    .EnableOrDisableDeleteCharacterButton(hasCharacter);
                characterSelectionOptionsWindow.Show();
            }
        }

        private void OnCharacterClicked(UICharacterDetails characterDetails)
        {
            characterSelectionController.SetCharacterDetails(
                characterDetails);

            ShowCharacterSelectionOptionsWindow();

            var characterIndex = characterDetails.GetCharacterIndex();
            var characterImage =
                characterImageCollection.GetCharacterImage(characterIndex);

            CharacterSelected?.Invoke(characterImage);
        }
        
        private void OnStartButtonClicked()
        {
            var characterDetails =
                characterSelectionController.GetCharacterDetails();
            var characterIndex = (int)characterDetails.GetCharacterIndex();

            CharacterStarted?.Invoke(characterIndex);
        }

        private void OnCreateCharacterButtonClicked()
        {
            characterSelectionController.ShowCharacterSelectionWindow();
        }

        private void OnDeleteCharacterButtonClicked()
        {
            var characterDetails =
                characterSelectionController.GetCharacterDetails();
            var characterIndex = (int)characterDetails.GetCharacterIndex();

            CharacterRemoved?.Invoke(characterIndex);
        }

        private string GetCharacterPath(UICharacterDetails uiCharacterDetails)
        {
            const string CharactersPath = "Characters/{0}";

            var characterIndex = (int)uiCharacterDetails.GetCharacterIndex();
            var characterClass = uiCharacterDetails.GetCharacterClass();
            var hasCharacter = uiCharacterDetails.HasCharacter();
            var name = 
                hasCharacter
                    ? $"{characterClass} {characterIndex}"
                    : $"Sample {characterIndex}";

            return string.Format(CharactersPath, name);
        }
    }
}
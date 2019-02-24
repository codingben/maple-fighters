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

        public event Action<ClickableCharacterImage> CharacterSelected;

        public event Action<int> CharacterRemoved;

        private ClickableCharacterImageCollection characterImageCollection;

        private CharacterView characterView;
        private CharacterSelectionOptionsWindow characterSelectionOptionsWindow;

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

        public void CreateCharacter()
        {
            var characterGameObject = 
                UIManagerUtils.LoadAndCreateGameObject(GetCharacterPath());
            var characterImage = 
                characterGameObject.GetComponent<ClickableCharacterImage>();
            if (characterImage != null)
            {
                var characterName = 
                    CharacterDetails.GetInstance().GetCharacterName();

                // TODO: Set Character Id
                // characterImage.SetCharacterDetails(uiCharacterDetails);
                characterImage.CharacterName = characterName;
                characterImage.CharacterClicked += OnCharacterClicked;

                var characterIndex =
                    CharacterDetails.GetInstance().GetCharacterIndex();
                characterImageCollection
                    .SetCharacterImage(characterIndex, characterImage);
            }
        }

        private void ShowCharacterSelectionOptionsWindow()
        {
            if (characterSelectionOptionsWindow != null)
            {
                var hasCharacter =
                    CharacterDetails.GetInstance().HasCharacter();

                characterSelectionOptionsWindow
                    .EnableOrDisableStartButton(hasCharacter);

                characterSelectionOptionsWindow
                    .EnableOrDisableCreateCharacterButton(!hasCharacter);

                characterSelectionOptionsWindow
                    .EnableOrDisableDeleteCharacterButton(hasCharacter);

                characterSelectionOptionsWindow.Show();
            }
        }

        private void OnCharacterClicked(UICharacterClass uiCharacterClass)
        {
            CharacterDetails.GetInstance().SetCharacterClass(uiCharacterClass);

            ShowCharacterSelectionOptionsWindow();

            var characterIndex =
                CharacterDetails.GetInstance().GetCharacterIndex();
            var characterImage =
                characterImageCollection.GetCharacterImage(characterIndex);

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
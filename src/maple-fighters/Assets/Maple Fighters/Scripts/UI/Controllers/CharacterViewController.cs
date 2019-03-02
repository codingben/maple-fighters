using Scripts.UI.Models;
using Scripts.UI.Windows;
using UI.Manager;
using UnityEngine;
using UIManagerUtils = UI.Manager.Utils;

namespace Scripts.UI.Controllers
{
    public class CharacterViewController : MonoBehaviour
    {
        private ClickableCharacterImageCollection characterImageCollection;
        
        private ICharacterView characterView;
        private ICharacterSelectionOptionsView characterSelectionOptionsView;

        // private UICharacterIndex uiCharacterIndex;

        private void Awake()
        {
            characterImageCollection = new ClickableCharacterImageCollection();

            CreateChooseFighterView();
            CreateCharacterView();
            CreateAndSubscribeToCharacterSelectionOptionsWindow();
        }

        private void CreateChooseFighterView()
        {
            IChooseFighterView chooseFighterView = UIElementsCreator
                .GetInstance().Create<ChooseFighterText>();
            chooseFighterView.Show();
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

        public void CreateCharacter(CharacterDetails characterDetails)
        {
            var path = GetCharacterPath(characterDetails);
            var characterIndex = characterDetails.GetCharacterIndex();
            var characterName = characterDetails.GetCharacterName();
            var hasCharacter = characterDetails.HasCharacter();
            var characterGameObject = UIManagerUtils.LoadAndCreateGameObject(path);
            if (characterGameObject != null)
            {
                var characterImage = 
                    characterGameObject.GetComponent<ClickableCharacterImage>();

                characterImage.CharacterIndex = characterIndex;
                characterImage.CharacterName = characterName;
                characterImage.HasCharacter = hasCharacter;
                characterImage.CharacterClicked += OnCharacterClicked;

                characterImageCollection
                    .SetCharacterView(characterIndex, characterImage);

                AttachCharacterToView(characterGameObject);
            }
        }

        private void AttachCharacterToView(GameObject characterGameObject)
        {
            if (characterView != null)
            {
                characterGameObject.transform.SetParent(characterView.Transform);
                characterGameObject.transform.SetAsFirstSibling();
            }
        }

        private void ShowCharacterSelectionOptionsWindow()
        {
            characterSelectionOptionsView?.Show();
        }

        private void EnableOrDisableCharacterSelectionOptionsViewButtons(
            bool hasCharacter)
        {
            if (characterSelectionOptionsView != null)
            {
                characterSelectionOptionsView
                    .EnableOrDisableStartButton(hasCharacter);

                characterSelectionOptionsView
                    .EnableOrDisableCreateCharacterButton(!hasCharacter);

                characterSelectionOptionsView
                    .EnableOrDisableDeleteCharacterButton(hasCharacter);
            }
        }

        private void OnCharacterClicked(
            UICharacterIndex uiCharacterIndex,
            bool hasCharacter)
        {
            // this.uiCharacterIndex = uiCharacterIndex;

            ShowCharacterSelectionOptionsWindow();
            EnableOrDisableCharacterSelectionOptionsViewButtons(hasCharacter);
            
            /*var characterImage =
                characterImageCollection.GetCharacterView(uiCharacterIndex);*/

            // TODO: CharacterSelected(characterImage)
        }

        private void OnStartButtonClicked()
        {
            // TODO: CharacterStarted((int)uiCharacterIndex)
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
            // TODO: CharacterRemoved((int)uiCharacterIndex)
        }

        private string GetCharacterPath(CharacterDetails characterDetails)
        {
            var characterIndex = characterDetails.GetCharacterIndex();
            var characterClass = characterDetails.GetCharacterClass();
            var hasCharacter = characterDetails.HasCharacter();
            var name = 
                hasCharacter
                    ? $"{characterClass} {(int)characterIndex}"
                    : $"Sample {characterIndex}";

            return $"Characters/{name}";
        }
    }
}
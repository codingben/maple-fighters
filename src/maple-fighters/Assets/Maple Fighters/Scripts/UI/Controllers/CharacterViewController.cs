using Scripts.UI.Models;
using Scripts.UI.Windows;
using UI.Manager;
using UnityEngine;
using UIManagerUtils = UI.Manager.Utils;

namespace Scripts.UI.Controllers
{
    [RequireComponent(typeof(CharacterViewInteractor))]
    public class CharacterViewController : MonoBehaviour, IOnCharacterReceivedListener
    {
        private ClickableCharacterImageCollection characterImageCollection;
        
        private ICharacterView characterView;
        private ICharacterSelectionOptionsView characterSelectionOptionsView;

        private CharacterViewInteractor characterViewInteractor;

        // private UICharacterIndex uiCharacterIndex;

        private void Awake()
        {
            characterImageCollection = new ClickableCharacterImageCollection();
            characterViewInteractor = GetComponent<CharacterViewInteractor>();

            CreateCharacterView();
            CreateAndSubscribeToCharacterSelectionOptionsWindow();
            CreateChooseFighterView();
        }

        private void Start()
        {
            characterViewInteractor.GetCharacters();
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

        public void OnCharacterReceived(CharacterDetails characterDetails)
        {
            var path = GetCharacterPath(characterDetails);
            var clickableCharacterView =
                CreateAndShowClickableCharacterView(path);
            if (clickableCharacterView != null)
            {
                var characterIndex = characterDetails.GetCharacterIndex();
                var characterName = characterDetails.GetCharacterName();
                var hasCharacter = characterDetails.HasCharacter();

                clickableCharacterView.CharacterIndex = characterIndex;
                clickableCharacterView.CharacterName = characterName;
                clickableCharacterView.HasCharacter = hasCharacter;

                characterImageCollection
                    .SetCharacterView(characterIndex, clickableCharacterView);
            }
        }

        private IClickableCharacterView CreateAndShowClickableCharacterView(
            string path)
        {
            IClickableCharacterView clickableCharacterView = null;

            var characterGameObject = UIManagerUtils.LoadAndCreateGameObject(path);
            if (characterGameObject != null)
            {
                clickableCharacterView = characterGameObject
                    .GetComponent<ClickableCharacterImage>();
                clickableCharacterView.CharacterClicked += OnCharacterClicked;
                clickableCharacterView.Show();

                AttachCharacterToView(characterGameObject);
            }

            return clickableCharacterView;
        }

        private void AttachCharacterToView(GameObject characterGameObject)
        {
            if (characterView != null)
            {
                characterGameObject.transform.SetParent(characterView.Transform, false);
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
                    : $"Sample {(int)characterIndex}";

            return $"Characters/{name}";
        }
    }
}
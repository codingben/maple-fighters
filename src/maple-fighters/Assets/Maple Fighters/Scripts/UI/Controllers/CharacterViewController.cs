using Scripts.UI.Models;
using Scripts.UI.Windows;
using UI.Manager;
using UnityEngine;
using UnityEngine.SceneManagement;
using UIManagerUtils = UI.Manager.Utils;

namespace Scripts.UI.Controllers
{
    [RequireComponent(typeof(CharacterViewInteractor))]
    public class CharacterViewController : MonoBehaviour,
                                           IOnCharacterReceivedListener,
                                           IOnCharacterValidatedListener,
                                           IOnCharacterRemovedListener
    {
        private int characterIndex;
        private ICharacterImageCollection characterImageCollection;
        
        private ICharacterView characterView;
        private ICharacterSelectionOptionsView characterSelectionOptionsView;
        private IChooseFighterView chooseFighterView;

        private CharacterViewInteractor characterViewInteractor;

        private void Awake()
        {
            characterImageCollection = new ClickableCharacterImageCollection();
            characterViewInteractor = GetComponent<CharacterViewInteractor>();

            CreateCharacterView();
            CreateAndSubscribeToCharacterSelectionOptionsWindow();
            CreateAndShowChooseFighterView();
        }

        private void Start()
        {
            characterViewInteractor.GetCharacters();
        }

        private void CreateAndShowChooseFighterView()
        {
            chooseFighterView = UIElementsCreator.GetInstance()
                .Create<ChooseFighterText>(UILayer.Background, UIIndex.End);
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
            var characterImages = characterImageCollection.GetAll();
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

        // TODO: Hack
        public void OnCharactersReceived()
        {
            DestroyAllCharacterImages();
        }

        public void OnCharacterReceived(CharacterDetails characterDetails)
        {
            var path = GetCharacterPath(characterDetails);
            var characterView = CreateAndShowCharacterView(path);
            if (characterView != null)
            {
                var characterIndex = characterDetails.GetCharacterIndex();
                var characterName = characterDetails.GetCharacterName();
                var hasCharacter = characterDetails.HasCharacter();

                characterView.CharacterIndex = characterIndex;
                characterView.CharacterName = characterName;
                characterView.HasCharacter = hasCharacter;

                characterImageCollection.Set(characterIndex, characterView);
            }
        }

        public void OnCharacterValidated(UIMapIndex uiMapIndex)
        {
            // TODO: Remove this from here
            SceneManager.LoadScene((int)uiMapIndex);
        }

        public void OnCharacterRemoved()
        {
            characterViewInteractor.GetCharacters();
        }

        private void DestroyAllCharacterImages()
        {
            var characterImages = characterImageCollection.GetAll();
            foreach (var characterImage in characterImages)
            {
                if (characterImage != null)
                {
                    print(characterImage.GameObject.name);

                    Destroy(characterImage.GameObject);
                }
            }
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
                characterSelectionOptionsView.EnableOrDisableStartButton(hasCharacter);
                characterSelectionOptionsView.EnableOrDisableCreateCharacterButton(!hasCharacter);
                characterSelectionOptionsView.EnableOrDisableDeleteCharacterButton(hasCharacter);
            }
        }

        private void OnCharacterClicked(
            UICharacterIndex uiCharacterIndex,
            bool hasCharacter)
        {
            characterIndex = (int)uiCharacterIndex;

            ShowCharacterSelectionOptionsWindow();
            EnableOrDisableCharacterSelectionOptionsViewButtons(hasCharacter);
        }

        private void OnStartButtonClicked()
        {
            characterViewInteractor.ValidateCharacter(characterIndex);
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
            characterViewInteractor.RemoveCharacter(characterIndex);
        }

        private IClickableCharacterView CreateAndShowCharacterView(
            string path)
        {
            IClickableCharacterView characterView = null;

            var characterGameObject = UIManagerUtils.LoadAndCreateGameObject(path);
            if (characterGameObject != null)
            {
                characterView = characterGameObject.GetComponent<ClickableCharacterImage>();
                characterView.CharacterClicked += OnCharacterClicked;
                characterView.Show();

                AttachCharacterToView(characterGameObject);
            }

            return characterView;
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
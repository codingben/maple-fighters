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
                                           IOnCharacterRemovedListener,
                                           IOnCharacterCreationFinishedListener
    {
        [SerializeField]
        private int characterNameLength;

        private ICharacterView characterView;
        private ICharacterSelectionOptionsView characterSelectionOptionsView;
        private IChooseFighterView chooseFighterView;
        private ICharacterSelectionView characterSelectionView;
        private ICharacterNameView characterNameView;

        private CharacterDetails characterDetails;
        private ClickableCharacterImageCollection characterImageCollection;

        private CharacterViewInteractor characterViewInteractor;

        private void Awake()
        {
            var characterViews =
                new IClickableCharacterView[] { null, null, null };

            characterDetails = new CharacterDetails();
            characterImageCollection =
                new ClickableCharacterImageCollection(characterViews);

            characterViewInteractor = GetComponent<CharacterViewInteractor>();

            CreateCharacterView();
            CreateAndSubscribeToCharacterSelectionOptionsWindow();
            CreateAndShowChooseFighterView();
            CreateAndSubscribeToCharacterSelectionWindow();
            CreateAndSubscribeToCharacterNameWindow();
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

        private void CreateAndSubscribeToCharacterSelectionWindow()
        {
            characterSelectionView = UIElementsCreator.GetInstance()
                .Create<CharacterSelectionWindow>();
            characterSelectionView.ChooseButtonClicked +=
                OnChooseButtonClicked;
            characterSelectionView.CancelButtonClicked +=
                OnCancelButtonClicked;
            characterSelectionView.CharacterSelected +=
                OnCharacterSelected;
        }

        private void CreateAndSubscribeToCharacterNameWindow()
        {
            characterNameView = UIElementsCreator.GetInstance()
                .Create<CharacterNameWindow>();
            characterNameView.ConfirmButtonClicked +=
                OnConfirmButtonClicked;
            characterNameView.BackButtonClicked +=
                OnBackButtonClicked;
            characterNameView.NameInputFieldChanged +=
                OnNameInputFieldChanged;
        }

        private void OnDestroy()
        {
            UnsubscribeFromCharacterImages();
            UnsubscribeFromCharacterSelectionOptionsWindow();
            UnsubscribeFromCharacterSelectionWindow();
            UnsubscribeFromCharacterNameWindow();
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

        private void UnsubscribeFromCharacterSelectionWindow()
        {
            if (characterSelectionView != null)
            {
                characterSelectionView.ChooseButtonClicked -=
                    OnChooseButtonClicked;
                characterSelectionView.CancelButtonClicked -=
                    OnCancelButtonClicked;
                characterSelectionView.CharacterSelected -=
                    OnCharacterSelected;
            }
        }

        private void UnsubscribeFromCharacterNameWindow()
        {
            if (characterNameView != null)
            {
                characterNameView.ConfirmButtonClicked -=
                    OnConfirmButtonClicked;
                characterNameView.BackButtonClicked -=
                    OnBackButtonClicked;
                characterNameView.NameInputFieldChanged -=
                    OnNameInputFieldChanged;
            }
        }

        public void OnBeforeCharacterReceived()
        {
            DestroyAllCharacterImages();
        }

        public void OnAfterCharacterReceived(CharacterDetails characterDetails)
        {
            var characterView =
                CreateAndShowCharacterView(GetCharacterPath(characterDetails));
            if (characterView != null)
            {
                characterView.CharacterIndex = characterDetails.GetCharacterIndex();
                characterView.CharacterName = characterDetails.GetCharacterName();
                characterView.HasCharacter = characterDetails.HasCharacter();

                var characterIndex = characterDetails.GetCharacterIndex();
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

        public void OnCharacterCreated()
        {
            characterViewInteractor.GetCharacters();
        }

        public void OnCreateCharacterFailed(CharacterCreationFailed reason)
        {
            switch (reason)
            {
                case CharacterCreationFailed.Unknown:
                {
                    var message = WindowMessages.CharacterCreationFailed;
                    NoticeUtils.ShowNotice(message);
                    break;
                }

                case CharacterCreationFailed.NameAlreadyInUse:
                {
                    var message = WindowMessages.NameAlreadyInUse;
                    NoticeUtils.ShowNotice(message);
                    break;
                }
            }
        }

        private void DestroyAllCharacterImages()
        {
            var characterImages = characterImageCollection.GetAll();
            foreach (var characterImage in characterImages)
            {
                if (characterImage != null)
                {
                    Destroy(characterImage.GameObject);
                }
            }
        }

        private void AttachCharacterToView(GameObject characterGameObject)
        {
            if (characterView != null)
            {
                characterGameObject.transform.SetParent(characterView.Transform, false);
                characterGameObject.transform.SetAsLastSibling();
            }
        }

        private void ShowCharacterSelectionOptionsWindow()
        {
            characterSelectionOptionsView?.Show();
        }

        private void EnableOrDisableCharacterSelectionOptionsViewButtons(bool hasCharacter)
        {
            if (characterSelectionOptionsView != null)
            {
                characterSelectionOptionsView.EnableOrDisableStartButton(hasCharacter);
                characterSelectionOptionsView.EnableOrDisableCreateCharacterButton(!hasCharacter);
                characterSelectionOptionsView.EnableOrDisableDeleteCharacterButton(hasCharacter);
            }
        }

        private void OnCharacterClicked(UICharacterIndex uiCharacterIndex, bool hasCharacter)
        {
            characterDetails.SetCharacterIndex(uiCharacterIndex);

            ShowCharacterSelectionOptionsWindow();
            EnableOrDisableCharacterSelectionOptionsViewButtons(hasCharacter);
        }

        private void OnStartButtonClicked()
        {
            var characterIndex = (int)characterDetails.GetCharacterIndex();
            characterViewInteractor.ValidateCharacter(characterIndex);
        }

        private void OnCreateCharacterButtonClicked()
        {
            ShowCharacterSelectionWindow();
        }

        private void OnDeleteCharacterButtonClicked()
        {
            var characterIndex = (int)characterDetails.GetCharacterIndex();
            characterViewInteractor.RemoveCharacter(characterIndex);
        }

        private void OnNameInputFieldChanged(string characterName)
        {
            if (characterName.Length >= characterNameLength)
            {
                characterNameView?.EnableConfirmButton();
            }
            else
            {
                characterNameView?.DisableConfirmButton();
            }
        }

        private void OnConfirmButtonClicked(string characterName)
        {
            characterDetails.SetCharacterName(characterName);
            characterViewInteractor.CreateCharacter(characterDetails);
        }

        private void OnBackButtonClicked()
        {
            HideCharacterNameWindow();
            ShowCharacterSelectionWindow();
        }

        private void OnChooseButtonClicked()
        {
            HideCharacterSelectionWindow();
            ShowCharacterNameWindow();
        }

        private void OnCancelButtonClicked()
        {
            HideCharacterSelectionWindow();
        }

        private void OnCharacterSelected(UICharacterClass uiCharacterClass)
        {
            characterDetails.SetCharacterClass(uiCharacterClass);
        }

        private void ShowCharacterNameWindow()
        {
            characterNameView?.Show();
        }

        private void HideCharacterNameWindow()
        {
            characterNameView?.Hide();
        }

        private void ShowCharacterSelectionWindow()
        {
            characterSelectionView?.Show();
        }

        private void HideCharacterSelectionWindow()
        {
            characterSelectionView?.Hide();
        }

        private IClickableCharacterView CreateAndShowCharacterView(string path)
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
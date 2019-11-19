using System;

using Scripts.Constants;
using Scripts.UI.MenuBackground;
using Scripts.UI.Notice;
using UI.Manager;
using UnityEngine;
using UnityEngine.SceneManagement;
using UIManagerUtils = UI.Manager.Utils;

namespace Scripts.UI.CharacterSelection
{
    [RequireComponent(typeof(CharacterViewInteractor))]
    public class CharacterViewController : MonoBehaviour, 
                                           IOnConnectionFinishedListener,
                                           IOnCharacterReceivedListener,
                                           IOnCharacterValidationFinishedListener,
                                           IOnCharacterDeletionFinishedListener,
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
        private CharacterViewCollection? characterViewCollection;

        private CharacterViewInteractor characterViewInteractor;

        private void Awake()
        {
            characterDetails = new CharacterDetails();
            characterViewInteractor = GetComponent<CharacterViewInteractor>();
        }

        private void Start()
        {
            characterViewInteractor.ConnectToGameServer();
        }

        private void OnDestroy()
        {
            UnsubscribeFromCharacterImages();
            UnsubscribeFromCharacterSelectionOptionsWindow();
            UnsubscribeFromCharacterSelectionWindow();
            UnsubscribeFromCharacterNameWindow();
            UnsubscribeFromBackgroundClicked();
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
                .Create<CharacterSelectionOptionsWindow>(
                    UILayer.Foreground,
                    UIIndex.End);
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
                .Create<CharacterSelectionWindow>(
                    UILayer.Foreground,
                    UIIndex.End);
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
                .Create<CharacterNameWindow>(UILayer.Foreground, UIIndex.End);
            characterNameView.ConfirmButtonClicked +=
                OnConfirmButtonClicked;
            characterNameView.BackButtonClicked +=
                OnBackButtonClicked;
            characterNameView.NameInputFieldChanged +=
                OnNameInputFieldChanged;
        }

        private void SubscribeToBackgroundClicked()
        {
            var backgroundController =
                FindObjectOfType<MenuBackgroundController>();
            if (backgroundController != null)
            {
                backgroundController.BackgroundClicked += OnBackgroundClicked;
            }
        }

        private void UnsubscribeFromBackgroundClicked()
        {
            var backgroundController =
                FindObjectOfType<MenuBackgroundController>();
            if (backgroundController != null)
            {
                backgroundController.BackgroundClicked -= OnBackgroundClicked;
            }
        }

        private void UnsubscribeFromCharacterImages()
        {
            var characterImages = characterViewCollection?.GetAll();
            if (characterImages != null)
            {
                foreach (var characterImage in characterImages)
                {
                    if (characterImage != null)
                    {
                        characterImage.CharacterClicked -= OnCharacterClicked;
                    }
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

        public void OnConnectionSucceed()
        {
            CreateCharacterView();
            CreateAndShowChooseFighterView();
            CreateAndSubscribeToCharacterSelectionOptionsWindow();
            CreateAndSubscribeToCharacterSelectionWindow();
            CreateAndSubscribeToCharacterNameWindow();
            SubscribeToBackgroundClicked();

            characterViewInteractor.GetCharacters();
        }

        public void OnConnectionFailed()
        {
            void OnOkButtonClicked()
            {
                var sceneName = SceneNames.Main;
                SceneManager.LoadScene(sceneName);
            }

            var message = NoticeMessages.CharacterView.CouldNotConnect;
            NoticeUtils.ShowNotice(message, OnOkButtonClicked);
        }

        public void OnCharacterReceived(CharacterDetails characterDetails)
        {
            var path = Utils.GetCharacterPath(characterDetails);
            var characterView = CreateAndShowCharacterView(path);
            if (characterView != null)
            {
                characterView.CharacterIndex = characterDetails.GetCharacterIndex();
                characterView.CharacterName = characterDetails.GetCharacterName();
                characterView.HasCharacter = characterDetails.HasCharacter();

                var characterIndex = characterDetails.GetCharacterIndex();
                if (characterIndex != UICharacterIndex.Zero)
                {
                    if (characterViewCollection == null)
                    {
                        var views = new IClickableCharacterView[] { null, null, null };
                        characterViewCollection = new CharacterViewCollection(views);
                    }

                    var index = (int)characterIndex;
                    characterViewCollection?.Set(index, characterView);
                }
            }
        }

        public void OnCharacterValidated(UIMapIndex uiMapIndex)
        {
            // TODO: Remove this from here
            SceneManager.LoadScene((int)uiMapIndex);
        }

        public void OnCharacterUnvalidated()
        {
            var message = 
                NoticeMessages.CharacterView.CharacterValidationFailed;
            NoticeUtils.ShowNotice(message);
        }

        public void OnCharacterDeletionSucceed()
        {
            DestroyAllCharacterImages();

            characterViewInteractor.GetCharacters();
        }

        public void OnCharacterDeletionFailed()
        {
            var message = 
                NoticeMessages.CharacterView.CharacterDeletionFailed;
            NoticeUtils.ShowNotice(message);
        }

        public void OnCharacterCreated()
        {
            DestroyAllCharacterImages();

            characterViewInteractor.GetCharacters();
        }

        public void OnCreateCharacterFailed(CharacterCreationFailed reason)
        {
            switch (reason)
            {
                case CharacterCreationFailed.Unknown:
                {
                    var message = 
                        NoticeMessages.CharacterView.CharacterCreationFailed;
                    NoticeUtils.ShowNotice(message);
                    break;
                }

                case CharacterCreationFailed.NameAlreadyInUse:
                {
                    var message = 
                        NoticeMessages.CharacterView.NameAlreadyInUse;
                    NoticeUtils.ShowNotice(message);
                    break;
                }
            }
        }

        private void DestroyAllCharacterImages()
        {
            var characterImages = characterViewCollection?.GetAll();
            if (characterImages != null)
            {
                foreach (var characterImage in characterImages)
                {
                    if (characterImage != null)
                    {
                        var image = characterImage.GameObject;
                        if (image != null)
                        {
                            Destroy(image);
                        }
                    }
                }
            }
        }

        private void ShowCharacterSelectionOptionsWindow()
        {
            characterSelectionOptionsView?.Show();
        }

        private void HideCharacterSelectionOptionsWindow()
        {
            if (characterSelectionOptionsView != null
                && characterSelectionOptionsView.IsShown)
            {
                characterSelectionOptionsView.Hide();
            }
        }

        private void EnableOrDisableCharacterSelectionOptionsViewButtons(bool hasCharacter)
        {
            characterSelectionOptionsView?.EnableOrDisableStartButton(hasCharacter);
            characterSelectionOptionsView?.EnableOrDisableCreateCharacterButton(!hasCharacter);
            characterSelectionOptionsView?.EnableOrDisableDeleteCharacterButton(hasCharacter);
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
            HideCharacterSelectionOptionsWindow();
            ShowCharacterSelectionWindow();
        }

        private void OnDeleteCharacterButtonClicked()
        {
            HideCharacterSelectionOptionsWindow();

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
            HideCharacterNameWindow();

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

        private void OnBackgroundClicked()
        {
            HideCharacterSelectionOptionsWindow();
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

            var character = CreateCharacterView(path);
            if (character != null)
            {
                characterView = character.GetComponent<ClickableCharacterImage>();

                if (characterView != null)
                {
                    characterView.CharacterClicked += OnCharacterClicked;
                    characterView.Show();
                }
            }

            return characterView;
        }

        private GameObject CreateCharacterView(string path)
        {
            var character = UIManagerUtils.LoadAndCreateGameObject(path);
            if (character != null)
            {
                character.transform.SetParent(characterView.Transform, false);
                character.transform.SetAsLastSibling();
            }

            return character;
        }
    }
}
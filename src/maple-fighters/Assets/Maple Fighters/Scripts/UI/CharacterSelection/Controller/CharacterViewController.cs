using System.Text.RegularExpressions;
using Scripts.Constants;
using Scripts.UI.Authenticator;
using Scripts.UI.GameServerBrowser;
using Scripts.UI.MenuBackground;
using Scripts.UI.Notice;
using UI;
using UnityEngine;

namespace Scripts.UI.CharacterSelection
{
    [RequireComponent(typeof(CharacterViewInteractor))]
    public class CharacterViewController : MonoBehaviour,
                                           IOnCharacterReceivedListener,
                                           IOnCharacterDeletionFinishedListener,
                                           IOnCharacterCreationFinishedListener
    {
        [SerializeField]
        private int minCharacterNameLength;

        private ILoadingView loadingView;
        private IChooseCharacterView chooseCharacterView;
        private ICharacterView characterView;
        private ICharacterSelectionOptionsView characterSelectionOptionsView;
        private ICharacterSelectionView characterSelectionView;
        private ICharacterNameView characterNameView;

        private CharacterViewCollection? characterViewCollection;
        private int characterIndex = -1;
        private UINewCharacterDetails characterDetails;

        private CharacterViewInteractor characterViewInteractor;

        private void Awake()
        {
            characterViewInteractor = GetComponent<CharacterViewInteractor>();
        }

        private void Start()
        {
            CreateAndShowCharacterView();
        }

        private void OnDestroy()
        {
            UnsubscribeFromCharacterImages();
            UnsubscribeFromCharacterSelectionOptionsWindow();
            UnsubscribeFromCharacterSelectionWindow();
            UnsubscribeFromCharacterNameWindow();
            UnsubscribeFromBackgroundClicked();
        }

        private void CreateAndShowCharacterView()
        {
            CreateLoadingView();
            SubscribeToLoadingView();
            ShowLoadingView();
        }

        private void CreateChooseCharacterView()
        {
            chooseCharacterView = UICreator
                .GetInstance()
                .Create<ChooseCharacterText>(UICanvasLayer.Background, UIIndex.End);
        }

        private void CreateLoadingView()
        {
            loadingView = UICreator
                .GetInstance()
                .Create<LoadingText>(UICanvasLayer.Background, UIIndex.End);
        }

        private void CreateCharacterView()
        {
            characterView = UICreator
                .GetInstance()
                .Create<CharacterView>(UICanvasLayer.Background, UIIndex.End);
        }

        private void CreateAndSubscribeToCharacterSelectionOptionsWindow()
        {
            characterSelectionOptionsView = UICreator
                .GetInstance()
                .Create<CharacterSelectionOptionsWindow>(UICanvasLayer.Foreground, UIIndex.End);

            if (characterSelectionOptionsView != null)
            {
                characterSelectionOptionsView.ChooseCharacterButtonClicked +=
                    OnChooseCharacterButtonClicked;
                characterSelectionOptionsView.CreateCharacterButtonClicked +=
                    OnCreateCharacterButtonClicked;
                characterSelectionOptionsView.DeleteCharacterButtonClicked +=
                    OnDeleteCharacterButtonClicked;
            }
        }

        private void CreateAndSubscribeToCharacterSelectionWindow()
        {
            characterSelectionView = UICreator
                .GetInstance()
                .Create<CharacterSelectionWindow>(UICanvasLayer.Foreground, UIIndex.End);

            if (characterSelectionView != null)
            {
                characterSelectionView.CharacterSelected +=
                    OnCharacterSelected;
            }
        }

        private void CreateAndSubscribeToCharacterNameWindow()
        {
            characterNameView = UICreator
                .GetInstance()
                .Create<CharacterNameWindow>(UICanvasLayer.Foreground, UIIndex.End);

            if (characterNameView != null)
            {
                characterNameView.ConfirmButtonClicked +=
                    OnConfirmButtonClicked;
                characterNameView.BackButtonClicked +=
                    OnBackButtonClicked;
                characterNameView.NameInputFieldChanged +=
                    OnNameInputFieldChanged;
            }
        }

        private void SubscribeToLoadingView()
        {
            if (loadingView != null)
            {
                loadingView.LoadingAnimation.Finished +=
                    OnLoadingAnimationFinished;
            }
        }

        private void SubscribeToBackgroundClicked()
        {
            var backgroundController =
                FindObjectOfType<MenuBackgroundController>();
            if (backgroundController != null)
            {
                backgroundController.BackgroundClicked +=
                    OnBackgroundClicked;
            }
        }

        private void UnsubscribeFromBackgroundClicked()
        {
            var backgroundController =
                FindObjectOfType<MenuBackgroundController>();
            if (backgroundController != null)
            {
                backgroundController.BackgroundClicked -=
                    OnBackgroundClicked;
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
                        characterImage.CharacterClicked -=
                            OnCharacterClicked;
                    }
                }
            }
        }

        private void UnsubscribeFromCharacterSelectionOptionsWindow()
        {
            if (characterSelectionOptionsView != null)
            {
                characterSelectionOptionsView.ChooseCharacterButtonClicked -=
                    OnChooseCharacterButtonClicked;
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

        private void UnsubscribeFromLoadingView()
        {
            if (loadingView != null)
            {
                loadingView.LoadingAnimation.Finished -=
                    OnLoadingAnimationFinished;
            }
        }

        public void OnCharacterReceived(UICharacterDetails characterDetails)
        {
            var path = Utils.GetCharacterPath(characterDetails);
            var characterView = CreateAndShowCharacterView(path);
            if (characterView != null)
            {
                characterView.Id = characterDetails.GetCharacterId();
                characterView.CharacterName = characterDetails.GetCharacterName();
                characterView.CharacterIndex = characterDetails.GetCharacterIndex();
                characterView.CharacterClass = characterDetails.GetCharacterClass();

                var characterIndex = characterDetails.GetCharacterIndex();
                if (characterIndex != UICharacterIndex.Zero)
                {
                    if (characterViewCollection == null)
                    {
                        var views =
                            new IClickableCharacterView[] { null, null, null };
                        characterViewCollection =
                            new CharacterViewCollection(views);
                    }

                    var index = (int)characterIndex;
                    characterViewCollection?.Set(index, characterView);
                }
            }
        }

        public void OnAfterCharacterReceived()
        {
            HideLoadingView();
            ShowChooseCharacterView();
        }

        public void OnCharacterDeletionSucceed()
        {
            HideChooseCharacterView();
            ShowLoadingView();

            LoadCharacters();
        }

        public void OnCharacterDeletionFailed()
        {
            NoticeUtils.ShowNotice(message: NoticeMessages.CharacterView.DeletionFailed);
        }

        public void OnCharacterCreated()
        {
            ResetCharacterName();

            HideCharacterNameWindow();
            HideChooseCharacterView();

            ShowLoadingView();

            LoadCharacters();
        }

        public void OnCreateCharacterFailed(UICharacterCreationFailed reason)
        {
            switch (reason)
            {
                case UICharacterCreationFailed.Unknown:
                {
                    NoticeUtils.ShowNotice(message: NoticeMessages.CharacterView.CreationFailed);
                    break;
                }

                case UICharacterCreationFailed.NameAlreadyInUse:
                {
                    NoticeUtils.ShowNotice(message: NoticeMessages.CharacterView.NameAlreadyInUse);
                    break;
                }
            }

            characterNameView.EnableConfirmButton();
        }

        private void OnLoadingAnimationFinished()
        {
            UnsubscribeFromLoadingView();

            CreateChooseCharacterView();
            CreateCharacterView();
            CreateAndSubscribeToCharacterSelectionOptionsWindow();
            CreateAndSubscribeToCharacterSelectionWindow();
            CreateAndSubscribeToCharacterNameWindow();

            SubscribeToBackgroundClicked();

            LoadCharacters();
        }

        public void LoadCharacters()
        {
            RemoveAllCharacterImages();

            characterViewInteractor.GetCharacters();
        }

        private void RemoveAllCharacterImages()
        {
            var characterImages = characterViewCollection?.GetAll();
            if (characterImages != null)
            {
                foreach (var characterImage in characterImages)
                {
                    if (characterImage != null)
                    {
                        var gameObject = characterImage.GameObject;
                        if (gameObject != null)
                        {
                            Destroy(gameObject);
                        }
                    }
                }
            }
        }

        private void ShowChooseCharacterView()
        {
            chooseCharacterView?.Show();
        }

        private void ShowLoadingView()
        {
            loadingView?.Show();
        }

        private void HideChooseCharacterView()
        {
            chooseCharacterView?.Hide();
        }

        private void HideLoadingView()
        {
            loadingView?.Hide();
        }

        private void ShowCharacterSelectionOptionsWindow()
        {
            characterSelectionOptionsView?.Show();
        }

        public void HideCharacterSelectionOptionsWindow()
        {
            if (characterSelectionOptionsView != null &&
                characterSelectionOptionsView.IsShown)
            {
                characterSelectionOptionsView.Hide();
            }
        }

        private void EnableOrDisableCharacterSelectionOptionsViewButtons(bool hasCharacter)
        {
            characterSelectionOptionsView?.EnableOrDisableChooseCharacterButton(hasCharacter);
            characterSelectionOptionsView?.EnableOrDisableCreateCharacterButton(!hasCharacter);
            characterSelectionOptionsView?.EnableOrDisableDeleteCharacterButton(hasCharacter);
        }

        private void OnCharacterClicked(UICharacterIndex uiCharacterIndex, bool hasCharacter)
        {
            characterIndex = (int)uiCharacterIndex;

            HideCharacterNameWindow();
            HideCharacterSelectionWindow();
            HideChooseCharacterView();
            HideGameServerBrowserWindow();
            HideAuthenticatorView();

            ShowCharacterSelectionOptionsWindow();

            EnableOrDisableCharacterSelectionOptionsViewButtons(hasCharacter);
        }

        private void OnChooseCharacterButtonClicked()
        {
            if (characterIndex != -1)
            {
                var character = characterViewCollection?.Get(characterIndex);
                if (character != null)
                {
                    var characterClass = (byte)character.CharacterClass;
                    var characterName = character.CharacterName;

                    characterViewInteractor.UpdateCharacterData(characterClass, characterName);

                    HideCharacterSelectionOptionsWindow();
                    ShowGameServerBrowserWindow();
                }
                else
                {
                    NoticeUtils.ShowNotice(message: NoticeMessages.CharacterView.NotFound);
                }
            }
        }

        private void OnCreateCharacterButtonClicked()
        {
            HideCharacterSelectionOptionsWindow();
            ShowCharacterSelectionWindow();
        }

        private void OnDeleteCharacterButtonClicked()
        {
            HideCharacterSelectionOptionsWindow();

            if (characterIndex != -1)
            {
                var character = characterViewCollection?.Get(characterIndex);
                if (character != null)
                {
                    var characterId = character.Id;
                    characterViewInteractor.RemoveCharacter(characterId);
                }
            }
        }

        private void OnNameInputFieldChanged(string characterName)
        {
            if (IsCharacterNameValid(characterName))
            {
                characterNameView?.EnableConfirmButton();
            }
            else
            {
                characterNameView?.DisableConfirmButton();
            }
        }

        private bool IsCharacterNameValid(string name)
        {
            var isEnoughLength = name.Length >= minCharacterNameLength;
            var isNotEmpty = !string.IsNullOrEmpty(name);
            var isNotWhitespace = !Regex.IsMatch(name, @"\s");

            return isEnoughLength && isNotEmpty && isNotWhitespace;
        }

        private void OnConfirmButtonClicked(string characterName)
        {
            characterDetails.SetCharacterName(characterName);
            characterViewInteractor.CreateCharacter(characterIndex, characterDetails);
        }

        private void OnBackButtonClicked()
        {
            HideCharacterNameWindow();
            ShowCharacterSelectionWindow();
        }

        private void OnCharacterSelected(UICharacterClass uiCharacterClass)
        {
            characterDetails.SetCharacterClass(uiCharacterClass);

            HideCharacterSelectionWindow();
            ShowCharacterNameWindow();
        }

        private void HideAuthenticatorView()
        {
            var authenticatorController = FindObjectOfType<AuthenticatorController>();
            authenticatorController?.HideLoginWindow();
            authenticatorController?.HideRegistrationWindow();
        }

        private void ResetCharacterName()
        {
            characterNameView.ResetNameInputField();
        }

        private void OnBackgroundClicked()
        {
            HideCharacterSelectionOptionsWindow();
            HideCharacterSelectionWindow();
            HideCharacterNameWindow();

            ShowChooseCharacterView();
        }

        private void ShowCharacterNameWindow()
        {
            characterNameView?.Show();
            characterNameView?.GenerateRandomCharacterName();
        }

        public void HideCharacterNameWindow()
        {
            if (characterNameView != null &&
                characterNameView.IsShown)
            {
                characterNameView?.Hide();
            }
        }

        private void ShowCharacterSelectionWindow()
        {
            characterSelectionView?.Show();
        }

        public void HideCharacterSelectionWindow()
        {
            if (characterSelectionView != null &&
                characterSelectionView.IsShown)
            {
                characterSelectionView?.Hide();
            }
        }

        private void ShowGameServerBrowserWindow()
        {
            var gameServerBrowserController =
                FindObjectOfType<GameServerBrowserController>();
            gameServerBrowserController?.ShowGameServerBrowserWindow();
        }

        private void HideGameServerBrowserWindow()
        {
            var gameServerBrowserController =
                FindObjectOfType<GameServerBrowserController>();
            gameServerBrowserController?.HideGameServerBrowserWindow();
        }

        private IClickableCharacterView CreateAndShowCharacterView(string path)
        {
            IClickableCharacterView characterView = null;

            var character = CreateCharacterView(path);
            if (character != null)
            {
                characterView =
                    character.GetComponent<ClickableCharacterImage>();

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
            var characterPrefab = Resources.Load<GameObject>(path);
            var character = Instantiate(characterPrefab);
            if (character != null)
            {
                if (characterView != null)
                {
                    var view = characterView.Transform;

                    character.transform.SetParent(view, false);
                    character.transform.SetAsLastSibling();
                }
            }

            return character;
        }
    }
}
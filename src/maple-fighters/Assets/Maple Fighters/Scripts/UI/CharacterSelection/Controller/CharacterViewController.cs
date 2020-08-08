using Scripts.Constants;
using Scripts.UI.MenuBackground;
using Scripts.UI.Notice;
using UI.Manager;
using UnityEngine;
using UnityEngine.SceneManagement;

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

        private ILoadingView loadingView;
        private IChooseFighterView chooseFighterView;
        private ICharacterView characterView;
        private ICharacterSelectionOptionsView characterSelectionOptionsView;
        private ICharacterSelectionView characterSelectionView;
        private ICharacterNameView characterNameView;

        private UICharacterDetails characterDetails;
        private CharacterViewCollection? characterViewCollection;

        private CharacterViewInteractor characterViewInteractor;

        private void Awake()
        {
            characterDetails = new UICharacterDetails();
            characterViewInteractor = GetComponent<CharacterViewInteractor>();
        }

        private void Start()
        {
            CreateLoadingView();
            ShowLoadingView();
            SubscribeToLoadingView();
        }

        private void OnDestroy()
        {
            UnsubscribeFromCharacterImages();
            UnsubscribeFromCharacterSelectionOptionsWindow();
            UnsubscribeFromCharacterSelectionWindow();
            UnsubscribeFromCharacterNameWindow();
            UnsubscribeFromBackgroundClicked();
        }

        private void CreateChooseFighterView()
        {
            chooseFighterView = UIElementsCreator.GetInstance()
                .Create<ChooseFighterText>(UILayer.Background, UIIndex.End);
        }

        private void CreateLoadingView()
        {
            loadingView = UIElementsCreator.GetInstance()
                .Create<LoadingText>(UILayer.Background, UIIndex.End);
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

        private void UnsubscribeFromLoadingView()
        {
            if (loadingView != null)
            {
                loadingView.LoadingAnimation.Finished -=
                    OnLoadingAnimationFinished;
            }
        }

        public void OnConnectionSucceed()
        {
            CreateChooseFighterView();
            CreateCharacterView();
            CreateAndSubscribeToCharacterSelectionOptionsWindow();
            CreateAndSubscribeToCharacterSelectionWindow();
            CreateAndSubscribeToCharacterNameWindow();
            SubscribeToBackgroundClicked();

            RemoveAndShowAllCharacterImages();
        }

        public void OnConnectionFailed()
        {
            NoticeUtils.ShowNotice(
                message: NoticeMessages.CharacterView.ConnectionFailed,
                () => SceneManager.LoadScene(sceneName: SceneNames.Main));
        }

        public void OnCharacterReceived(UICharacterDetails characterDetails)
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
                        var views =
                            new IClickableCharacterView[] { null, null, null };
                        characterViewCollection = new CharacterViewCollection(views);
                    }

                    var index = (int)characterIndex;
                    characterViewCollection?.Set(index, characterView);
                }
            }
        }

        public void OnAfterCharacterReceived()
        {
            HideLoadingView();
            ShowChooseFighterView();
        }

        public void OnCharacterValidated(string mapName)
        {
            SceneManager.LoadScene(sceneName: mapName);
        }

        public void OnCharacterUnvalidated()
        {
            NoticeUtils.ShowNotice(message: NoticeMessages.CharacterView.CharacterValidationFailed);
        }

        public void OnCharacterDeletionSucceed()
        {
            HideChooseFighterView();
            ShowLoadingView();

            RemoveAndShowAllCharacterImages();
        }

        public void OnCharacterDeletionFailed()
        {
            NoticeUtils.ShowNotice(message: NoticeMessages.CharacterView.CharacterDeletionFailed);
        }

        public void OnCharacterCreated()
        {
            HideChooseFighterView();
            ShowLoadingView();

            RemoveAndShowAllCharacterImages();
        }

        public void OnCreateCharacterFailed(UICharacterCreationFailed reason)
        {
            switch (reason)
            {
                case UICharacterCreationFailed.Unknown:
                {
                    NoticeUtils.ShowNotice(message: NoticeMessages.CharacterView.CharacterCreationFailed);
                    break;
                }

                case UICharacterCreationFailed.NameAlreadyInUse:
                {
                    NoticeUtils.ShowNotice(message: NoticeMessages.CharacterView.NameAlreadyInUse);
                    break;
                }
            }
        }

        private void OnLoadingAnimationFinished()
        {
            UnsubscribeFromLoadingView();

            characterViewInteractor.ConnectToGameServer();
        }

        private void RemoveAndShowAllCharacterImages()
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
                        var image = characterImage.GameObject;
                        if (image != null)
                        {
                            Destroy(image);
                        }
                    }
                }
            }
        }

        private void ShowChooseFighterView()
        {
            chooseFighterView?.Show();
        }

        private void ShowLoadingView()
        {
            loadingView?.Show();
        }

        private void HideChooseFighterView()
        {
            chooseFighterView?.Hide();
        }

        private void HideLoadingView()
        {
            loadingView?.Hide();
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
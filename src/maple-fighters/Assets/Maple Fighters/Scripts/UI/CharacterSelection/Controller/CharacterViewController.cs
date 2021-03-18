using System.Text.RegularExpressions;
using Scripts.Constants;
using Scripts.UI.MenuBackground;
using Scripts.UI.Notice;
using Scripts.UI.ScreenFade;
using UI;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Scripts.UI.CharacterSelection
{
    [RequireComponent(typeof(CharacterViewInteractor))]
    public class CharacterViewController : MonoBehaviour,
                                           IOnCharacterReceivedListener,
                                           IOnCharacterValidationFinishedListener,
                                           IOnCharacterDeletionFinishedListener,
                                           IOnCharacterCreationFinishedListener
    {
        [SerializeField]
        private int minCharacterNameLength;

        private ILoadingView loadingView;
        private IChooseFighterView chooseFighterView;
        private ICharacterView characterView;
        private ICharacterSelectionOptionsView characterSelectionOptionsView;
        private ICharacterSelectionView characterSelectionView;
        private ICharacterNameView characterNameView;

        private CharacterViewCollection? characterViewCollection;
        private int characterIndex = -1;
        private UINewCharacterDetails characterDetails;

        private CharacterViewInteractor characterViewInteractor;
        private ScreenFadeController screenFadeController;

        private string mapName;

        private void Awake()
        {
            characterViewInteractor = GetComponent<CharacterViewInteractor>();
            screenFadeController = FindObjectOfType<ScreenFadeController>();
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
            chooseFighterView = UICreator
                .GetInstance()
                .Create<ChooseFighterText>(UICanvasLayer.Background, UIIndex.End);
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
                characterSelectionOptionsView.StartButtonClicked +=
                    OnStartButtonClicked;
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
                characterSelectionView.ChooseButtonClicked +=
                    OnChooseButtonClicked;
                characterSelectionView.CancelButtonClicked +=
                    OnCancelButtonClicked;
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
            ShowChooseFighterView();
        }

        public void OnCharacterValidated(string mapName)
        {
            this.mapName = mapName;

            if (screenFadeController != null)
            {
                screenFadeController.Show();
                screenFadeController.FadeInCompleted += OnFadeInCompleted;
            }
        }

        private void OnFadeInCompleted()
        {
            if (screenFadeController != null)
            {
                screenFadeController.FadeInCompleted -= OnFadeInCompleted;
            }

            SceneManager.LoadScene(sceneName: mapName);
        }

        public void OnCharacterUnvalidated()
        {
            NoticeUtils.ShowNotice(message: NoticeMessages.CharacterView.ValidationFailed);
        }

        public void OnCharacterDeletionSucceed()
        {
            HideChooseFighterView();
            ShowLoadingView();

            RemoveAndShowAllCharacterImages();
        }

        public void OnCharacterDeletionFailed()
        {
            NoticeUtils.ShowNotice(message: NoticeMessages.CharacterView.DeletionFailed);
        }

        public void OnCharacterCreated()
        {
            ResetCharacterSelection();
            ResetCharacterName();

            HideCharacterNameWindow();
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
                    NoticeUtils.ShowNotice(message: NoticeMessages.CharacterView.CreationFailed);
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

            CreateChooseFighterView();
            CreateCharacterView();
            CreateAndSubscribeToCharacterSelectionOptionsWindow();
            CreateAndSubscribeToCharacterSelectionWindow();
            CreateAndSubscribeToCharacterNameWindow();

            SubscribeToBackgroundClicked();

            characterViewInteractor.GetCharacters();
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
                        var gameObject = characterImage.GameObject;
                        if (gameObject != null)
                        {
                            Destroy(gameObject);
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
            characterIndex = (int)uiCharacterIndex;

            HideCharacterNameWindow();
            HideCharacterSelectionWindow();
            HideChooseFighterView();

            ShowCharacterSelectionOptionsWindow();

            EnableOrDisableCharacterSelectionOptionsViewButtons(hasCharacter);
        }

        private void OnStartButtonClicked()
        {
            if (characterIndex != -1)
            {
                var character = characterViewCollection?.Get(characterIndex);
                if (character != null)
                {
                    var characterClass = (byte)character.CharacterClass;
                    var characterName = character.CharacterName;

                    characterViewInteractor.ValidateCharacter(characterClass, characterName);
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

        private void OnChooseButtonClicked()
        {
            HideCharacterSelectionWindow();
            ShowCharacterNameWindow();
        }

        private void OnCancelButtonClicked()
        {
            ResetCharacterSelection();
            ResetCharacterName();

            HideCharacterSelectionWindow();
            ShowCharacterSelectionOptionsWindow();
        }

        private void OnCharacterSelected(UICharacterClass uiCharacterClass)
        {
            characterDetails.SetCharacterClass(uiCharacterClass);

            characterSelectionView.EnableChooseButton();
            characterSelectionView.ResetSelection();
            characterSelectionView.SelectCharacterClass(uiCharacterClass);
        }

        private void ResetCharacterSelection()
        {
            characterSelectionView.ResetSelection();
            characterSelectionView.DisableChooseButton();
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

            ShowChooseFighterView();
        }

        private void ShowCharacterNameWindow()
        {
            characterNameView?.Show();
        }

        private void HideCharacterNameWindow()
        {
            if (characterNameView != null
                && characterNameView.IsShown)
            {
                characterNameView?.Hide();
            }
        }

        private void ShowCharacterSelectionWindow()
        {
            characterSelectionView?.Show();
        }

        private void HideCharacterSelectionWindow()
        {
            if (characterSelectionView != null
                && characterSelectionView.IsShown)
            {
                characterSelectionView?.Hide();
            }
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
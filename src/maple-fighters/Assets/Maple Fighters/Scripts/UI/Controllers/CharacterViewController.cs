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

        private ClickableCharacterImageCollection clickableCharacterImageCollection;

        private CharacterView characterView;
        private CharacterSelectionOptionsWindow characterSelectionOptionsWindow;

        private CharacterSelectionController characterSelectionController;

        private void Awake()
        {
            clickableCharacterImageCollection =
                new ClickableCharacterImageCollection();
            characterView = 
                UIElementsCreator.GetInstance().Create<CharacterView>(
                    UILayer.Background,
                    UIIndex.End);
        }

        private void Start()
        {
            // TODO: Use event bus system
            characterSelectionController =
                FindObjectOfType<CharacterSelectionController>();
        }

        private void OnDestroy()
        {
            foreach (var characterImage in 
                clickableCharacterImageCollection.GetCharacterImages())
            {
                if (characterImage != null)
                {
                    characterImage.CharacterClicked -= OnCharacterClicked;

                    Destroy(characterImage.gameObject);
                }
            }

            if (characterView != null)
            {
                Destroy(characterView.gameObject);
            }

            if (characterSelectionOptionsWindow != null)
            {
                characterSelectionOptionsWindow.StartButtonClicked -=
                    OnStartButtonClicked;
                characterSelectionOptionsWindow.CreateCharacterButtonClicked -=
                    OnCreateCharacterButtonClicked;
                characterSelectionOptionsWindow.DeleteCharacterButtonClicked -=
                    OnDeleteCharacterButtonClicked;
            }
        }

        public void CreateCharacter(UICharacterDetails uiCharacterDetails)
        {
            var characterGameObject =
                UIManagerUtils.LoadAndCreateGameObject(
                    GetCharacterPath(uiCharacterDetails));
            var clickableCharacterImage = 
                characterGameObject.GetComponent<ClickableCharacterImage>();
            if (clickableCharacterImage != null)
            {
                clickableCharacterImage.SetCharacterDetails(uiCharacterDetails);
                clickableCharacterImage
                    .SetCharacterName(uiCharacterDetails.GetCharacterName());
                clickableCharacterImage.CharacterClicked += OnCharacterClicked;

                clickableCharacterImageCollection.SetCharacterImage(
                    uiCharacterDetails.GetCharacterIndex(),
                    clickableCharacterImage);
            }
        }

        private void ShowCharacterSelectionOptionsWindow()
        {
            if (characterSelectionOptionsWindow == null)
            {
                characterSelectionOptionsWindow = UIElementsCreator
                    .GetInstance().Create<CharacterSelectionOptionsWindow>();
                characterSelectionOptionsWindow.StartButtonClicked +=
                    OnStartButtonClicked;
                characterSelectionOptionsWindow.CreateCharacterButtonClicked +=
                    OnCreateCharacterButtonClicked;
                characterSelectionOptionsWindow.DeleteCharacterButtonClicked +=
                    OnDeleteCharacterButtonClicked;
            }

            var uiCharacterDetails =
                characterSelectionController.GetCharacterDetails();

            characterSelectionOptionsWindow
                .EnableOrDisableStartButton(uiCharacterDetails.HasCharacter());

            characterSelectionOptionsWindow
                .EnableOrDisableCreateCharacterButton(
                    !uiCharacterDetails.HasCharacter());

            characterSelectionOptionsWindow
                .EnableOrDisableDeleteCharacterButton(
                    uiCharacterDetails.HasCharacter());

            characterSelectionOptionsWindow.Show();
        }

        private void OnCharacterClicked(UICharacterDetails uiCharacterDetails)
        {
            characterSelectionController.SetCharacterDetails(
                uiCharacterDetails);

            ShowCharacterSelectionOptionsWindow();

            var clickableCharacter =
                clickableCharacterImageCollection
                    .GetCharacterImage(uiCharacterDetails.GetCharacterIndex());

            CharacterSelected?.Invoke(clickableCharacter);
        }
        
        private void OnStartButtonClicked()
        {
            var characterDetails =
                characterSelectionController.GetCharacterDetails();

            CharacterStarted?.Invoke((int)characterDetails.GetCharacterIndex());
        }

        private void OnCreateCharacterButtonClicked()
        {
            characterSelectionController.ShowCharacterSelectionWindow();
        }

        private void OnDeleteCharacterButtonClicked()
        {
            var characterDetails =
                characterSelectionController.GetCharacterDetails();

            CharacterRemoved?.Invoke((int)characterDetails.GetCharacterIndex());
        }

        private string GetCharacterPath(UICharacterDetails uiCharacterDetails)
        {
            const string CharactersPath = "Characters/{0}";

            var index = (int)uiCharacterDetails.GetCharacterIndex();
            var name = 
                uiCharacterDetails.HasCharacter()
                    ? $"{uiCharacterDetails.GetCharacterClass()} {index}"
                    : $"Sample {index}";

            return string.Format(CharactersPath, name);
        }
    }
}
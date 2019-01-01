using System;
using CommonTools.Log;
using Scripts.UI.Core;
using UnityEngine;
using UnityEngine.UI;

namespace Scripts.UI.Windows
{
    public class CharacterSelectionOptionsWindow : UserInterfaceWindowFadeEffect
    {
        public event Action StartButtonClicked;

        public event Action CreateCharacterButtonClicked;

        public event Action DeleteCharacterButtonClicked;

        public event Action PlayCharacterIdleAnimation;

        [SerializeField] private Button startButton;
        [SerializeField] private Button createCharacterButton;
        [SerializeField] private Button deleteCharacterButton;

        private void Start()
        {
            startButton.onClick.AddListener(OnStartClicked);
            createCharacterButton.onClick.AddListener(OnCreateCharacterClicked);
            deleteCharacterButton.onClick.AddListener(OnDeleteCharacterClicked);

            SubscribeToMouseDetectionBackgroundEvent();
        }

        private void OnDestroy()
        {
            startButton.onClick.RemoveListener(OnStartClicked);
            createCharacterButton.onClick.RemoveListener(OnCreateCharacterClicked);
            deleteCharacterButton.onClick.RemoveListener(OnDeleteCharacterClicked);

            UnsubscribeFromMouseDetectionBackgroundEvent();
        }

        public override void Hide()
        {
            UserInterfaceContainer.GetInstance()?.Remove(this);

            PlayCharacterIdleAnimation?.Invoke();
        }

        private void SubscribeToMouseDetectionBackgroundEvent()
        {
            var screenMouseDetection = UserInterfaceContainer.GetInstance().Get<MouseDetectionBackground>().AssertNotNull();
            screenMouseDetection.MouseClicked += Hide;
            screenMouseDetection.MouseClicked += ShowChooseFighterText;
        }

        private void UnsubscribeFromMouseDetectionBackgroundEvent()
        {
            var screenMouseDetection = UserInterfaceContainer.GetInstance()?.Get<MouseDetectionBackground>().AssertNotNull();
            if (screenMouseDetection != null)
            {
                screenMouseDetection.MouseClicked -= Hide;
                screenMouseDetection.MouseClicked -= ShowChooseFighterText;
            }
        }

        private void ShowChooseFighterText()
        {
            var chooseFighterText = UserInterfaceContainer.GetInstance().Get<ChooseFighterText>().AssertNotNull();
            chooseFighterText.Show();
        }

        private void OnStartClicked()
        {
            Hide();

            StartButtonClicked?.Invoke();
        }

        private void OnCreateCharacterClicked()
        {
            Hide();

            CreateCharacterButtonClicked?.Invoke();
        }

        private void OnDeleteCharacterClicked()
        {
            Hide();

            DeleteCharacterButtonClicked?.Invoke();
        }

        public void StartButtonInteraction(bool interctable)
        {
            startButton.interactable = interctable;
        }

        public void CreateCharacterButtonInteraction(bool interctable)
        {
            createCharacterButton.interactable = interctable;
        }

        public void DeleteCharacterButtonInteraction(bool interctable)
        {
            deleteCharacterButton.interactable = interctable;
        }
    }
}
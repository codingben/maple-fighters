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

        public void StartButtonInteraction(bool interctable) => startButton.interactable = interctable;
        public void CreateCharacteButtonInteraction(bool interctable) => createCharacterButton.interactable = interctable;
        public void DeleteCharacterButtonInteraction(bool interctable) => deleteCharacterButton.interactable = interctable;

        [SerializeField] private Button startButton;
        [SerializeField] private Button createCharacterButton;
        [SerializeField] private Button deleteCharacterButton;

        private Action removeMeAction;

        private void Start()
        {
            removeMeAction = RemoveMe;

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

        private void SubscribeToMouseDetectionBackgroundEvent()
        {
            var screenMouseDetection = UserInterfaceContainer.Instance.Get<MouseDetectionBackground>().AssertNotNull();
            screenMouseDetection.MouseClicked += removeMeAction;
        }

        private void UnsubscribeFromMouseDetectionBackgroundEvent()
        {
            var screenMouseDetection = UserInterfaceContainer.Instance.Get<MouseDetectionBackground>().AssertNotNull();
            screenMouseDetection.MouseClicked -= removeMeAction;
        }

        private void RemoveMe()
        {
            UserInterfaceContainer.Instance.Remove(this);
        }

        private void OnStartClicked()
        {
            StartButtonClicked?.Invoke();
        }

        private void OnCreateCharacterClicked()
        {
            CreateCharacterButtonClicked?.Invoke();
        }

        private void OnDeleteCharacterClicked()
        {
            DeleteCharacterButtonClicked?.Invoke();
        }
    }
}
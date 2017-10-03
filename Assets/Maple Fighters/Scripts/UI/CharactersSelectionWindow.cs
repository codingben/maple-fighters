using System;
using CommonTools.Log;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Scripts.UI
{
    public class CharactersSelectionWindow : UserInterfaceWindowFadeEffect
    {
        public event Action Choosed;
        public event Action KnightSelected;
        public event Action ArrowSelected;
        public event Action WizardSelected;
        public event Action Deselected;

        [Header("Buttons")]
        [SerializeField] private Button ChooseButton;
        [SerializeField] private Button knightButton;
        [SerializeField] private Button arrowButton;
        [SerializeField] private Button wizardButton;
        [Header("Selected Images")]
        [SerializeField] private GameObject knightSelectedImage;
        [SerializeField] private GameObject arrowSelectedImage;
        [SerializeField] private GameObject wizardSelectedImage;
        [Header("Texts")]
        [SerializeField] private TextMeshProUGUI knightName;
        [SerializeField] private TextMeshProUGUI arrowName;
        [SerializeField] private TextMeshProUGUI wizardName;

        private void Start()
        {
            ChooseButton.onClick.AddListener(OnChooseClicked);
            knightButton.onClick.AddListener(OnKnightSelected);
            arrowButton.onClick.AddListener(OnArrowSelected);
            wizardButton.onClick.AddListener(OnWizardSelected);

            SubscribeToMouseDetectionBackgroundEvent();
        }

        private void SubscribeToMouseDetectionBackgroundEvent()
        {
            var screenMouseDetection = UserInterfaceContainer.Instance.Get<MouseDetectionBackground>().AssertNotNull();
            screenMouseDetection.MouseClicked += DeactiveAllSelectedClasses;
            screenMouseDetection.MouseClicked += OnDeselected;
        }

        private void UnsubscribeFromMouseDetectionBackgroundEvent()
        {
            var screenMouseDetection = UserInterfaceContainer.Instance.Get<MouseDetectionBackground>().AssertNotNull();
            screenMouseDetection.MouseClicked -= DeactiveAllSelectedClasses;
            screenMouseDetection.MouseClicked -= OnDeselected;
        }

        private void OnDestroy()
        {
            UnsubscribeFromMouseDetectionBackgroundEvent();
        }

        private void OnKnightSelected()
        {
            DeactiveAllSelectedClasses();

            knightSelectedImage.SetActive(true);
            knightName.fontStyle = FontStyles.Bold;

            KnightSelected?.Invoke();
        }

        private void OnArrowSelected()
        {
            DeactiveAllSelectedClasses();

            arrowSelectedImage.SetActive(true);
            arrowName.fontStyle = FontStyles.Bold;

            ArrowSelected?.Invoke();
        }

        private void OnWizardSelected()
        {
            DeactiveAllSelectedClasses();

            wizardSelectedImage.SetActive(true);
            wizardName.fontStyle = FontStyles.Bold;

            WizardSelected?.Invoke();
        }

        private void OnChooseClicked()
        {
            Choosed?.Invoke();
        }

        private void DeactiveAllSelectedClasses()
        {
            knightName.fontStyle = FontStyles.SmallCaps;
            arrowName.fontStyle = FontStyles.SmallCaps;
            wizardName.fontStyle = FontStyles.SmallCaps;

            knightSelectedImage.SetActive(false);
            arrowSelectedImage.SetActive(false);
            wizardSelectedImage.SetActive(false);
        }

        private void OnDeselected()
        {
            Deselected?.Invoke();
        }
    }
}
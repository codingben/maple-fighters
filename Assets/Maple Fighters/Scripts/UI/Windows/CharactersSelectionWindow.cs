using System;
using Scripts.UI.Core;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Scripts.UI.Windows
{
    public class CharactersSelectionWindow : UserInterfaceWindowFadeEffect
    {
        public event Action ChoosedClicked;
        public event Action CancelClicked;
        public event Action KnightSelected;
        public event Action ArrowSelected;
        public event Action WizardSelected;
        public event Action Deselected;

        public Action DeactiveAll;

        [SerializeField] private MouseDetectionBackground screenMouseDetection;
        [Header("Buttons")]
        [SerializeField] private Button cancelButton;
        [SerializeField] private Button chooseButton;
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
            DeactiveAll = Deactive;

            cancelButton.onClick.AddListener(OnCancelClicked);
            chooseButton.onClick.AddListener(OnChooseClicked);
            knightButton.onClick.AddListener(OnKnightSelected);
            arrowButton.onClick.AddListener(OnArrowSelected);
            wizardButton.onClick.AddListener(OnWizardSelected);

            SubscribeToMouseDetectionBackgroundEvent();
        }

        private void OnDestroy()
        {
            cancelButton.onClick.RemoveListener(OnCancelClicked);
            chooseButton.onClick.RemoveListener(OnChooseClicked);
            knightButton.onClick.RemoveListener(OnKnightSelected);
            arrowButton.onClick.RemoveListener(OnArrowSelected);
            wizardButton.onClick.RemoveListener(OnWizardSelected);

            UnsubscribeFromMouseDetectionBackgroundEvent();
        }

        private void SubscribeToMouseDetectionBackgroundEvent()
        {
            screenMouseDetection.MouseClicked += DeactiveAllSelectedClasses;
            screenMouseDetection.MouseClicked += OnDeselected;
        }

        private void UnsubscribeFromMouseDetectionBackgroundEvent()
        {
            screenMouseDetection.MouseClicked -= DeactiveAllSelectedClasses;
            screenMouseDetection.MouseClicked -= OnDeselected;
        }

        private void OnKnightSelected()
        {
            DeactiveAllSelectedClasses();

            knightSelectedImage.SetActive(true);
            knightName.fontStyle = FontStyles.Bold;

            chooseButton.interactable = true;

            KnightSelected?.Invoke();
        }

        private void OnArrowSelected()
        {
            DeactiveAllSelectedClasses();

            arrowSelectedImage.SetActive(true);
            arrowName.fontStyle = FontStyles.Bold;

            chooseButton.interactable = true;

            ArrowSelected?.Invoke();
        }

        private void OnWizardSelected()
        {
            DeactiveAllSelectedClasses();

            wizardSelectedImage.SetActive(true);
            wizardName.fontStyle = FontStyles.Bold;

            chooseButton.interactable = true;

            WizardSelected?.Invoke();
        }

        private void OnCancelClicked()
        {
            Hide();

            chooseButton.interactable = false;

            DeactiveAllSelectedClasses();

            CancelClicked?.Invoke();
        }

        private void OnChooseClicked()
        {
            Hide();

            ChoosedClicked?.Invoke();
        }

        private void DeactiveAllSelectedClasses()
        {
            knightName.fontStyle = FontStyles.Normal;
            arrowName.fontStyle = FontStyles.Normal;
            wizardName.fontStyle = FontStyles.Normal;

            knightSelectedImage.SetActive(false);
            arrowSelectedImage.SetActive(false);
            wizardSelectedImage.SetActive(false);
        }

        private void Deactive()
        {
            chooseButton.interactable = false;

            DeactiveAllSelectedClasses();
        }

        private void OnDeselected()
        {
            chooseButton.interactable = false;

            Deselected?.Invoke();
        }
    }
}
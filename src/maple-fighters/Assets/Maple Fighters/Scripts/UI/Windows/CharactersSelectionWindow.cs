using System;
using TMPro;
using UI.Manager;
using UnityEngine;
using UnityEngine.UI;

namespace Scripts.UI.Windows
{
    [RequireComponent(typeof(UIFadeAnimation))]
    public class CharactersSelectionWindow : UIElement
    {
        public event Action KnightSelected;

        public event Action ArrowSelected;

        public event Action WizardSelected;

        public event Action ChooseButtonClicked;

        public event Action CancelButtonClicked;

        [Header("Buttons")]
        [SerializeField]
        private Button cancelButton;

        [SerializeField]
        private Button chooseButton;

        [SerializeField]
        private Button knightButton;

        [SerializeField]
        private Button arrowButton;

        [SerializeField]
        private Button wizardButton;

        [Header("Images")]
        [SerializeField]
        private GameObject knightSelectedImage;

        [SerializeField]
        private GameObject arrowSelectedImage;

        [SerializeField]
        private GameObject wizardSelectedImage;

        [Header("Texts")]
        [SerializeField]
        private TextMeshProUGUI knightName;

        [SerializeField]
        private TextMeshProUGUI arrowName;

        [SerializeField]
        private TextMeshProUGUI wizardName;

        private enum Class
        {
            /// <summary>
            /// The knight.
            /// </summary>
            Knight,

            /// <summary>
            /// The arrow.
            /// </summary>
            Arrow,

            /// <summary>
            /// The wizard.
            /// </summary>
            Wizard
        }

        private void Start()
        {
            cancelButton.onClick.AddListener(OnCancelClicked);
            chooseButton.onClick.AddListener(OnChooseClicked);
            knightButton.onClick.AddListener(OnKnightSelected);
            arrowButton.onClick.AddListener(OnArrowSelected);
            wizardButton.onClick.AddListener(OnWizardSelected);
        }

        private void OnDestroy()
        {
            cancelButton.onClick.RemoveListener(OnCancelClicked);
            chooseButton.onClick.RemoveListener(OnChooseClicked);
            knightButton.onClick.RemoveListener(OnKnightSelected);
            arrowButton.onClick.RemoveListener(OnArrowSelected);
            wizardButton.onClick.RemoveListener(OnWizardSelected);
        }

        private void OnKnightSelected()
        {
            SetClassSelection(Class.Knight);

            KnightSelected?.Invoke();
        }

        private void OnArrowSelected()
        {
            SetClassSelection(Class.Arrow);

            ArrowSelected?.Invoke();
        }

        private void OnWizardSelected()
        {
            SetClassSelection(Class.Wizard);

            WizardSelected?.Invoke();
        }

        private void OnCancelClicked()
        {
            Hide();

            ResetSelectedClasses();

            CancelButtonClicked?.Invoke();
        }

        private void OnChooseClicked()
        {
            Hide();

            ChooseButtonClicked?.Invoke();
        }

        private void SetClassSelection(Class character)
        {
            ResetSelectedClasses();

            switch (character)
            {
                case Class.Knight:
                {
                    knightSelectedImage.SetActive(true);
                    knightName.fontStyle = FontStyles.Bold;
                    break;
                }

                case Class.Arrow:
                {
                    arrowSelectedImage.SetActive(true);
                    arrowName.fontStyle = FontStyles.Bold;
                    break;
                }

                case Class.Wizard:
                {
                    wizardSelectedImage.SetActive(true);
                    wizardName.fontStyle = FontStyles.Bold;
                    break;
                }
            }

            chooseButton.interactable = true;
        }

        public void ResetSelectedClasses()
        {
            chooseButton.interactable = false;

            knightName.fontStyle = FontStyles.Normal;
            arrowName.fontStyle = FontStyles.Normal;
            wizardName.fontStyle = FontStyles.Normal;

            knightSelectedImage.SetActive(false);
            arrowSelectedImage.SetActive(false);
            wizardSelectedImage.SetActive(false);
        }
    }
}
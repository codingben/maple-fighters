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
            if (chooseButton != null)
            {
                chooseButton.onClick.AddListener(OnChooseButtonClicked);
            }

            if (cancelButton != null)
            {
                cancelButton.onClick.AddListener(OnCancelButtonClicked);
            }

            if (knightButton != null)
            {
                knightButton.onClick.AddListener(OnKnightSelected);
            }

            if (arrowButton != null)
            {
                arrowButton.onClick.AddListener(OnArrowSelected);
            }

            if (wizardButton != null)
            {
                wizardButton.onClick.AddListener(OnWizardSelected);
            }
        }

        private void OnDestroy()
        {
            if (chooseButton != null)
            {
                chooseButton.onClick.RemoveListener(OnChooseButtonClicked);
            }

            if (cancelButton != null)
            {
                cancelButton.onClick.RemoveListener(OnCancelButtonClicked);
            }

            if (knightButton != null)
            {
                knightButton.onClick.RemoveListener(OnKnightSelected);
            }

            if (arrowButton != null)
            {
                arrowButton.onClick.RemoveListener(OnArrowSelected);
            }

            if (wizardButton != null)
            {
                wizardButton.onClick.RemoveListener(OnWizardSelected);
            }
        }

        private void OnChooseButtonClicked()
        {
            Hide();

            ChooseButtonClicked?.Invoke();
        }

        private void OnCancelButtonClicked()
        {
            Hide();

            ResetSelectedClasses();

            CancelButtonClicked?.Invoke();
        }

        private void OnKnightSelected()
        {
            SelectClass(Class.Knight);

            KnightSelected?.Invoke();
        }

        private void OnArrowSelected()
        {
            SelectClass(Class.Arrow);

            ArrowSelected?.Invoke();
        }

        private void OnWizardSelected()
        {
            SelectClass(Class.Wizard);

            WizardSelected?.Invoke();
        }

        private void SelectClass(Class character)
        {
            ResetSelectedClasses();

            switch (character)
            {
                case Class.Knight:
                {
                    if (knightSelectedImage != null)
                    {
                        knightSelectedImage.SetActive(true);
                    }

                    if (knightName != null)
                    {
                        knightName.fontStyle = FontStyles.Bold;
                    }

                    break;
                }

                case Class.Arrow:
                {
                    if (arrowSelectedImage != null)
                    {
                        arrowSelectedImage.SetActive(true);
                    }

                    if (arrowName != null)
                    {
                        arrowName.fontStyle = FontStyles.Bold;
                    }

                    break;
                }

                case Class.Wizard:
                {
                    if (wizardSelectedImage != null)
                    {
                        wizardSelectedImage.SetActive(true);
                    }

                    if (wizardName != null)
                    {
                        wizardName.fontStyle = FontStyles.Bold;
                    }

                    break;
                }
            }

            if (chooseButton != null)
            {
                chooseButton.interactable = true;
            }
        }

        public void ResetSelectedClasses()
        {
            if (chooseButton != null)
            {
                chooseButton.interactable = false;
            }

            if (knightName != null)
            {
                knightName.fontStyle = FontStyles.Normal;
            }

            if (arrowName != null)
            {
                arrowName.fontStyle = FontStyles.Normal;
            }

            if (wizardName != null)
            {
                wizardName.fontStyle = FontStyles.Normal;
            }

            if (knightSelectedImage != null)
            {
                knightSelectedImage.SetActive(false);
            }

            if (arrowSelectedImage != null)
            {
                arrowSelectedImage.SetActive(false);
            }

            if (wizardSelectedImage != null)
            {
                wizardSelectedImage.SetActive(false);
            }
        }
    }
}
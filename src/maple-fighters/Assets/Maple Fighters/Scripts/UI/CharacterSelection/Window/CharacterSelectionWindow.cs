using System;
using UI;
using UnityEngine;
using UnityEngine.UI;

namespace Scripts.UI.CharacterSelection
{
    [RequireComponent(typeof(UIFadeAnimation))]
    public class CharacterSelectionWindow : UIElement, ICharacterSelectionView
    {
        public event Action<UICharacterClass> CharacterSelected;

        [Header("Buttons")]
        [SerializeField]
        private Button knightButton;

        [SerializeField]
        private Button archerButton;

        [SerializeField]
        private Button wizardButton;

        private void Start()
        {
            knightButton?.onClick.AddListener(OnKnightSelected);
            archerButton?.onClick.AddListener(OnArcherSelected);
            wizardButton?.onClick.AddListener(OnWizardSelected);
        }

        private void OnDestroy()
        {
            knightButton?.onClick.RemoveListener(OnKnightSelected);
            archerButton?.onClick.RemoveListener(OnArcherSelected);
            wizardButton?.onClick.RemoveListener(OnWizardSelected);
        }

        private void OnKnightSelected()
        {
            OnCharacterSelected(UICharacterClass.Knight);
        }

        private void OnArcherSelected()
        {
            OnCharacterSelected(UICharacterClass.Archer);
        }

        private void OnWizardSelected()
        {
            OnCharacterSelected(UICharacterClass.Wizard);
        }

        private void OnCharacterSelected(UICharacterClass uiCharacterClass)
        {
            CharacterSelected?.Invoke(uiCharacterClass);
        }
    }
}
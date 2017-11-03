using System;
using Scripts.UI.Core;
using UnityEngine;
using UnityEngine.UI;

namespace Scripts.UI.Windows
{
    public class MinimapWindow : UserInterfaceWindow
    {
        public event Action<int> MarkSelectionChanged;

        [SerializeField] private Dropdown markSelection;

        private void Start()
        {
            markSelection.onValueChanged.AddListener(OnMarkSelectionDropdownChanged);
        }

        private void OnMarkSelectionDropdownChanged(int selection)
        {
            MarkSelectionChanged?.Invoke(selection);
        }
    }
}
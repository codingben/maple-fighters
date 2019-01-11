using System;
using UI.Manager;
using UnityEngine;
using UnityEngine.UI;

namespace Scripts.UI.Windows
{
    public class MinimapWindow : UIElement
    {
        public event Action<int> MarkSelectionChanged;

        [Header("Dropdown"), SerializeField]
        private Dropdown markSelection;

        private void Start()
        {
            markSelection.onValueChanged.AddListener(OnMarkSelectionChanged);
        }

        private void OnMarkSelectionChanged(int selection)
        {
            MarkSelectionChanged?.Invoke(selection);
        }
    }
}
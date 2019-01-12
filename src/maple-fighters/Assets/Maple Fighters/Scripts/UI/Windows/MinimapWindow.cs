using System;
using UI.Manager;
using UnityEngine;
using UnityEngine.UI;

namespace Scripts.UI.Windows
{
    [RequireComponent(typeof(UICanvasGroup))]
    public class MinimapWindow : UIElement
    {
        public event Action<int> MarkSelectionChanged;

        [Header("Dropdown"), SerializeField]
        private Dropdown markSelection;

        private void Start()
        {
            if (markSelection != null)
            {
                markSelection.onValueChanged.AddListener(
                    OnMarkSelectionChanged);
            }
        }

        private void OnMarkSelectionChanged(int selection)
        {
            MarkSelectionChanged?.Invoke(selection);
        }
    }
}
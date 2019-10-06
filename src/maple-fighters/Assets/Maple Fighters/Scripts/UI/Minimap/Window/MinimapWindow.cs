using System;
using UI.Manager;
using UnityEngine;
using UnityEngine.UI;

namespace Scripts.UI.Minimap
{
    [RequireComponent(typeof(UICanvasGroup))]
    public class MinimapWindow : UIElement, IMinimapView
    {
        public event Action<int> MarkSelectionChanged;

        [Header("Dropdown"), SerializeField]
        private Dropdown markSelection;

        private void Start()
        {
            markSelection?.onValueChanged.AddListener(OnMarkSelectionChanged);
        }

        private void OnDestroy()
        {
            markSelection?.onValueChanged.RemoveListener(OnMarkSelectionChanged);
        }

        private void OnMarkSelectionChanged(int index)
        {
            MarkSelectionChanged?.Invoke(index);
        }
    }
}
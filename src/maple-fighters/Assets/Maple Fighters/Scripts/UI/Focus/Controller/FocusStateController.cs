using System;
using Scripts.Editor;
using UnityEngine;

namespace Scripts.UI.Focus
{
    public class FocusStateController : MonoBehaviour
    {
        public event Action<UIFocusState> FocusChanged;

        [ViewOnly, SerializeField]
        private UIFocusState focusState = UIFocusState.Game;

        public void ChangeFocusState(UIFocusState focusState)
        {
            this.focusState = focusState;

            FocusChanged?.Invoke(focusState);
        }

        public UIFocusState GetFocusState()
        {
            return focusState;
        }
    }
}
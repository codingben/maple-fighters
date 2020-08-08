using UnityEngine;

namespace Scripts.UI.Focus
{
    public class FocusStateController : MonoBehaviour
    {
        private UIFocusState focusState = UIFocusState.Game;

        public void ChangeFocusState(UIFocusState focusState)
        {
            this.focusState = focusState;
        }

        public UIFocusState GetFocusState()
        {
            return focusState;
        }
    }
}
using System;
using UnityEngine;

namespace Scripts.UI.Controllers
{
    public class FocusStateController : MonoBehaviour
    {
        public event Action<FocusState> FocusStateChanged;

        private FocusState focusState = FocusState.Game;

        public void ChangeFocusState(FocusState focusState)
        {
            this.focusState = focusState;

            FocusStateChanged?.Invoke(focusState);
        }

        public FocusState GetFocusState()
        {
            return focusState;
        }
    }
}
using System;
using Scripts.Utils;

namespace Scripts.UI.Controllers
{
    public class FocusController : MonoSingleton<FocusController>
    {
        public event Action<Focusable> StateChanged;

        public Focusable Focusable { get; private set; } = Focusable.Game;

        public void SetState(Focusable focusable)
        {
            Focusable = focusable;

            StateChanged?.Invoke(focusable);
        }
    }
}
using System;
using Scripts.Utils;

namespace Scripts.UI.Controllers
{
    public enum Focusable
    {
        /// <summary>
        /// The game.
        /// </summary>
        Game,

        /// <summary>
        /// The chat.
        /// </summary>
        Chat,

        /// <summary>
        /// The ui.
        /// </summary>
        UI
    }

    public class FocusController : DontDestroyOnLoad<FocusController>
    {
        public event Action<Focusable> FocusableStateChanged;

        public Focusable Focusable { get; private set; } = Focusable.Game;

        public void SetState(Focusable focusable)
        {
            Focusable = focusable;

            FocusableStateChanged?.Invoke(focusable);
        }
    }
}
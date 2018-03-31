using Scripts.Utils;

namespace Scripts.UI.Controllers
{
    public enum Focusable
    {
        Game,
        Chat,
        UI
    }

    public class FocusController : DontDestroyOnLoad<FocusController>
    {
        public Focusable Focusable { get; set; } = Focusable.Game;
    }
}
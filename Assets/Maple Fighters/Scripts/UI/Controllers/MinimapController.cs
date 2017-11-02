using Scripts.UI.Core;
using Scripts.UI.Windows;
using Scripts.Utils;

namespace Scripts.UI.Controllers
{
    public class MinimapController : DontDestroyOnLoad<MinimapController>
    {
        private MinimapWindow minimapWindow;

        private void Start()
        {
            minimapWindow = UserInterfaceContainer.Instance.Add<MinimapWindow>();
        }
    }
}
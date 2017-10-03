using UnityEngine;

namespace Scripts.UI
{
    public class UniqueUserInterfaceBase : MonoBehaviour, IUserInterface
    {
        public GameObject GameObject { get; } = null;

        public void Show()
        {
            // Left blank intentionally
        }

        public void Hide()
        {
            // Left blank intentionally
        }
    }
}
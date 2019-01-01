using UnityEngine;

namespace Scripts.UI.Core
{
    public class UniqueUserInterfaceBase : MonoBehaviour, IUserInterface
    {
        public GameObject GameObject => gameObject;

        public virtual void Show()
        {
            // Left blank intentionally
        }

        public virtual void Hide()
        {
            // Left blank intentionally
        }
    }
}
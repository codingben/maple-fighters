using UnityEngine;

namespace Scripts.UI
{
    [RequireComponent(typeof(CanvasGroup))]
    public class UserInterfaceBase : MonoBehaviour, IUserInterface
    {
        public GameObject GameObject { get; private set; }
        protected CanvasGroup CanvasGroup;

        private void Awake()
        {
            GameObject = gameObject;
            CanvasGroup = GetComponent<CanvasGroup>();
        }

        public virtual void Show()
        {
            CanvasGroup.alpha = 1;
            CanvasGroup.blocksRaycasts = true;
        }

        public virtual void Hide()
        {
            CanvasGroup.alpha = 0;
            CanvasGroup.blocksRaycasts = false;
        }
    }
}
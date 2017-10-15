using UnityEngine;

namespace Scripts.UI.Core
{
    [RequireComponent(typeof(CanvasGroup))]
    public class UserInterfaceBase : MonoBehaviour, IUserInterface
    {
        public GameObject GameObject { get; private set; }
        protected CanvasGroup CanvasGroup;

        private void Awake()
        {
            OnAwake();
        }

        protected virtual void OnAwake()
        {
            GameObject = gameObject;
            CanvasGroup = GetComponent<CanvasGroup>();
        }

        public virtual void Show()
        {
            CanvasGroup.alpha = 1;
            CanvasGroup.blocksRaycasts = true;
            CanvasGroup.interactable = true;
        }

        public virtual void Hide()
        {
            CanvasGroup.alpha = 0;
            CanvasGroup.blocksRaycasts = false;
            CanvasGroup.interactable = false;
        }
    }
}
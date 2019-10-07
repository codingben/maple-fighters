using UnityEngine;

namespace UI.Manager
{
    [RequireComponent(typeof(UIElement), typeof(CanvasGroup))]
    public class UICanvasGroup : MonoBehaviour
    {
        protected float Alpha
        {
            get => canvasGroup.alpha;

            set => canvasGroup.alpha = value;
        }

        private UIElement uiElement;
        private CanvasGroup canvasGroup;

        private void Awake()
        {
            uiElement = GetComponent<UIElement>();
            canvasGroup = GetComponent<CanvasGroup>();

            SubscribeToUIElementEvents();
        }

        private void OnDestroy()
        {
            UnsubscribeFromUIElementEvents();
        }

        private void SubscribeToUIElementEvents()
        {
            uiElement.Shown += OnShown;
            uiElement.Hidden += OnHidden;
        }

        private void UnsubscribeFromUIElementEvents()
        {
            uiElement.Shown -= OnShown;
            uiElement.Hidden -= OnHidden;
        }

        protected virtual void OnShown()
        {
            EnableCanvasGroup();
        }

        protected virtual void OnHidden()
        {
            DisableCanvasGroup();
        }

        protected void EnableCanvasGroup()
        {
            canvasGroup.alpha = 1;
            canvasGroup.blocksRaycasts = true;
            canvasGroup.interactable = true;
        }

        protected void DisableCanvasGroup()
        {
            canvasGroup.alpha = 0;
            canvasGroup.blocksRaycasts = false;
            canvasGroup.interactable = false;
        }
    }
}
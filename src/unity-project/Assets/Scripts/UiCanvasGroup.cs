using UnityEngine;

namespace UserInterface
{
    [RequireComponent(typeof(UiElement), typeof(CanvasGroup))]
    public class UiCanvasGroup : MonoBehaviour
    {
        protected float Alpha
        {
            get
            {
                return canvasGroup.alpha;
            }

            set
            {
                canvasGroup.alpha = value;
            }
        }

        private UiElement uiElement;
        private CanvasGroup canvasGroup;

        private void Awake()
        {
            uiElement = GetComponent<UiElement>();
            canvasGroup = GetComponent<CanvasGroup>();

            SubscribeToUiElementEvents();
        }

        private void OnDestroy()
        {
            UnsubscribeFromUiElementEvents();
        }

        private void SubscribeToUiElementEvents()
        {
            uiElement.Shown += OnShown;
            uiElement.Hidden += OnHidden;
        }

        private void UnsubscribeFromUiElementEvents()
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
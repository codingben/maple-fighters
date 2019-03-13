using UI.Manager;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Sample.Scripts
{
    public class SampleWindowController : MonoBehaviour
    {
        private SampleWindow sampleWindow;

        private void Awake()
        {
            CreateAndSubscribeToSampleWindow();
            SubscribeToFadeAnimationEvents();
        }

        private void CreateAndSubscribeToSampleWindow()
        {
            sampleWindow =
                UIElementsCreator.GetInstance().Create<SampleWindow>();
            sampleWindow.PointerClicked += OnPointerClicked;
        }

        private void Start()
        {
            ShowSampleWindow();
        }

        private void OnDestroy()
        {
            UnsubscribeFromSampleWindow();
            UnsubscribeFromFadeAnimationEvents();
        }

        private void OnPointerClicked(PointerEventData eventData)
        {
            HideSampleWindow();
        }

        private void ShowSampleWindow()
        {
            if (sampleWindow != null)
            {
                sampleWindow.Show();
            }
        }

        private void HideSampleWindow()
        {
            if (sampleWindow != null)
            {
                sampleWindow.Hide();
            }
        }

        private void SubscribeToFadeAnimationEvents()
        {
            if (sampleWindow != null)
            {
                var uiFadeAnimation =
                    sampleWindow.GetComponent<UIFadeAnimation>();
                uiFadeAnimation.FadeInCompleted += OnFadeInCompleted;
                uiFadeAnimation.FadeOutCompleted += OnFadeOutCompleted;
            }
        }

        private void UnsubscribeFromFadeAnimationEvents()
        {
            if (sampleWindow != null)
            {
                var uiFadeAnimation =
                    sampleWindow.GetComponent<UIFadeAnimation>();
                uiFadeAnimation.FadeInCompleted -= OnFadeInCompleted;
                uiFadeAnimation.FadeOutCompleted -= OnFadeOutCompleted;
            }
        }

        private void OnFadeInCompleted()
        {
            var sampleMessage =
                UIElementsCreator.GetInstance().Create<SampleMessage>();
            sampleMessage.Show();
        }

        private void OnFadeOutCompleted()
        {
            UnsubscribeFromFadeAnimationEvents();
            UnsubscribeFromSampleWindow();
        }

        private void UnsubscribeFromSampleWindow()
        {
            if (sampleWindow != null)
            {
                sampleWindow.PointerClicked -= OnPointerClicked;
            }
        }
    }
}
using UI.Manager;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Sample.Scripts
{
    public class SampleWindowController : MonoBehaviour
    {
        private SampleWindow sampleWindow;
        private SampleImage sampleImage;

        private void Awake()
        {
            sampleWindow =
                UIElementsCreator.GetInstance().Create<SampleWindow>();
            sampleImage = 
                UIElementsCreator.GetInstance()
                    .Create<SampleImage>(UILayer.Background);

            SubscribeToSampleImageEvents();
            SubscribeToFadeAnimationEvents();
        }

        private void Start()
        {
            sampleWindow.Show();
        }

        private void SubscribeToSampleImageEvents()
        {
            sampleImage.PointerClicked += OnPointerClicked;
        }

        private void UnsubscribeToSampleImageEvents()
        {
            sampleImage.PointerClicked -= OnPointerClicked;
        }

        private void OnPointerClicked(PointerEventData eventData)
        {
            sampleWindow.Hide();
        }

        private void SubscribeToFadeAnimationEvents()
        {
            var uiFadeAnimation = sampleWindow.GetComponent<UIFadeAnimation>();
            uiFadeAnimation.FadeInCompleted += OnFadeInCompleted;
            uiFadeAnimation.FadeOutCompleted += OnFadeOutCompleted;
        }

        private void UnsubscribeFromFadeAnimationEvents()
        {
            var uiFadeAnimation = sampleWindow.GetComponent<UIFadeAnimation>();
            uiFadeAnimation.FadeInCompleted -= OnFadeInCompleted;
            uiFadeAnimation.FadeOutCompleted -= OnFadeOutCompleted;
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
            UnsubscribeToSampleImageEvents();

            Destroy(sampleWindow.gameObject);
            Destroy(gameObject);
        }
    }
}
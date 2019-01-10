using UI.Manager;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Sample.Scripts
{
    public class SampleWindowController : MonoBehaviour
    {
        private SampleWindow sampleWindow;
        private BackgroundImage backgroundImage;

        private void Awake()
        {
            sampleWindow =
                UIElementsCreator.GetInstance().Create<SampleWindow>();
            backgroundImage = 
                UIElementsCreator.GetInstance()
                    .Create<BackgroundImage>(UILayer.Background);

            SubscribeToBackgroundImageEvents();
            SubscribeToFadeAnimationEvents();
        }

        private void Start()
        {
            sampleWindow.Show();
        }

        private void SubscribeToBackgroundImageEvents()
        {
            backgroundImage.PointerClicked += OnPointerClicked;
        }

        private void UnsubscribeToBackgroundImageEvents()
        {
            backgroundImage.PointerClicked -= OnPointerClicked;
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
            UnsubscribeToBackgroundImageEvents();

            Destroy(sampleWindow.gameObject);
            Destroy(gameObject);
        }
    }
}
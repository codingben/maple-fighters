using UnityEngine;
using UnityEngine.EventSystems;
using UserInterface;

namespace Sample.Scripts
{
    public class SampleWindowController : MonoBehaviour
    {
        private SampleWindow sampleWindow;
        private BackgroundImage backgroundImage;

        private void Awake()
        {
            sampleWindow =
                UiElementsCreator.GetInstance().Create<SampleWindow>();
            backgroundImage = 
                UiElementsCreator.GetInstance()
                    .Create<BackgroundImage>(UiLayer.Background);

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
            var uiFadeAnimation = sampleWindow.GetComponent<UiFadeAnimation>();
            uiFadeAnimation.FadeInCompleted += OnFadeInCompleted;
            uiFadeAnimation.FadeOutCompleted += OnFadeOutCompleted;
        }

        private void UnsubscribeFromFadeAnimationEvents()
        {
            var uiFadeAnimation = sampleWindow.GetComponent<UiFadeAnimation>();
            uiFadeAnimation.FadeInCompleted -= OnFadeInCompleted;
            uiFadeAnimation.FadeOutCompleted -= OnFadeOutCompleted;
        }

        private void OnFadeInCompleted()
        {
            var sampleMessage =
                UiElementsCreator.GetInstance().Create<SampleMessage>();
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
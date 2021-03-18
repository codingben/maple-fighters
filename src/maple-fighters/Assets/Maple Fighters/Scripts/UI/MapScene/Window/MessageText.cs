using System;
using System.Collections;
using UI;
using UnityEngine;
using UnityEngine.UI;

namespace Scripts.UI.MapScene
{
    [RequireComponent(typeof(Text), typeof(UIFadeAnimation))]
    public class MessageText : UIElement, IMessageView
    {
        public Action TimeUp { get; set; }

        public string Text
        {
            set
            {
                var textMeshPro = GetComponent<Text>();
                textMeshPro.text = value;
            }
        }

        public float Seconds
        {
            set => seconds = value;
        }

        private float seconds;
        private UIFadeAnimation uiFadeAnimation;

        private void Awake()
        {
            uiFadeAnimation = GetComponent<UIFadeAnimation>();
            uiFadeAnimation.FadeInCompleted += OnFadeInCompleted;
            uiFadeAnimation.FadeOutCompleted += OnFadeOutCompleted;
        }

        private void Start()
        {
            Show();
        }

        private void OnDestroy()
        {
            uiFadeAnimation.FadeInCompleted -= OnFadeInCompleted;
            uiFadeAnimation.FadeOutCompleted -= OnFadeOutCompleted;
        }

        private void OnFadeInCompleted()
        {
            StartCoroutine(HideAfterSomeTime());
        }

        private void OnFadeOutCompleted()
        {
            Destroy(gameObject);
        }

        private IEnumerator HideAfterSomeTime()
        {
            yield return new WaitForSeconds(seconds);

            Hide();

            TimeUp?.Invoke();
        }
    }
}
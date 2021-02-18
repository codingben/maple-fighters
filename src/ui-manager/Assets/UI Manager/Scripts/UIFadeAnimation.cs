using System;
using System.Collections;
using UnityEngine;

namespace UI.Manager
{
    public class UIFadeAnimation : UICanvasGroup
    {
        /// <summary>
        /// The event invoked when the animation completed.
        /// </summary>
        public event Action FadeInCompleted;

        /// <summary>
        /// The event invoked when the animation completed.
        /// </summary>
        public event Action FadeOutCompleted;

        [SerializeField]
        private float speed = 1;

        private Coroutine fadeInCoroutine;
        private Coroutine fadeOutCoroutine;

        protected override void OnShown()
        {
            StopFadeOutIfNecessary();

            if (fadeInCoroutine == null)
            {
                fadeInCoroutine = StartCoroutine(FadeIn());
            }
        }

        protected override void OnHidden()
        {
            StopFadeInIfNecessary();

            if (fadeOutCoroutine == null)
            {
                fadeOutCoroutine = StartCoroutine(FadeOut());
            }
        }

        private void StopFadeInIfNecessary()
        {
            if (fadeInCoroutine != null)
            {
                StopCoroutine(fadeInCoroutine);

                fadeInCoroutine = null;
            }
        }

        private void StopFadeOutIfNecessary()
        {
            if (fadeOutCoroutine != null)
            {
                StopCoroutine(fadeOutCoroutine);

                fadeOutCoroutine = null;
            }
        }

        private IEnumerator FadeIn()
        {
            OnBeforeFadeIn();

            while (Alpha < 0.99f)
            {
                Alpha += 1 * speed * Time.deltaTime;
                yield return null;
            }

            OnAfterFadeIn();
        }

        private void OnBeforeFadeIn()
        {
            DisableCanvasGroup();
        }

        private void OnAfterFadeIn()
        {
            EnableCanvasGroup();

            FadeInCompleted?.Invoke();

            fadeInCoroutine = null;
        }

        private IEnumerator FadeOut()
        {
            OnBeforeFadeOut();

            while (Alpha > 0.01f)
            {
                Alpha -= 1 * speed * Time.deltaTime;
                yield return null;
            }

            OnAfterFadeOut();
        }

        private void OnBeforeFadeOut()
        {
            EnableCanvasGroup();
        }

        private void OnAfterFadeOut()
        {
            DisableCanvasGroup();

            FadeOutCompleted?.Invoke();

            fadeOutCoroutine = null;
        }
    }
}
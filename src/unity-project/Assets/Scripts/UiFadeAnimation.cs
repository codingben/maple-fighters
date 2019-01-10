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
        private Coroutine fadeCoroutine;

        protected override void OnShown()
        {
            if (fadeCoroutine != null)
            {
                StopCoroutine(fadeCoroutine);
            }

            fadeCoroutine = StartCoroutine(FadeIn());
        }

        protected override void OnHidden()
        {
            if (fadeCoroutine != null)
            {
                StopCoroutine(fadeCoroutine);
            }

            fadeCoroutine = StartCoroutine(FadeOut());
        }

        private IEnumerator FadeIn()
        {
            DisableCanvasGroup();

            while (Alpha < 0.99f)
            {
                Alpha += 1 * speed * Time.deltaTime;
                yield return null;
            }

            EnableCanvasGroup();

            FadeInCompleted?.Invoke();
        }

        private IEnumerator FadeOut()
        {
            EnableCanvasGroup();

            while (Alpha > 0.01f)
            {
                Alpha -= 1 * speed * Time.deltaTime;
                yield return null;
            }

            DisableCanvasGroup();

            FadeOutCompleted?.Invoke();
        }
    }
}
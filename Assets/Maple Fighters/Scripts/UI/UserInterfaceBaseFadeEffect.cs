using System;
using System.Collections;
using UnityEngine;

namespace Scripts.UI
{
    public class UserInterfaceBaseFadeEffect : UserInterfaceBase
    {
        [Header("Fade Speed")]
        [SerializeField] protected float showSpeed;
        [SerializeField] protected float hideSpeed;

        private Coroutine fadeCoroutine;

        public void Show(Action onFinished)
        {
            Fade(onFinished);
        }

        public void Hide(Action onFinished)
        {
            UnFade(onFinished);
        }

        public override void Show()
        {
            Fade();
        }

        public override void Hide()
        {
            UnFade();
        }

        private void Fade(Action onFinished = null)
        {
            if (fadeCoroutine != null)
            {
                StopCoroutine(fadeCoroutine);
            }

            fadeCoroutine = StartCoroutine(FadeRoutine(showSpeed, onFinished));
        }

        private void UnFade(Action onFinished = null)
        {
            if (fadeCoroutine != null)
            {
                StopCoroutine(fadeCoroutine);
            }

            fadeCoroutine = StartCoroutine(UnFadeRoutine(hideSpeed, onFinished));
        }

        private IEnumerator FadeRoutine(float speed, Action onFinished = null)
        {
            while (true)
            {
                if (CanvasGroup.alpha >= 1)
                {
                    FadeFinished(onFinished);
                    yield break;
                }

                CanvasGroup.alpha += 1 * speed * Time.deltaTime;
                yield return null;
            }
        }

        private IEnumerator UnFadeRoutine(float speed, Action onFinished = null)
        {
            while (true)
            {
                if (CanvasGroup.alpha <= 0)
                {
                    UnFadeFinished(onFinished);
                    yield break;
                }

                CanvasGroup.alpha -= 1 * speed * Time.deltaTime;
                yield return null;
            }
        }

        private void FadeFinished(Action onFinished = null)
        {
            CanvasGroup.alpha = 1;
            CanvasGroup.blocksRaycasts = true;
            CanvasGroup.interactable = true;

            onFinished?.Invoke();
        }

        private void UnFadeFinished(Action onFinished = null)
        {
            CanvasGroup.alpha = 0;
            CanvasGroup.blocksRaycasts = false;
            CanvasGroup.interactable = false;

            onFinished?.Invoke();
        }
    }
}
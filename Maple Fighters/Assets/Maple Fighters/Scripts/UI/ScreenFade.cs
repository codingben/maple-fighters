using System;
using System.Collections.Generic;
using CommonTools.Coroutines;
using Scripts.Coroutines;
using UnityEngine;

namespace Scripts.UI
{
    public class ScreenFade : MonoSingleton<ScreenFade>
    {
        private ExternalCoroutinesExecutor coroutinesExecutor;
        private ICoroutine fadeCoroutine;

        private void Awake()
        {
            coroutinesExecutor = new ExternalCoroutinesExecutor().ExecuteExternally();
        }

        private void Start()
        {
            UnFade(1);
        }

        public void Fade(float time, float speed = 10, Action onFinished = null)
        {
            fadeCoroutine?.Dispose();
            fadeCoroutine = coroutinesExecutor.StartCoroutine(FadeRoutine(time, speed, onFinished));
        }

        public void UnFade(float time, float speed = 10, Action onFinished = null)
        {
            fadeCoroutine?.Dispose();
            fadeCoroutine = coroutinesExecutor.StartCoroutine(UnFadeRoutine(time, speed, onFinished));
        }

        private IEnumerator<IYieldInstruction> FadeRoutine(float time, float speed = 10, Action onFinished = null)
        {
            var curTime = Time.time;
            var canvasGroup = GetComponent<CanvasGroup>();

            do
            {
                if (canvasGroup.alpha >= 1)
                {
                    canvasGroup.alpha = 1;

                    onFinished?.Invoke();
                    yield break;
                }

                canvasGroup.alpha += 0.1f * speed * Time.deltaTime;
                yield return null;
            } while (Time.time < curTime + time);

            onFinished?.Invoke();
        }

        private IEnumerator<IYieldInstruction> UnFadeRoutine(float time, float speed = 10, Action onFinished = null)
        {
            var curTime = Time.time;
            var canvasGroup = GetComponent<CanvasGroup>();

            do
            {
                if (canvasGroup.alpha <= 0)
                {
                    canvasGroup.alpha = 0;

                    onFinished?.Invoke();
                    yield break;
                }

                canvasGroup.alpha -= 0.1f * speed * Time.deltaTime;
                yield return null;
            } while (Time.time < curTime + time);

            onFinished?.Invoke();
        }
    }
}
using System;
using System.Collections;
using UnityEngine;

namespace Scripts.World.Effects
{
    [RequireComponent(typeof(SpriteRenderer))]
    public class FadeEffect : MonoBehaviour
    {
        private enum FadeState
        {
            /// <summary>
            /// The fade.
            /// </summary>
            Fade,

            /// <summary>
            /// The unfade.
            /// </summary>
            UnFade
        }

        [Header("Fade Settings")]
        [SerializeField]
        private FadeState startingState;

        [Header("Fade Speed")]
        [SerializeField]
        private float showSpeed;

        [SerializeField]
        private float hideSpeed;

        [Header("Fade Color")]
        [SerializeField]
        private Color fadeColor;

        [SerializeField]
        private Color unFadeColor;

        private Coroutine fadeCoroutine;
        private SpriteRenderer spriteRenderer;

        private void Awake()
        {
            spriteRenderer = GetComponent<SpriteRenderer>();
        }

        private void OnEnable()
        {
            switch (startingState)
            {
                case FadeState.Fade:
                {
                    Fade();
                    break;
                }

                case FadeState.UnFade:
                {
                    UnFade();
                    break;
                }
            }
        }

        public void Fade(Action onFinished = null)
        {
            spriteRenderer.color = unFadeColor;

            if (fadeCoroutine != null)
            {
                StopCoroutine(fadeCoroutine);
            }

            fadeCoroutine = 
                StartCoroutine(FadeRoutine(showSpeed, onFinished));
        }

        public void UnFade(Action onFinished = null)
        {
            spriteRenderer.color = fadeColor;

            if (fadeCoroutine != null)
            {
                StopCoroutine(fadeCoroutine);
            }

            fadeCoroutine =
                StartCoroutine(UnFadeRoutine(hideSpeed, onFinished));
        }

        private IEnumerator FadeRoutine(float speed, Action onFinished)
        {
            var color = new Color(0, 0, 0, 0.25f);

            while (true)
            {
                if (spriteRenderer.color.a >= fadeColor.a)
                {
                    onFinished?.Invoke();
                    yield break;
                }

                spriteRenderer.color += color * speed * Time.deltaTime;
                yield return null;
            }
        }

        private IEnumerator UnFadeRoutine(float speed, Action onFinished)
        {
            var color = new Color(0, 0, 0, 1);

            while (true)
            {
                if (spriteRenderer.color.a <= 0)
                {
                    onFinished?.Invoke();
                    yield break;
                }

                spriteRenderer.color -= color * speed * Time.deltaTime;
                yield return null;
            }
        }
    }
}
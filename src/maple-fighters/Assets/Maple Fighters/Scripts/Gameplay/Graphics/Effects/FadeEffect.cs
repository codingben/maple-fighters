﻿using System;
using System.Collections;
using UnityEngine;

namespace Scripts.Gameplay.Graphics
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
            /// The un-fade.
            /// </summary>
            UnFade
        }

        public event Action FadeEffectStarted;

        public event Action UnFadeEffectStarted;

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
        private GameObject owner;

        private void Awake()
        {
            spriteRenderer = GetComponent<SpriteRenderer>();
        }

        private void OnEnable()
        {
            if (startingState == FadeState.Fade)
            {
                Fade();
            }
            else
            {
                UnFade();
            }
        }

        public void SetOwner(GameObject owner)
        {
            this.owner = owner;
        }

        public void UnFadeAndDestroyGameObject()
        {
            if (owner != null)
            {
                UnFade(() => Destroy(owner));
            }
            else
            {
                Debug.LogError("Fade effect owner is null.");
            }
        }

        public void Fade(Action onFinished = null)
        {
            if (fadeCoroutine != null)
            {
                StopCoroutine(fadeCoroutine);
            }

            fadeCoroutine =
                StartCoroutine(FadeRoutine(showSpeed, onFinished));
        }

        public void UnFade(Action onFinished = null)
        {
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

            spriteRenderer.color = unFadeColor;

            FadeEffectStarted?.Invoke();

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

            spriteRenderer.color = fadeColor;

            UnFadeEffectStarted?.Invoke();

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
using System.Collections;
using UnityEngine;

namespace Scripts.World
{
    public class FadeEffect : MonoBehaviour
    {
        protected enum FadeState
        {
            Fade,
            UnFade
        }

        [Header("Fade Settings")]
        [SerializeField] protected FadeState startingState;
        [Header("Fade Speed")]
        [SerializeField] protected float showSpeed;
        [SerializeField] protected float hideSpeed;

        private Coroutine fadeCoroutine;
        private SpriteRenderer spriteRenderer;

        private void Awake()
        {
            spriteRenderer = GetComponent<SpriteRenderer>();
        }

        private void Start()
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

        private void Fade()
        {
            if (fadeCoroutine != null)
            {
                StopCoroutine(fadeCoroutine);
            }

            fadeCoroutine = StartCoroutine(FadeRoutine(showSpeed));
        }

        private void UnFade()
        {
            if (fadeCoroutine != null)
            {
                StopCoroutine(fadeCoroutine);
            }

            fadeCoroutine = StartCoroutine(UnFadeRoutine(hideSpeed));
        }

        private IEnumerator FadeRoutine(float speed)
        {
            var color = new Color(0, 0, 0, 0.25f);

            while (true)
            {
                if (spriteRenderer.color.a >= 1)
                {
                    yield break;
                }

                spriteRenderer.color += color * speed * Time.deltaTime;
                yield return null;
            }
        }

        private IEnumerator UnFadeRoutine(float speed)
        {
            var color = new Color(0, 0, 0, 1);

            while (true)
            {
                if (spriteRenderer.color.a <= 0)
                {
                    yield break;
                }

                spriteRenderer.color -= color * speed * Time.deltaTime;
                yield return null;
            }
        }
    }
}
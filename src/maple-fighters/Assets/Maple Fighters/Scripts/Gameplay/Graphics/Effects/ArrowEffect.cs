using System.Collections;
using Scripts.Constants;
using Scripts.Gameplay.Player;
using UnityEngine;

namespace Scripts.Gameplay.Graphics
{
    [RequireComponent(typeof(SpriteRenderer))]
    public class ArrowEffect : MonoBehaviour
    {
        private const float WAIT_TIME_BEFORE_EFFECT = 0.3f;

        [SerializeField]
        private float moveSpeed;

        [SerializeField]
        private float colorSpeed;

        private SpriteRenderer spriteRenderer;
        private PlayerAttackMessageSender attackMessageSender;

        private void Awake()
        {
            spriteRenderer = GetComponent<SpriteRenderer>();
            attackMessageSender = GetComponent<PlayerAttackMessageSender>();
        }

        private void Start()
        {
            StartCoroutine(Move());
            StartCoroutine(ColorEffect());
        }

        private void OnDestroy()
        {
            StopAllCoroutines();
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            var target = other.gameObject;
            if (target.CompareTag(GameTags.MobTag))
            {
                attackMessageSender?.Attack(target);

                StopAllCoroutines();
                Destroy(gameObject);
            }
        }

        private IEnumerator Move()
        {
            yield return new WaitForSeconds(WAIT_TIME_BEFORE_EFFECT);

            while (true)
            {
                var direction = new Vector3(transform.localScale.x, 0, 0);
                transform.position += direction * moveSpeed * Time.deltaTime;
                yield return null;
            }
        }

        private IEnumerator ColorEffect()
        {
            yield return new WaitForSeconds(WAIT_TIME_BEFORE_EFFECT);

            var color = new Color(0, 0, 0, 1);

            while (true)
            {
                if (spriteRenderer.color.a <= 0)
                {
                    Destroy(gameObject);
                    yield break;
                }

                spriteRenderer.color -= color * colorSpeed * Time.deltaTime;
                yield return null;
            }
        }
    }
}
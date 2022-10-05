using System.Collections;
using Game.Messages;
using Scripts.Constants;
using Scripts.Gameplay.Entity;
using Scripts.Gameplay.Map.Objects;
using Scripts.Services;
using UnityEngine;

namespace Scripts.Gameplay.Graphics
{
    [RequireComponent(typeof(SpriteRenderer), typeof(Animator))]
    public class FireBallEffect : MonoBehaviour
    {
        private const float WAIT_TIME_BEFORE_EFFECT = 0.5f;
        private const float AUTO_DESTROY_TIME = 3f;

        [SerializeField]
        private int minDamageAmount = 10;

        [SerializeField]
        private int maxDamageAmount = 50;

        [SerializeField]
        private float moveSpeed;

        [SerializeField]
        private float colorSpeed;

        private Animator animator;
        private SpriteRenderer spriteRenderer;

        private void Awake()
        {
            animator = GetComponent<Animator>();
            spriteRenderer = GetComponent<SpriteRenderer>();
        }

        private void Start()
        {
            StartCoroutine(Move());
            StartCoroutine(AutoDestroyTimer());
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
                Attack(target);

                StopAllCoroutines();
                StartCoroutine(AnimateAndDestroy());
            }
        }

        private void Attack(GameObject other)
        {
            var damageAmount =
                Random.Range(minDamageAmount, maxDamageAmount);
            var mobEntity =
                other.transform.parent.GetComponent<IEntity>();
            var mobBehaviour =
                other.transform.parent.GetComponent<MobBehaviour>();

            mobBehaviour?.Attack(damageAmount);

            var id = mobEntity?.Id ?? -1;
            var message = new AttackMobMessage
            {
                MobId = id,
                DamageAmount = damageAmount
            };

            var gameApi = ApiProvider.ProvideGameApi();
            gameApi?.SendMessage(MessageCodes.AttackMob, message);
        }

        private IEnumerator Move()
        {
            spriteRenderer.enabled = false;

            yield return new WaitForSeconds(WAIT_TIME_BEFORE_EFFECT);

            spriteRenderer.enabled = true;

            while (true)
            {
                var direction = new Vector3(transform.localScale.x, 0, 0);
                transform.position += direction * moveSpeed * Time.deltaTime;
                yield return null;
            }
        }

        private IEnumerator AutoDestroyTimer()
        {
            yield return new WaitForSeconds(AUTO_DESTROY_TIME);

            StartCoroutine(AnimateAndDestroy());
        }

        private IEnumerator AnimateAndDestroy()
        {
            animator.SetBool("Hit", true);

            var color = new Color(0, 0, 0, 1);

            spriteRenderer.enabled = true;

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
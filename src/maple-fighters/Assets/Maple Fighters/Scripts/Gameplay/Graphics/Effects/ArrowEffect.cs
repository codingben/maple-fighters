using System.Collections;
using Game.Messages;
using Scripts.Constants;
using Scripts.Gameplay.Entity;
using Scripts.Gameplay.Map.Objects;
using Scripts.Services;
using UnityEngine;

namespace Scripts.Gameplay.Graphics
{
    [RequireComponent(typeof(SpriteRenderer))]
    public class ArrowEffect : MonoBehaviour
    {
        private const float WAIT_TIME_BEFORE_EFFECT = 0.3f;

        [SerializeField]
        private int minDamageAmount = 10;

        [SerializeField]
        private int maxDamageAmount = 50;

        [SerializeField]
        private float moveSpeed;

        [SerializeField]
        private float colorSpeed;

        private SpriteRenderer spriteRenderer;

        private void Awake()
        {
            spriteRenderer = GetComponent<SpriteRenderer>();
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
                Attack(target);
                Destroy(gameObject);
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
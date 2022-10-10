using Scripts.Constants;
using Scripts.Gameplay.Player;
using UnityEngine;

namespace Scripts.Gameplay.Graphics
{
    [RequireComponent(typeof(SpriteRenderer))]
    public class ArrowEffect : MonoBehaviour
    {
        [SerializeField]
        private float moveSpeed;

        [SerializeField]
        private float colorSpeed;

        private SpriteRenderer spriteRenderer;
        private PlayerAttackMessageSender attackMessageSender;

        private float previousTime;

        private void Awake()
        {
            spriteRenderer = GetComponent<SpriteRenderer>();
            attackMessageSender = GetComponent<PlayerAttackMessageSender>();
        }

        private void Start()
        {
            previousTime = Time.time;
        }

        private void Update()
        {
            if (CanMove())
            {
                Move();
                DeacraseColor();
            }
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            var target = other.gameObject;
            if (target.CompareTag(GameTags.MobTag))
            {
                attackMessageSender?.Attack(target);

                Destroy(gameObject);
            }
        }

        private void Move()
        {
            var direction = new Vector3(transform.localScale.x, 0, 0);
            transform.position += direction * moveSpeed * Time.deltaTime;
        }

        private void DeacraseColor()
        {
            if (spriteRenderer.color.a <= 0)
            {
                Destroy(gameObject);
            }

            var color = new Color(0, 0, 0, 1);
            spriteRenderer.color -= color * colorSpeed * Time.deltaTime;
        }

        private bool CanMove()
        {
            return Time.time >= previousTime + 0.3f;
        }
    }
}
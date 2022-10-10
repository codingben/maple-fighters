using Scripts.Constants;
using Scripts.Gameplay.Player;
using UnityEngine;

namespace Scripts.Gameplay.Graphics
{
    [RequireComponent(typeof(SpriteRenderer), typeof(Animator))]
    public class FireBallEffect : MonoBehaviour
    {
        [SerializeField]
        private float moveSpeed;

        [SerializeField]
        private float colorSpeed;

        private Animator animator;
        private SpriteRenderer spriteRenderer;
        private PlayerAttackMessageSender attackMessageSender;

        private float previousTime;
        private bool triggerEntered;

        private void Awake()
        {
            animator = GetComponent<Animator>();
            spriteRenderer = GetComponent<SpriteRenderer>();
            attackMessageSender = GetComponent<PlayerAttackMessageSender>();
        }

        private void Start()
        {
            previousTime = Time.time;

            DisableSpriteRenderer();
        }

        private void Update()
        {
            if (CanMove())
            {
                EnableSpriteRenderer();
                Move();
            }

            if (CanAnimateAndDestroy())
            {
                EnableHitAnimation();
                EnableSpriteRenderer();
                AnimateAndDestroy();
            }
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            var target = other.gameObject;
            if (target.CompareTag(GameTags.MobTag))
            {
                triggerEntered = true;

                attackMessageSender?.Attack(target);
            }
        }

        private void Move()
        {
            var direction = new Vector3(transform.localScale.x, 0, 0);
            transform.position += direction * moveSpeed * Time.deltaTime;
        }

        private void AnimateAndDestroy()
        {
            if (spriteRenderer.color.a <= 0)
            {
                Destroy(gameObject);
            }

            var color = new Color(0, 0, 0, 1);
            spriteRenderer.color -= color * colorSpeed * Time.deltaTime;
        }

        private void EnableHitAnimation()
        {
            animator.SetBool("Hit", true);
        }

        private void EnableSpriteRenderer()
        {
            spriteRenderer.enabled = true;
        }

        private void DisableSpriteRenderer()
        {
            spriteRenderer.enabled = false;
        }

        private bool CanMove()
        {
            if (triggerEntered)
            {
                return false;
            }

            return Time.time >= previousTime + 0.5f;
        }

        private bool CanAnimateAndDestroy()
        {
            if (triggerEntered)
            {
                return true;
            }

            return Time.time >= previousTime + 3.0f;
        }
    }
}
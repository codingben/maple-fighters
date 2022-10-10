using Scripts.Gameplay.Player;
using UnityEngine;

namespace Scripts.Gameplay.Graphics
{
    [RequireComponent(typeof(SpriteRenderer))]
    public class SwordEffect : MonoBehaviour
    {
        [SerializeField]
        private float colorSpeed;

        private SpriteRenderer spriteRenderer;
        private PlayerAttackMessageSender attackMessageSender;

        private bool attacked;
        private float previousTime;

        private void Awake()
        {
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
            if (CanAnimateAndDestroy())
            {
                Attack();
                EnableSpriteRenderer();
                DeacraseColor();
            }
        }

        private void Attack()
        {
            if (attacked == false)
            {
                var direction = GetDirection();
                attackMessageSender?.Attack(direction);
            }

            attacked = true;
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

        private void EnableSpriteRenderer()
        {
            spriteRenderer.enabled = true;
        }

        private void DisableSpriteRenderer()
        {
            spriteRenderer.enabled = false;
        }

        private Vector2 GetDirection()
        {
            return new Vector2(-transform.localScale.x, 0);
        }

        private bool CanAnimateAndDestroy()
        {
            return Time.time > previousTime + 0.6f;
        }
    }
}
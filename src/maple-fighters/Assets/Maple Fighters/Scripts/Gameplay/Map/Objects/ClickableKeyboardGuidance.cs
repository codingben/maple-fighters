using UnityEngine;

namespace Scripts.Gameplay.Map.Objects
{
    public class ClickableKeyboardGuidance : MonoBehaviour
    {
        [SerializeField]
        private KeyCode primaryKeyCode;

        [SerializeField]
        private KeyCode secondaryKeyCode;

        [SerializeField]
        private float colorSpeed;

        private bool clicked;

        private new Animation animation;
        private SpriteRenderer spriteRenderer;

        private void Awake()
        {
            animation = GetComponent<Animation>();
            spriteRenderer = GetComponent<SpriteRenderer>();
        }

        private void Update()
        {
            if (clicked)
            {
                DeacraseColor();
            }
            else
            {
                clicked = Input.GetKeyDown(primaryKeyCode) || Input.GetKeyDown(secondaryKeyCode);
            }
        }

        private void DeacraseColor()
        {
            animation.Stop();

            if (spriteRenderer.color.a <= 0)
            {
                Destroy(gameObject);
            }

            var color = new Color(0, 0, 0, 1);
            spriteRenderer.color -= color * colorSpeed * Time.deltaTime;
        }
    }
}
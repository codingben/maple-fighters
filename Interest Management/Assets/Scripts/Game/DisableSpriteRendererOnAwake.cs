using UnityEngine;

namespace Assets.Scripts.Game
{
    [RequireComponent(typeof(SpriteRenderer))]
    public class DisableSpriteRendererOnAwake : MonoBehaviour
    {
        private void Awake()
        {
            var spriteRenderer = GetComponent<SpriteRenderer>();
            spriteRenderer.enabled = false;
        }
    }
}
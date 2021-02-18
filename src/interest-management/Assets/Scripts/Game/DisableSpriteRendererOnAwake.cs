using UnityEngine;

namespace Game.InterestManagement.Simulation
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
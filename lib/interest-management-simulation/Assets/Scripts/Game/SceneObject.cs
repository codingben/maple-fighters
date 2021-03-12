using InterestManagement;
using UnityEngine;

namespace Game.InterestManagement.Simulation
{
    [ExecuteInEditMode]
    public class SceneObject : MonoBehaviour, IGameObject
    {
        public int Id { get; private set; }

        public ITransform Transform { get; private set; }

        [Header("Interest Area Size")]
        [SerializeField]
        private Vector2 size;

        [Header("Graphics")]
        [SerializeField]
        private SpriteRenderer spriteRenderer;

        private void Awake()
        {
            Id = gameObject.GetInstanceID();
            Transform = new GameTransform(transform.position.ToVector2(), size.ToVector2());
        }

        public void SetGraphics(bool active)
        {
            if (spriteRenderer != null)
            {
                spriteRenderer.enabled = active;
            }
        }
    }
}
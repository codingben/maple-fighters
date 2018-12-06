using Game.InterestManagement;
using UnityEngine;

namespace Assets.Scripts.Game
{
    [ExecuteInEditMode]
    public class SceneObject : MonoBehaviour, IGameObject
    {
        public int Id { get; private set; }

        public ITransform Transform { get; private set; }

        [Header("These values will affect the interest area")]
        [SerializeField]
        private Vector2 position;

        [SerializeField]
        private Vector2 size;

        [Header("Graphics")]
        [SerializeField]
        private SpriteRenderer spriteRenderer;

        private void Awake()
        {
            // The purpose to execute this in edit mode.
            position = transform.position;

            Id = gameObject.GetInstanceID();
            Transform = 
                new GameTransform(position.ToVector2(), size.ToVector2());
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
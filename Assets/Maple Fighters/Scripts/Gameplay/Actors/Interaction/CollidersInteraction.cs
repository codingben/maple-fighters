using UnityEngine;

namespace Scripts.Gameplay.Actors.Interaction
{
    public class CollidersInteraction
    {
        private readonly Collider2D baseCollider;

        private Collider2D overlappingCollider;
        private Collider2D ignoredCollider;

        public CollidersInteraction(Collider2D collider)
        {
            baseCollider = collider;
        }

        public void SetOverlappingCollider(Collider2D collider)
        {
            overlappingCollider = collider;
        }

        public void SetIgnoredCollider(Collider2D collider)
        {
            ignoredCollider = collider;
        }

        public void EnableCollisionWithIgnoredCollider()
        {
            if (ignoredCollider != null)
            {
                Physics2D.IgnoreCollision(baseCollider, ignoredCollider, false);
            }
        }

        public void DisableCollisionWithIgnoredCollider()
        {
            if (ignoredCollider != null)
            {
                Physics2D.IgnoreCollision(baseCollider, ignoredCollider, true);
            }
        }

        public bool HasOverlappingCollider()
        {
            return overlappingCollider != null;
        }

        public bool HasOverlappingColliderPosition(out Vector2 position)
        {
            var isOverlappingWithCollider = overlappingCollider != null;
            if (isOverlappingWithCollider)
            {
                position = overlappingCollider.bounds.center;
            }
            else
            {
                position = Vector2.zero;
            }

            return isOverlappingWithCollider;
        }

        public Rigidbody2D GetAttachedRigidbody()
        {
            return baseCollider.attachedRigidbody;
        }
    }
}
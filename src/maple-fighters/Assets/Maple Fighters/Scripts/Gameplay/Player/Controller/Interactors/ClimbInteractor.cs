using Game.Common;
using UnityEngine;

namespace Scripts.Gameplay.Player
{
    public abstract class ClimbInteractor : MonoBehaviour
    {
        private ColliderInteraction colliderInteraction;

        private void Awake()
        {
            colliderInteraction = GetColliderInteraction();
        }

        private void Update()
        {
            if (Input.GetKeyDown(GetInteractionKey()) && IsPlayerStateSuitable() && !IsClimbing())
            {
                if (colliderInteraction.HasOverlappingCollider())
                {
                    StartClimbing();
                }
            }
        }

        private void OnTriggerEnter2D(Collider2D collider)
        {
            if (collider.transform.CompareTag(GetTagName()))
            {
                colliderInteraction.SetOverlappingCollider(collider);
            }
        }

        private void OnTriggerExit2D(Collider2D collider)
        {
            if (collider.transform.CompareTag(GetTagName()))
            {
                if (IsClimbing())
                {
                    StopClimbing();
                }

                colliderInteraction.SetOverlappingCollider(null);
            }
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (collision.transform.CompareTag(GetTagName()))
            {
                if (IsClimbing())
                {
                    colliderInteraction.SetIgnoredCollider(collision.collider);
                    colliderInteraction.DisableCollisionWithIgnoredCollider();
                }
            }
        }

        private void StartClimbing()
        {
            var ground = Utils.GetGroundedCollider(transform.parent.position);
            if (ground != null)
            {
                colliderInteraction.SetIgnoredCollider(ground);
                colliderInteraction.DisableCollisionWithIgnoredCollider();
            }

            ChangePositionToCenter();
            ChangePlayerStateToClimbState();
        }

        private void ChangePlayerStateToClimbState()
        {
            SetPlayerToClimbState();
        }

        private void StopClimbing()
        {
            colliderInteraction.EnableCollisionWithIgnoredCollider();
            colliderInteraction.SetIgnoredCollider(null);

            UnsetPlayerFromClimbState();
        }

        private void ChangePositionToCenter()
        {
            var rigidbody = colliderInteraction.GetAttachedRigidbody();
            rigidbody.velocity = Vector2.zero;

            if (colliderInteraction.HasOverlappingColliderPosition(out var center))
            {
                transform.parent.position = 
                    new Vector3(center.x, transform.parent.position.y);
            }
        }

        private bool IsPlayerStateSuitable()
        {
            return GetPlayerState() == PlayerState.Idle ||
                   GetPlayerState() == PlayerState.Jumping || 
                   GetPlayerState() == PlayerState.Falling;
        }

        private bool IsClimbing()
        {
            return GetPlayerState() == GetClimbState();
        }

        protected abstract void SetPlayerToClimbState();

        protected abstract void UnsetPlayerFromClimbState();

        protected abstract PlayerState GetPlayerState();

        protected abstract KeyCode GetInteractionKey();

        protected abstract ColliderInteraction GetColliderInteraction();

        protected abstract string GetTagName();

        protected abstract PlayerState GetClimbState();
    }
}
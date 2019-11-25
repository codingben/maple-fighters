using Game.Common;
using UnityEngine;

namespace Scripts.Gameplay.Player
{
    public abstract class ClimbInteractor : MonoBehaviour
    {
        private void Update()
        {
            if (Input.GetKeyDown(GetKey()) && IsPlayerStateSuitable() && !IsClimbing())
            {
                if (GetColliderInteraction().HasOverlappingCollider())
                {
                    StartClimbing();
                }
            }
        }

        private void OnTriggerEnter2D(Collider2D collider)
        {
            if (collider.transform.CompareTag(GetTagName()))
            {
                GetColliderInteraction().SetOverlappingCollider(collider);
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

                GetColliderInteraction().SetOverlappingCollider(null);
            }
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (collision.transform.CompareTag(GetTagName()))
            {
                if (IsClimbing())
                {
                    GetColliderInteraction().SetIgnoredCollider(collision.collider);
                    GetColliderInteraction().DisableCollisionWithIgnoredCollider();
                }
            }
        }

        private void StartClimbing()
        {
            var ground = Utils.GetGroundedCollider(transform.parent.position);
            if (ground != null)
            {
                GetColliderInteraction().SetIgnoredCollider(ground);
                GetColliderInteraction().DisableCollisionWithIgnoredCollider();
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
            GetColliderInteraction().EnableCollisionWithIgnoredCollider();
            GetColliderInteraction().SetIgnoredCollider(null);

            UnsetPlayerFromClimbState();
        }

        private void ChangePositionToCenter()
        {
            var rigidbody = GetColliderInteraction().GetAttachedRigidbody();
            rigidbody.velocity = Vector2.zero;

            if (GetColliderInteraction().HasOverlappingColliderPosition(out var center))
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

        protected abstract KeyCode GetKey();

        protected abstract ColliderInteraction GetColliderInteraction();

        protected abstract string GetTagName();

        protected abstract PlayerState GetClimbState();
    }
}
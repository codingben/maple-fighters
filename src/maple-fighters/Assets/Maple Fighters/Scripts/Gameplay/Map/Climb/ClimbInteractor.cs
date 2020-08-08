using Scripts.Constants;
using Scripts.Gameplay.Player;
using UnityEngine;

namespace Scripts.Gameplay.Map.Climb
{
    public abstract class ClimbInteractor : MonoBehaviour
    {
        private Transform LocalPlayer => transform.parent;

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
            if (collision.transform.CompareTag(GameTags.FloorTag))
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
            IgnoreGroundIfNeeded();

            ChangePositionToCenter();
            ChangePlayerStateToClimbState();
        }

        private void IgnoreGroundIfNeeded()
        {
            var ground = Player.Utils.GetGroundedCollider(LocalPlayer.position);
            if (ground != null)
            {
                GetColliderInteraction().SetIgnoredCollider(ground);
                GetColliderInteraction().DisableCollisionWithIgnoredCollider();
            }
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
            if (GetColliderInteraction().HasOverlappingColliderPosition(out var center))
            {
                ChangeVelocityToZero();

                var x = center.x;
                var y = LocalPlayer.position.y;

                LocalPlayer.position = new Vector3(x, y);
            }
        }

        private void ChangeVelocityToZero()
        {
            var rigidbody =
                GetColliderInteraction().GetAttachedRigidbody();
            if (rigidbody != null)
            {
                rigidbody.velocity = Vector2.zero;
            }
        }

        private bool IsPlayerStateSuitable()
        {
            return GetPlayerState() == PlayerStates.Idle ||
                   GetPlayerState() == PlayerStates.Jumping ||
                   GetPlayerState() == PlayerStates.Falling;
        }

        private bool IsClimbing()
        {
            return GetPlayerState() == GetClimbState();
        }

        protected abstract void SetPlayerToClimbState();

        protected abstract void UnsetPlayerFromClimbState();

        protected abstract PlayerStates GetPlayerState();

        protected abstract KeyCode GetKey();

        protected abstract ColliderInteraction GetColliderInteraction();

        protected abstract string GetTagName();

        protected abstract PlayerStates GetClimbState();
    }
}
using System.Linq;
using Game.Common;
using Scripts.Constants;
using UnityEngine;

namespace Scripts.Gameplay.Player
{
    [RequireComponent(typeof(PlayerController), typeof(Collider2D))]
    public class RopeInteractor : MonoBehaviour
    {
        [SerializeField]
        private KeyCode interactionKey = KeyCode.LeftControl;

        private PlayerController playerController;
        private ColliderInteraction colliderInteraction;

        private void Awake()
        {
            playerController = GetComponent<PlayerController>();

            var collider = GetComponent<Collider2D>();
            colliderInteraction = new ColliderInteraction(collider);
        }

        private void Update()
        {
            if (IsInInteraction())
            {
                return;
            }

            if (Input.GetKeyDown(interactionKey) && IsPlayerStateSuitable())
            {
                if (colliderInteraction.HasOverlappingCollider())
                {
                    StartInteraction();
                }
            }
        }

        private void OnTriggerEnter2D(Collider2D collider)
        {
            if (collider.transform.CompareTag(GameTags.RopeTag))
            {
                colliderInteraction.SetOverlappingCollider(collider);
            }
        }

        private void OnTriggerExit2D(Collider2D collider)
        {
            if (collider.transform.CompareTag(GameTags.RopeTag))
            {
                if (IsInInteraction())
                {
                    colliderInteraction.EnableCollisionWithIgnoredCollider();
                    colliderInteraction.SetIgnoredCollider(null);

                    StopInteraction();
                }

                colliderInteraction.SetOverlappingCollider(null);
            }
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (IsInInteraction()
                && collision.transform.CompareTag(GameTags.FloorTag))
            {
                colliderInteraction.SetIgnoredCollider(collision.collider);
                colliderInteraction.DisableCollisionWithIgnoredCollider();
            }
        }

        private void StartInteraction()
        {
            var ground = GetGroundedCollider();
            if (ground != null)
            {
                colliderInteraction.SetIgnoredCollider(ground);
                colliderInteraction.DisableCollisionWithIgnoredCollider();
            }

            ChangePositionToRopeCenter();
            ChangePlayerStateToRope();
        }

        private void ChangePlayerStateToRope()
        {
            playerController.ChangePlayerState(PlayerState.Rope);
        }

        private void StopInteraction()
        {
            colliderInteraction.EnableCollisionWithIgnoredCollider();
            colliderInteraction.SetIgnoredCollider(null);

            ChangePlayerStateFromRope();
        }

        private void ChangePlayerStateFromRope()
        {
            if (playerController.IsGrounded())
            {
                playerController.ChangePlayerState(PlayerState.Idle);
            }
            else
            {
                playerController.ChangePlayerState(PlayerState.Falling);
            }
        }

        private void ChangePositionToRopeCenter()
        {
            var rigidbody = colliderInteraction.GetAttachedRigidbody();
            rigidbody.velocity = Vector2.zero;

            Vector2 center;
            if (colliderInteraction.HasOverlappingColliderPosition(out center))
            {
                transform.parent.position = 
                    new Vector3(center.x, transform.parent.position.y);
            }
        }

        private bool IsPlayerStateSuitable()
        {
            return playerController.PlayerState == PlayerState.Idle
                             || playerController.PlayerState
                             == PlayerState.Jumping
                             || playerController.PlayerState
                             == PlayerState.Falling;
        }

        private bool IsInInteraction()
        {
            return playerController.PlayerState == PlayerState.Rope;
        }

        private Collider2D GetGroundedCollider()
        {
            var hit = Physics2D.RaycastAll(
                transform.parent.position,
                Vector2.down,
                1,
                1 << LayerMask.NameToLayer(GameLayers.FloorLayer));

            Collider2D collider = null;

            if (hit.Any())
            {
                collider = hit.First().collider;
            }

            return collider;
        }
    }
}
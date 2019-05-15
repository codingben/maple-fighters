using System.Linq;
using Game.Common;
using UnityEngine;

namespace Scripts.Gameplay.Actors.Interaction
{
    [RequireComponent(typeof(PlayerController), typeof(Collider2D))]
    public class LadderInteraction : MonoBehaviour
    {
        private const string LadderTag = "Ladder";
        private const string FloorTag = "Ground";
        private const string FloorLayerName = "Floor";

        [SerializeField]
        private KeyCode interactionKey = KeyCode.LeftControl;

        private PlayerController playerController;
        private CollidersInteraction collidersInteraction;

        private void Awake()
        {
            playerController = GetComponent<PlayerController>();

            var collider = GetComponent<Collider2D>();
            collidersInteraction = new CollidersInteraction(collider);
        }

        private void Update()
        {
            if (IsInInteraction())
            {
                return;
            }

            if (Input.GetKeyDown(interactionKey) && IsPlayerStateSuitable())
            {
                if (collidersInteraction.HasOverlappingCollider())
                {
                    StartInteraction();
                }
            }
        }

        private void OnTriggerEnter2D(Collider2D collider)
        {
            if (collider.transform.CompareTag(LadderTag))
            {
                collidersInteraction.SetOverlappingCollider(collider);
            }
        }

        private void OnTriggerExit2D(Collider2D collider)
        {
            if (collider.transform.CompareTag(LadderTag))
            {
                if (IsInInteraction())
                {
                    collidersInteraction.EnableCollisionWithIgnoredCollider();
                    collidersInteraction.SetIgnoredCollider(null);

                    StopInteraction();
                }

                collidersInteraction.SetOverlappingCollider(null);
            }
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (IsInInteraction() && collision.transform.CompareTag(FloorTag))
            {
                collidersInteraction.SetIgnoredCollider(collision.collider);
                collidersInteraction.DisableCollisionWithIgnoredCollider();
            }
        }

        private void StartInteraction()
        {
            var ground = GetGroundedCollider();
            if (ground != null)
            {
                collidersInteraction.SetIgnoredCollider(ground);
                collidersInteraction.DisableCollisionWithIgnoredCollider();
            }

            ChangePositionToLadderCenter();
            ChangePlayerStateToLadder();
        }

        private void ChangePlayerStateToLadder()
        {
            playerController.ChangePlayerState(PlayerState.Ladder);
        }

        private void StopInteraction()
        {
            collidersInteraction.EnableCollisionWithIgnoredCollider();
            collidersInteraction.SetIgnoredCollider(null);

            ChangePlayerStateFromLadder();
        }

        private void ChangePlayerStateFromLadder()
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

        private void ChangePositionToLadderCenter()
        {
            var rigidbody = collidersInteraction.GetAttachedRigidbody();
            rigidbody.velocity = Vector2.zero;

            Vector2 center;
            if (collidersInteraction.HasOverlappingColliderPosition(out center))
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
            return playerController.PlayerState == PlayerState.Ladder;
        }

        private Collider2D GetGroundedCollider()
        {
            var hit = Physics2D.RaycastAll(
                transform.parent.position,
                Vector2.down,
                1,
                1 << LayerMask.NameToLayer(FloorLayerName));

            Collider2D collider = null;

            if (hit.Any())
            {
                collider = hit.First().collider;
            }

            return collider;
        }
    }
}
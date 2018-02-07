using Scripts.Gameplay.Actors;
using Shared.Game.Common;
using UnityEngine;

#pragma warning disable 0109

namespace Scripts.World
{
    public class ClimbingInteraction : MonoBehaviour
    {
        public bool CanInteract { get; set; }
        public bool IsInInteraction { get; set; }

        private enum Interaction
        {
            Rope,
            Ladder
        }

        private const string ROPE_TAG = "Rope";
        private const string LADDER_TAG = "Ladder";

        [Header("Keyboard")]
        [SerializeField] private KeyCode flyInteractionKey = KeyCode.UpArrow;
        [SerializeField] private KeyCode floorInteractionKey = KeyCode.DownArrow;

        private Vector3 center;
        private new Collider2D collider;
        private Collider2D ignoredCollider;
        private Interaction interactionType;

        private IPlayerController playerController;

        private void Awake()
        {
            collider = GetComponent<Collider2D>();
            playerController = GetComponent<PlayerController>();
        }

        private void Update()
        {
            if (CanInteract)
            {
                IsInInteractionState();
            }
        }

        private void IsInInteractionState()
        {
            if (Input.GetKeyDown(floorInteractionKey)
                && playerController.PlayerState == PlayerState.Idle)
            {
                KeyDownPressed();
            }

            if (Input.GetKeyDown(flyInteractionKey)
                && (playerController.PlayerState == PlayerState.Jumping || playerController.PlayerState == PlayerState.Falling))
            {
                KeyUpPressed();
            }
        }

        private void KeyUpPressed()
        {
            StartedInteractionWithRopeOrLadder();
        }

        private void KeyDownPressed()
        {
            StartedInteractionWithRopeOrLadder();
            DisableGroundInteraction();
        }

        private void StartedInteractionWithRopeOrLadder()
        {
            IsInInteraction = true;
            playerController.PlayerState = interactionType == Interaction.Rope ? PlayerState.Rope : PlayerState.Ladder;

            SetPositionToRopeCenter();
        }

        private void SetPositionToRopeCenter()
        {
            collider.attachedRigidbody.velocity = new Vector2(0, collider.attachedRigidbody.velocity.y);
            transform.parent.position = new Vector3(center.x, transform.parent.position.y);
        }

        private void IgnoreCollision(bool ignore)
        {
            if (IsInInteraction)
            {
                Physics2D.IgnoreCollision(collider, ignoredCollider, ignore);
            }
        }

        private void OnTriggerEnter2D(Collider2D collider)
        {
            if (collider.transform.CompareTag(ROPE_TAG) || collider.transform.CompareTag(LADDER_TAG))
            {
                center = collider.bounds.center;

                var interactionType = collider.transform.CompareTag(ROPE_TAG) ? Interaction.Rope : Interaction.Ladder;
                EnableInteraction(interactionType);
            }
        }

        private void OnTriggerExit2D(Collider2D collider)
        {
            if (collider.transform.CompareTag(ROPE_TAG) || collider.transform.CompareTag(LADDER_TAG))
            {
                DisableInteraction();
            }
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            ignoredCollider = collision.gameObject.GetComponent<Collider2D>();

            DisableGroundInteraction();
        }

        private void EnableInteraction(Interaction type)
        {
            CanInteract = true;
            interactionType = type;
        }

        private void DisableInteraction()
        {
            if (IsInInteraction)
            {
                playerController.PlayerState = PlayerState.Idle;
            }

            EnableGroundInteraction();

            CanInteract = false;
            IsInInteraction = false;
        }

        private void EnableGroundInteraction()
        {
            IgnoreCollision(false);
        }

        private void DisableGroundInteraction()
        {
            IgnoreCollision(true);
        }
    }
}
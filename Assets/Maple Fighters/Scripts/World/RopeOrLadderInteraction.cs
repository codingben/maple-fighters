using Scripts.Gameplay.Actors;
using Shared.Game.Common;
using UnityEngine;

#pragma warning disable 0109

namespace Scripts.World
{
    public class RopeOrLadderInteraction : MonoBehaviour
    {
        public bool IsInInteraction { get; private set; }

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

        private PlayerController playerController;

        private Interaction interactionType;

        private void Awake()
        {
            collider = GetComponent<Collider2D>();
            playerController = GetComponent<PlayerController>();
        }

        private void Update()
        {
            if (IsInInteraction)
            {
                IsInInteractionState();
            }
        }

        private void IsInInteractionState()
        {
            if (IsInInteraction
                && Input.GetKeyDown(floorInteractionKey)
                && playerController.PlayerState == PlayerState.Idle)
            {
                KeyDownPressed();
            }

            if (IsInInteraction
                && Input.GetKeyDown(flyInteractionKey)
                && playerController.PlayerState == PlayerState.Falling)
            {
                KeyUpPressed();
            }

            if (ignoredCollider != null)
            {
                if (!Physics2D.GetIgnoreCollision(ignoredCollider, collider))
                {
                    return;
                }

                if (playerController.PlayerState == PlayerState.Idle)
                {
                    IgnoreCollision(false);
                }
            }
        }

        private void KeyUpPressed()
        {
            StartedInteractionWithRopeOrLadder();
        }

        private void KeyDownPressed()
        {
            DisableGroundInteraction();
            StartedInteractionWithRopeOrLadder();
        }

        private void StartedInteractionWithRopeOrLadder()
        {
            SetPlayerStateToCurrentInteraction();
            SetPositionToRopeCenter();
        }

        private void SetPlayerStateToCurrentInteraction()
        {
            switch (interactionType)
            {
                case Interaction.Rope:
                {
                    playerController.SetStateFromRopeOrLadderInteraction(PlayerState.Rope);
                    break;
                }
                case Interaction.Ladder:
                {
                    playerController.SetStateFromRopeOrLadderInteraction(PlayerState.Ladder);
                    break;
                }
            }
        }

        private void SetPositionToRopeCenter()
        {
            transform.parent.position = new Vector3(center.x, transform.parent.position.y);
        }

        private void IgnoreCollision(bool ignore)
        {
            Physics2D.IgnoreCollision(collider, ignoredCollider, ignore);
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
            if (!IsInInteraction)
            {
                return;
            }

            ignoredCollider = collision.gameObject.GetComponent<Collider2D>();
            IgnoreCollision(true);

            EnableGroundDetection();
        }

        private void EnableInteraction(Interaction type)
        {
            IsInInteraction = true;
            interactionType = type;
        }

        private void DisableInteraction()
        {
            IsInInteraction = false;

            playerController.SetStateFromRopeOrLadderInteraction(PlayerState.Idle);

            EnableGroundDetection();
        }

        private void EnableGroundDetection()
        {
            if (!playerController.DetectGround)
            {
                playerController.DetectGround = true;
            }
        }

        private void DisableGroundInteraction()
        {
            IgnoreCollision(true);

            playerController.DetectGround = false;
        }
    }
}
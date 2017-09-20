using Scripts.Gameplay.Actors;
using Shared.Game.Common;
using UnityEngine;

namespace Scripts.World
{
    [RequireComponent(typeof(PlayerController))]
    public class RopeOrLadderInteraction : MonoBehaviour
    {
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

        private bool isInteracted;

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
            if (isInteracted)
            {
                IsInInteractionState();
            }
        }

        private void IsInInteractionState()
        {
            if (isInteracted
                && Input.GetKeyDown(floorInteractionKey)
                && playerController.PlayerState == PlayerState.Idle)
            {
                KeyDownPressed();
            }

            if (isInteracted
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
            SetPlayerStateToCurrentInteraction();
            SetPositionToRopeCenter();
        }

        private void KeyDownPressed()
        {
            SetPlayerStateToCurrentInteraction();

            IgnoreCollision(true);
            playerController.DetectGround = false;

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
            transform.position = new Vector3(center.x, transform.position.y);
        }

        private void IgnoreCollision(bool ignore)
        {
            Physics2D.IgnoreCollision(collider, ignoredCollider, ignore);
        }

        private void OnTriggerEnter2D(Collider2D collider)
        {
            if (collider.transform.CompareTag(ROPE_TAG))
            {
                center = collider.bounds.center;
                EnableInteraction(Interaction.Rope);
            }

            if (collider.transform.CompareTag(LADDER_TAG))
            {
                center = collider.bounds.center;
                EnableInteraction(Interaction.Ladder);
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
            if (!isInteracted)
            {
                return;
            }

            ignoredCollider = collision.gameObject.GetComponent<Collider2D>();
            IgnoreCollision(true);

            EnableGroundDetection();
        }

        private void EnableInteraction(Interaction type)
        {
            interactionType = type;
            isInteracted = true;
        }

        private void DisableInteraction()
        {
            isInteracted = false;

            playerController.SetStateFromRopeOrLadderInteraction(PlayerState.Idle);

            EnableGroundDetection();
        }

        private void EnableGroundDetection()
        {
            if (playerController.DetectGround)
            {
                return;
            }

            playerController.DetectGround = true;
        }
    }
}
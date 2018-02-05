using Scripts.UI.Controllers;
using Shared.Game.Common;
using UnityEngine;

namespace Scripts.Gameplay.Actors
{
    /*public class PlayerInput : MonoBehaviour
    {
        [Header("Keyboard")]
        [SerializeField] private KeyCode jumpKey;

        private PlayerController playerController;

        private void Awake()
        {
            playerController = GetComponent<PlayerController>();
        }

        private void Update()
        {
            if (FocusController.Instance.Focusable != Focusable.Game)
            {
                return;
            }

            switch (playerController.PlayerState)
            {
                case PlayerState.Idle:
                case PlayerState.Moving:
                case PlayerState.Falling:
                {
                    if (playerController.PlayerState != PlayerState.Attacked)
                    {
                        GetInputOrdinary();
                    }
                    break;
                }
                case PlayerState.Rope:
                case PlayerState.Ladder:
                {
                    GetInputOnRopeOrLadder();
                    break;
                }
            }
        }

        private void GetInputOrdinary()
        {
            if (Input.GetKeyDown(jumpKey) && playerController.PlayerState != PlayerState.Falling)
            {
                playerController.Jump();
            }

            var direction = GetHorizontalDirection();
            playerController.Move(direction);
        }

        private void GetInputOnRopeOrLadder()
        {
            if (Input.GetKeyDown(jumpKey))
            {
                playerController.JumpFromRopeOrLadder(GetHorizontalDirection());
            }

            var direction = GetVerticalDirection();

            switch (playerController.PlayerState)
            {
                case PlayerState.Rope:
                {
                    playerController.MoveOnRope(direction);
                    break;
                }
                case PlayerState.Ladder:
                {
                    playerController.MoveOnLadder(direction);
                    break;
                }
            }
        }

        private Directions GetHorizontalDirection()
        {
            const string AXIS_NAME = "Horizontal";

            if (Input.GetAxisRaw(AXIS_NAME) == 0)
            {
                return Directions.None;
            }

            return Input.GetAxisRaw(AXIS_NAME) > 0 ? Directions.Right : Directions.Left;
        }

        private Directions GetVerticalDirection()
        {
            const string AXIS_NAME = "Vertical";

            if (Input.GetAxisRaw(AXIS_NAME) == 0)
            {
                return Directions.None;
            }

            return Input.GetAxisRaw(AXIS_NAME) > 0 ? Directions.Up : Directions.Down;
        }
    }*/
}
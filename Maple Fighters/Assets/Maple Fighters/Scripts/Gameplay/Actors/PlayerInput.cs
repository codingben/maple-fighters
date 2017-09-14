using CommonTools.Log;
using Shared.Game.Common;
using UnityEngine;

namespace Scripts.Gameplay.Actors
{
    public class PlayerInput : MonoBehaviour
    {
        [Header("Keyboard")]
        [SerializeField] private string axisName;
        [SerializeField] private KeyCode jumpKey;

        private PlayerController playerController;

        private void Awake()
        {
            playerController = GetComponent<PlayerController>().AssertNotNull();
        }

        private void Update()
        {
            if (Input.GetKeyDown(jumpKey))
            {
                playerController.Jump();
            }

            playerController.Move(GetDirectionByInput());
        }

        private Directions GetDirectionByInput()
        {
            if (Input.GetAxisRaw(axisName) == 0)
            {
                return Directions.None;
            }

            return Input.GetAxisRaw(axisName) > 0 ? Directions.Right : Directions.Left;
        }
    }
}
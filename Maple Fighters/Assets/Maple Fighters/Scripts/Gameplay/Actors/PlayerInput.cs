using System.Collections.Generic;
using CommonTools.Coroutines;
using CommonTools.Log;
using Scripts.Coroutines;
using UnityEngine;

namespace Scripts.Gameplay.Actors
{
    public class PlayerInput : MonoBehaviour
    {
        [Header("Keyboard")]
        [SerializeField] private KeyCode moveLeftKey;
        [SerializeField] private KeyCode moveRightKey;
        [SerializeField] private KeyCode jumpKey;

        private Vector2 moveDirection;

        private PlayerController playerController;
        private ExternalCoroutinesExecutor coroutinesExecuter;

        private void Awake()
        {
            playerController = GetComponent<PlayerController>().AssertNotNull();
            coroutinesExecuter = new ExternalCoroutinesExecutor().ExecuteExternally();
        }

        private void Start() => coroutinesExecuter.StartCoroutine(PlayerInputListener());

        private IEnumerator<IYieldInstruction> PlayerInputListener()
        {
            while (true)
            {
                moveDirection = GetDirectionByInput();

                if (moveDirection != Vector2.zero)
                {
                    playerController?.Move(moveDirection);
                }

                yield return null;
            }
        }

        private Vector2 GetDirectionByInput()
        {
            if (Input.GetKey(moveLeftKey))
            {
                return Vector2.left;
            }

            if (Input.GetKey(moveRightKey))
            {
                return Vector2.right;
            }

            return Vector2.zero;
        }
    }
}
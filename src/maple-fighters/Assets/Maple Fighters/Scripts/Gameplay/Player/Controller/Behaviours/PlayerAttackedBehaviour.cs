using System.Collections;
using Game.Messages;
using Scripts.Gameplay.Entity;
using Scripts.Services;
using Scripts.Services.GameApi;
using UnityEngine;

namespace Scripts.Gameplay.Player.Behaviours
{
    [RequireComponent(typeof(PlayerController))]
    public class PlayerAttackedBehaviour : MonoBehaviour
    {
        [SerializeField]
        private float attackDelay = 3;

        [SerializeField]
        private Vector2 hitAmount;

        [SerializeField]
        private Animation playerAttackedAnim;

        private IGameApi gameApi;
        private PlayerController playerController;
        private bool isAttacked;

        private void Awake()
        {
            playerController = GetComponent<PlayerController>();
        }

        private void Start()
        {
            gameApi = ApiProvider.ProvideGameApi();
            gameApi.Attacked.AddListener(OnAttacked);
        }

        private void OnDisable()
        {
            gameApi?.Attacked?.RemoveListener(OnAttacked);
        }

        private void OnAttacked(AttackedMessage message)
        {
            var entity =
                EntityContainer.GetInstance().GetLocalEntity();
            var spawnedCharacter =
                entity?.GameObject.GetComponent<ISpawnedCharacter>();
            var character =
                spawnedCharacter?.GetCharacter();
            if (character != null)
            {
                var normalized =
                    (character.transform.position - transform.position).normalized;
                var direction = new Vector2(
                    x: (normalized.x > 0 ? 1 : -1) * hitAmount.x,
                    y: hitAmount.y);

                StartAttackedEffect(direction);
            }
        }

        private void StartAttackedEffect(Vector3 direction)
        {
            if (playerController.GetPlayerState() == PlayerStates.Idle ||
                playerController.GetPlayerState() == PlayerStates.Moving ||
                playerController.GetPlayerState() == PlayerStates.PrimaryAttack ||
                playerController.GetPlayerState() == PlayerStates.SecondaryAttack)
            {
                if (isAttacked)
                {
                    return;
                }

                isAttacked = true;

                StartCoroutine(WaitFrameAndBounce(direction));
            }
        }

        private IEnumerator WaitFrameAndBounce(Vector3 direction)
        {
            // NOTE: One frame delay to prevent an exception in the game server (in World.Step)
            yield return null;

            playerController.Bounce(direction);

            StartCoroutine(PlayAttackedAnimAndWait());
        }

        private IEnumerator PlayAttackedAnimAndWait()
        {
            playerAttackedAnim.Play();

            yield return new WaitForSeconds(attackDelay);

            playerAttackedAnim.Stop();

            isAttacked = false;
        }
    }
}
using Game.Messages;
using Scripts.Constants;
using Scripts.Gameplay.Entity;
using Scripts.Gameplay.Map.Objects;
using Scripts.Services;
using Scripts.Services.GameApi;
using UnityEngine;

namespace Scripts.Gameplay.Player
{
    public class PlayerAttackMessageSender : MonoBehaviour
    {
        private IGameApi gameApi;
        private ISpawnedCharacter spawnedCharacter;
        private PlayerController playerController;

        private void Awake()
        {
            spawnedCharacter = GetComponent<ISpawnedCharacter>();
            spawnedCharacter.CharacterSpawned += OnCharacterSpawned;
        }

        private void Start()
        {
            gameApi = ApiProvider.ProvideGameApi();
        }

        private void OnCharacterSpawned()
        {
            playerController = spawnedCharacter
                .GetCharacter()
                .GetComponent<PlayerController>();
            if (playerController)
            {
                playerController.PlayerStateChanged += OnPlayerStateChanged;
            }
        }

        private void OnPlayerStateChanged(PlayerStates playerState)
        {
            if (playerState != PlayerStates.PrimaryAttack)
            {
                return;
            }

            var distance =
                1.5f;
            var damageAmount =
                Random.Range(0, 50);
            var direction =
                GetPlayerDirection();
            var raycasts =
                Physics2D.RaycastAll(transform.position, direction, distance);
            foreach (var raycast in raycasts)
            {
                var isMob =
                    raycast.transform.gameObject.CompareTag(GameTags.MobTag);
                if (isMob)
                {
                    var mob = raycast.transform.gameObject;
                    var mobAttackedEffect = mob.GetComponent<MobAttackedEffect>();
                    var mobEntity = mob.transform.parent.GetComponent<IEntity>();
                    var id = mobEntity?.Id ?? -1;
                    var message = new AttackMobMessage
                    {
                        MobId = id,
                        DamageAmount = damageAmount
                    };

                    mobAttackedEffect?.PlayAttackedEffect();
                    gameApi?.SendMessage(MessageCodes.AttackMob, message);
                }
            }
        }

        private Vector2 GetPlayerDirection()
        {
            var direction = Vector2.zero;

            var playerDirection = playerController.GetDirection();
            if (playerDirection == Direction.Left)
            {
                direction = Vector3.left;
            }
            else if (playerDirection == Direction.Right)
            {
                direction = Vector3.right;
            }

            return direction;
        }
    }
}
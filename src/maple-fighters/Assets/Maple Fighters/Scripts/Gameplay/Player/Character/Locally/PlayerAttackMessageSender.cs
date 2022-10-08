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
        [SerializeField]
        private float distance = 1.5f;

        [SerializeField]
        private int minDamageAmount = 0;

        [SerializeField]
        private int maxDamageAmount = 50;

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
            if (playerState == PlayerStates.PrimaryAttack)
            {
                Attack();
            }
            else if (playerState == PlayerStates.SecondaryAttack)
            {
                if (playerController.GetCharacterType() == CharacterClasses.Knight)
                {
                    Attack();
                }
            }
        }

        private void Attack()
        {
            var damageAmount =
                Random.Range(minDamageAmount, maxDamageAmount);
            var direction =
                playerController.GetDirection();
            var raycasts =
                Physics2D.RaycastAll(transform.position, direction, distance);
            foreach (var raycast in raycasts)
            {
                var isMob =
                    raycast.transform.gameObject.CompareTag(GameTags.MobTag);
                if (isMob)
                {
                    var mob = raycast.transform.gameObject;
                    var mobEntity = mob.transform.parent.GetComponent<IEntity>();
                    var mobBehaviour = mob.transform.parent.GetComponent<MobBehaviour>();

                    mobBehaviour?.Attack(damageAmount);

                    var id = mobEntity?.Id ?? -1;
                    var message = new AttackMobMessage
                    {
                        MobId = id,
                        DamageAmount = damageAmount
                    };

                    gameApi?.SendMessage(MessageCodes.AttackMob, message);
                }
            }
        }
    }
}